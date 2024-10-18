namespace FitnessTakip.DTOs
{
    public class dtoUpdateLessonRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class dtoListUsersRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }

    public class dtoListTeachersRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }

    public class dtoListEducationRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string TeacherName { get; set; }
    }

    public class dtoAddEducationRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TeacherId { get; set; }
    }

    public class dtoAddUserProgramRequest
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Credi { get; set; }
        public int UserId { get; set; }
        public int EducationId { get; set; }
    }

    public class EducationSummaryDto
    {
        public string EducationName { get; set; }
        public int StudentCount { get; set; }
    }
}
