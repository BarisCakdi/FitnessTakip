using FitnessTakip.Data;
using FitnessTakip.DTOs;
using FitnessTakip.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTakip.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserProgramsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserProgramsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<UserProgram> GetUserPrograms()
        {
            return _context.UserPrograms
                .Include(p => p.User)
                .Include(x => x.Education).ThenInclude(x => x.Teacher).ToList();
        }

        [HttpGet("{id}")]
        public UserProgram GetUserProgram(int id)
        {
            return _context.UserPrograms
                .Include(p => p.User)
                .Include(x => x.Education).ThenInclude(x => x.Teacher)
                .FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public dtoAddUserProgramRequest AddUserProgram([FromBody] dtoAddUserProgramRequest userProgram)
        {
            if (userProgram.Id is not 0)
            {
                var userProgramToUpdate = _context.UserPrograms.Find(userProgram.Id);
                if (userProgramToUpdate is null)
                    return new dtoAddUserProgramRequest();
                userProgramToUpdate.UserId = userProgram.UserId;
                userProgramToUpdate.EducationId = userProgram.EducationId;
                _context.UserPrograms.Update(userProgramToUpdate);
                _context.SaveChanges();
                return new dtoAddUserProgramRequest
                {
                    Id = userProgramToUpdate.Id,
                    Price = userProgramToUpdate.Price,
                    Credi = userProgramToUpdate.Credi,
                    UserId = userProgramToUpdate.UserId,
                    EducationId = userProgramToUpdate.EducationId
                };
            }
            var newUserProgram = new UserProgram
            {
                Price = userProgram.Price,
                Credi = userProgram.Credi,
                UserId = userProgram.UserId,
                EducationId = userProgram.EducationId
            };
            _context.UserPrograms.Add(newUserProgram);
            _context.SaveChanges();
            return new dtoAddUserProgramRequest
            {
                Id = newUserProgram.Id,
                Price = newUserProgram.Price,
                Credi = newUserProgram.Credi,
                UserId = newUserProgram.UserId,
                EducationId = newUserProgram.EducationId
            };
        }

        [HttpDelete("{id}")]
        public bool DeleteUserProgram(int id)
        {
            var userProgram = _context.UserPrograms.Find(id);
            if (userProgram is null)
                return false;
            _context.UserPrograms.Remove(userProgram);
            _context.SaveChanges();
            return true;
        }

        [HttpGet("check-permission/{userId}/{educationId}")]
        public IActionResult CheckLessonPermission(int userId, int educationId)
        {
            var userProgram = _context.UserPrograms
                .Include(up => up.User)
                .Include(up => up.Education)
                .ThenInclude(e => e.Teacher) 
                .FirstOrDefault(up => up.UserId == userId && up.EducationId == educationId);

            if (userProgram == null)
            {
                return NotFound("Kullanıcı ya da eğitim programı bulunamadı.");
            }

            var hasPermission = userProgram.HasEntryPermission();

            return Ok(new
            {
                PermissionGranted = hasPermission ? "İzin verildi" : "İzin verilmedi",
                RemainingCredits = userProgram.Credi,
                UserName = userProgram.User?.Name, 
                EducationName = userProgram.Education?.Name,
                TeacherName = userProgram.Education?.Teacher?.Name 
            });
        }
        [HttpGet("entries-by-date/{startDate}/{endDate}")]
        public IActionResult GetEntriesByDate(DateTime startDate, DateTime endDate)
        {
            var entries = _context.UserPrograms
                .Include(up => up.User)
                .Include(up => up.Education)
                .ThenInclude(e => e.Teacher)
                .Where(up => up.KayitTarih >= startDate && up.KayitTarih <= endDate)
                .OrderByDescending(up => up.KayitTarih)
                .Select(up => new
                {
                    UserName = up.User.Name,
                    EducationName = up.Education.Name,
                    TeacherName = up.Education.Teacher.Name,
                    DateAndTime = up.KayitTarih,
                    RemainingCredits = up.Credi
                })
                .ToList();

            if (!entries.Any())
            {
                return NotFound("Belirtilen tarih aralığında giriş kaydı bulunamadı.");
            }

            return Ok(entries);
        }

        [HttpGet("teacher/{teacherId}")]
        public IActionResult GetTeacherWithPrograms(int teacherId)
        {
            var teacher = _context.Teachers
                .Include(t => t.Educations)
                    .ThenInclude(e => e.UserPrograms)
                .FirstOrDefault(t => t.Id == teacherId);

            if (teacher == null)
            {
                return NotFound("Eğitmen bulunamadı.");
            }

            var educationSummaries = teacher.Educations.Select(e => new EducationSummaryDto
            {
                EducationName = e.Name,
                StudentCount = e.UserPrograms.Count()
            }).ToList();

            var result = new
            {
                TeacherName = teacher.Name,
                TotalPrograms = teacher.Educations.Count,
                EducationSummaries = educationSummaries
            };

            return Ok(result);
        }
        


    }
}
