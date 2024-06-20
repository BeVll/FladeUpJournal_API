using FladeUp_API.Models.Nationality;
using FladeUp_API.Models.Sex;

namespace FladeUp_API.Constants
{
    public static class Gender
    {
        public static List<GenderModel> All = new()
            {
                new GenderModel { Id = 1, NameEn = "Male", NameUk = "Чоловіча" },
                new GenderModel { Id = 2, NameEn = "Female", NameUk = "Жіноча" },
                new GenderModel { Id = 3, NameEn = "Other", NameUk = "Інша" },
            };

    }
}
