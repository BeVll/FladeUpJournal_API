namespace FladeUp_API.Requests.Class
{
    public class ClassCreateRequest
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FormOfStudy { get; set; }
        public int YearOfStart { get; set; }
        public int YearOfEnd { get; set; }
        public string? ClassSpecialization { get; set; }
    }
}
