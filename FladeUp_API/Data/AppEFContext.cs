using FladeUp_Api.Data.Entities;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using FladeUp_API.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FladeUp_Api.Data
{
    public class AppEFContext : IdentityDbContext<UserEntity, RoleEntity, int,
        IdentityUserClaim<int>, UserRoleEntity, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        public AppEFContext(DbContextOptions<AppEFContext> options)
            : base(options)
        {
            
        }

        public DbSet<ClassEntity> Classes { get; set; }
        public DbSet<UserClassEntity> UserClasses { get; set; }
        public DbSet<UserAdresses> UserAdresses { get; set; }

        public DbSet<SubjectEnitity> Subjects { get; set; }
        public DbSet<ClassSubjectsEnitity> ClassSubjects { get; set; }

        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<EventEnitity> Events { get; set; }
        public DbSet<EventClassesEntity> EventClasses { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserRoleEntity>(ur =>
            {
                ur.HasKey(ur => new { ur.UserId, ur.RoleId });

                ur.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(r => r.RoleId)
                    .IsRequired();

                ur.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(u => u.UserId)
                    .IsRequired();
            });

        }
    }
}
