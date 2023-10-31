﻿namespace FladeUp_Api.Requests
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Image { get; set; }
        public string? IndetificateCode { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Sex { get; set; }
        public string National { get; set; }
        public string? Passport { get; set; }
        public bool IsLightTheme { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }

        public string? BankAccount { get; set; }
        
    }
}
