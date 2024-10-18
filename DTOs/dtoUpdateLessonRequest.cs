namespace FitnessTakip.DTOs
{
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
    public class dtoEducationDetailWithEarnings
    {
        public string EducationName { get; set; }
        public string TeacherName { get; set; }
        public List<dtoMonthlyEarnings> MonthlyEarnings { get; set; }
        public List<dtoRegisteredUser> RegisteredUsers { get; set; }
    }

    public class dtoMonthlyEarnings
    {
        public string Month { get; set; }
        public decimal Earnings { get; set; }
    }

    public class dtoRegisteredUser
    {
        public string UserName { get; set; }
    }
    public class dtoFilteredEducationList
    {
        public string TeacherName { get; set; }
        public string EducationName { get; set; }
        public decimal TotalEarnings { get; set; }
        public int RegisteredStudentCount { get; set; }
    }

    public class dtoFilterRequest
    {
        public int? TeacherId { get; set; }
        public int? EducationId { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }

}
