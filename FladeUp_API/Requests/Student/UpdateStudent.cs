namespace FladeUp_API.Requests.Student
{
    public class UpdateStudent
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public IFormFile? NewImage { get; set; }
        public string? IndetificateCode { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public int GenderId { get; set; }
        public int NationalityId { get; set; }
        public string? Passport { get; set; }

        public string? WorkExp { get; set; }

        public string? BankAccount { get; set; }

        public string Status { get; set; }
    }
}
