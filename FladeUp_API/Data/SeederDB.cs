using FladeUp_Api.Constants;
using FladeUp_Api.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace FladeUp_Api.Data
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var service = scope.ServiceProvider;
                var context = service.GetRequiredService<AppEFContext>();
                var userNamager = service.GetRequiredService<UserManager<UserEntity>>();
                var roleNamager = service.GetRequiredService<RoleManager<RoleEntity>>();
                // context.Database.Migrate(); //автоматично запускає міграції на БД


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



            }
        }
    }
}
