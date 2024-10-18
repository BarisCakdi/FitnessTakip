using FitnessTakip.Data;
using FitnessTakip.DTOs;
using FitnessTakip.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]/[action]")]
public class TeacherController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeacherController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public List<dtoListTeachersRequest> GetTeachers()
    {

        List<dtoListTeachersRequest> dto = _context.Teachers.Select(x => new dtoListTeachersRequest()
        {
            Id = x.Id,
            Name = x.Name,
            Created = x.Created
        }).ToList();

        return dto;
    }

    [HttpGet("{id}")]
    public dtoListTeachersRequest getTeacher(int id)
    {
        var teacher = _context.Teachers.Find(id);
        if (teacher is null)
            return new dtoListTeachersRequest();
        return new dtoListTeachersRequest
        {
            Id = teacher.Id,
            Name = teacher.Name,
            Created = teacher.Created
        };
    }

    [HttpPost]
    public dtoListTeachersRequest AddTeacher([FromBody] dtoListTeachersRequest teacher)
    {
        if (teacher.Id is not 0)
        {
            var teacherToUpdate = _context.Teachers.Find(teacher.Id);
            if (teacherToUpdate is null)
                return new dtoListTeachersRequest();
            teacherToUpdate.Name = teacher.Name;
            teacherToUpdate.Created = teacher.Created;
            _context.Teachers.Update(teacherToUpdate);
            _context.SaveChanges();
            return new dtoListTeachersRequest
            {
                Id = teacherToUpdate.Id,
                Name = teacherToUpdate.Name,
                Created = teacherToUpdate.Created
            };
        }
        else
        {
            var newTeacher = new Teacher
            {
                Name = teacher.Name,
                Created = teacher.Created
            };
            _context.Teachers.Add(newTeacher);
            _context.SaveChanges();
            return new dtoListTeachersRequest
            {
                Id = newTeacher.Id,
                Name = newTeacher.Name,
                Created = newTeacher.Created
            };
        }
    }


    [HttpDelete("{id}")]
    public bool DeleteTeacher(int id)
    {
        var teacher = _context.Teachers.Find(id);
        if (teacher is null)
            return false;
        _context.Teachers.Remove(teacher);
        _context.SaveChanges();
        return true;
    }
}
