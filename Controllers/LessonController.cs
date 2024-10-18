using FitnessTakip.Data;
using FitnessTakip.DTOs;
using FitnessTakip.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]/[action]")]
public class LessonController : ControllerBase
{
    private readonly AppDbContext _context;

    public LessonController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public List<Lesson> GetLessons()
    {
        return _context.Lessons.ToList();
    }

    [HttpGet("{id}")]
    public Lesson getLesson(int id)
    {
        var lesson = _context.Lessons.Find(id);
        if (lesson is null)
            return new Lesson();
        return lesson;
    }

    [HttpPost]
    public Lesson AddLesson([FromBody] dtoUpdateLessonRequest lesson)
    {
        if (lesson.Id is not 0)
        {

            var lessonToUpdate = _context.Lessons.Find(lesson.Id);
            if (lessonToUpdate is null)
                return new Lesson();
            lessonToUpdate.Name = lesson.Name;
            lessonToUpdate.Description = lesson.Description;
            _context.Lessons.Update(lessonToUpdate);
            _context.SaveChanges();
            return lessonToUpdate;

        }
        var newLesson = new Lesson
        {
            Name = lesson.Name,
            Description = lesson.Description,
            CreatedDate = DateTime.Now,
            CreatedUserId = 1
        };
        _context.Lessons.Add(newLesson);
        _context.SaveChanges();
        return newLesson;
    }

    [HttpDelete("{id}")]
    public bool DeleteLesson(int id)
    {
        var lesson = _context.Lessons.Find(id);
        if (lesson is null)
            return false;
        _context.Lessons.Remove(lesson);
        _context.SaveChanges();
        return true;
    }

}
