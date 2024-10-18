using FitnessTakip.Data;
using FitnessTakip.DTOs;
using FitnessTakip.Model;
using Microsoft.AspNetCore.Mvc;

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
    }
}
