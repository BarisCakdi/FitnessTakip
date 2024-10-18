using FitnessTakip.Data;
using FitnessTakip.DTOs;
using FitnessTakip.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTakip.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EducationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EducationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<dtoListEducationRequest> GetEducations()
        {

            List<dtoListEducationRequest> dto = _context.Educations.Select(x => new dtoListEducationRequest()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Created = x.Created,
                TeacherName = x.Teacher.Name
            }).ToList();

            return dto;
        }

        [HttpGet("{id}")]
        public dtoListEducationRequest getEducation(int id)
        {
            var education = _context.Educations.Find(id);
            if (education is null)
                return new dtoListEducationRequest();
            return new dtoListEducationRequest
            {
                Id = education.Id,
                Name = education.Name,
                Description = education.Description,
                Created = education.Created,
                TeacherName = education.Teacher.Name
            };
        }

        [HttpPost]
        public ActionResult<dtoAddEducationRequest> AddEducation([FromBody] dtoAddEducationRequest education)
        {
            if (education.TeacherId == null || education.TeacherId == 0 || !_context.Teachers.Any(t => t.Id == education.TeacherId))
            {
                return BadRequest("Geçersiz veya eksik eğitmen ID'si. Lütfen geçerli bir eğitmen ID'si girin.");
            }

            if (education.Id is not 0)
            {
                var educationToUpdate = _context.Educations.Find(education.Id);
                if (educationToUpdate is null)
                    return NotFound();

                educationToUpdate.Name = education.Name;
                educationToUpdate.Description = education.Description;
                educationToUpdate.TeacherId = education.TeacherId;

                _context.Educations.Update(educationToUpdate);
                _context.SaveChanges();

                return new dtoAddEducationRequest
                {
                    Id = educationToUpdate.Id,
                    Name = educationToUpdate.Name,
                    Description = educationToUpdate.Description,
                    TeacherId = educationToUpdate.TeacherId
                };
            }
            else
            {
                var newEducation = new Education
                {
                    Name = education.Name,
                    Description = education.Description,
                    TeacherId = education.TeacherId
                };

                _context.Educations.Add(newEducation);
                _context.SaveChanges();

                return new dtoAddEducationRequest
                {
                    Id = newEducation.Id,
                    Name = newEducation.Name,
                    Description = newEducation.Description,
                    TeacherId = newEducation.TeacherId
                };
            }
        }


        [HttpDelete("{id}")]
        public bool DeleteEducation(int id)
        {
            var education = _context.Educations.Find(id);
            if (education is null)
                return false;
            _context.Educations.Remove(education);
            _context.SaveChanges();
            return true;
        }

        [HttpGet("{id}")]
        public ActionResult<dtoEducationDetailWithEarnings> GetEducationDetails(int id)
        {
            var education = _context.Educations.Include(e => e.Teacher)
                                                .FirstOrDefault(e => e.Id == id);

            if (education == null)
                return NotFound("Geçersiz id girişi");

            var monthlyEarnings = new List<dtoMonthlyEarnings>();
            
            for (int month = 1; month <= 12; month++)
            {
                var earnings = _context.UserPrograms
                    .Where(up => up.EducationId == education.Id && up.KayitTarih.Month == month)
                    .Sum(up => up.Price); 

                monthlyEarnings.Add(new dtoMonthlyEarnings
                {
                    Month = new DateTime(2024, month, 1).ToString("MMMM yyyy"), 
                    Earnings = earnings
                });
            }

            var registeredUsers = _context.UserPrograms
                .Where(up => up.EducationId == education.Id)
                .Select(up => new dtoRegisteredUser
                {
                    UserName = up.User.Name 
                }).ToList();

            return new dtoEducationDetailWithEarnings
            {
                EducationName = education.Name,
                TeacherName = education.Teacher.Name,
                MonthlyEarnings = monthlyEarnings,
                RegisteredUsers = registeredUsers
            };
        }
        [HttpGet]
        public ActionResult<List<dtoFilteredEducationList>> GetFilteredEducations([FromQuery] dtoFilterRequest filter)
        {
            var query = _context.Educations.Include(e => e.Teacher).AsQueryable();

            if (filter.TeacherId.HasValue)
            {
                query = query.Where(e => e.TeacherId == filter.TeacherId.Value);
            }

            if (filter.EducationId.HasValue)
            {
                query = query.Where(e => e.Id == filter.EducationId.Value);
            }

            if (filter.MinDate.HasValue)
            {
                
                query = query.Where(e => e.Created >= filter.MinDate.Value);
            }

            if (filter.MaxDate.HasValue)
            {
                query = query.Where(e => e.Created <= filter.MaxDate.Value);
            }

            var result = query.Select(e => new dtoFilteredEducationList
            {
                TeacherName = e.Teacher.Name,
                EducationName = e.Name,
                TotalEarnings = _context.UserPrograms.Where(up => up.EducationId == e.Id).Sum(up => up.Price),
                RegisteredStudentCount = _context.UserPrograms.Count(up => up.EducationId == e.Id)
            }).ToList();

            return result;
        }

    }
}
