using FladeUp_Api.Constants;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace FladeUp_Api.Data
{
    public static class SeederDB
    {
        public static async void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var service = scope.ServiceProvider;
                var context = service.GetRequiredService<AppEFContext>();
                var userNamager = service.GetRequiredService<UserManager<UserEntity>>();
                var roleNamager = service.GetRequiredService<RoleManager<RoleEntity>>();
                context.Database.EnsureCreated();


                if (!context.Roles.Any())
                {
                    foreach (string name in Roles.All)
                    {
                        var role = new RoleEntity
                        {
                            Name = name
                        };
                        var result = roleNamager.CreateAsync(role).Result;
                    }
                }
                if (!context.Users.Any())
                {
                    
                    
                        var user = new UserEntity
                        {
                            Firstname = "Admin",
                            Lastname = "Admin",
                            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow),
                            PlaceOfBirth = "None",
                            GenderId = 3,
                            NationalityId = 1,
                            IsLightTheme = false,
                            Email = "admin@gmail.com",
                            UserName = "admin@gmail.com",
                            Status = "Admin", 
                            EmailConfirmed = true
                        };
                        var result = userNamager.CreateAsync(user).Result;

                    UserAdresses userAdresses = new UserAdresses
                    {
                        UserId = user.Id
                    };

                    context.UserAdresses.Add(userAdresses);
                   

                    await userNamager.AddToRoleAsync(user, "Admin");
                    await userNamager.AddPasswordAsync(user, "admin");
                    await userNamager.UpdateAsync(user);
                    await context.SaveChangesAsync();
                }


            }
        }
    }
}
