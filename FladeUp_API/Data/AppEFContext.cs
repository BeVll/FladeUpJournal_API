using FladeUp_Api.Data.Entities;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
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

        public DbSet<TagEnitity> Tags { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<PostMediaEntity> PostsMedias { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<DepartmentEntity> Departaments { get; set; }
        public DbSet<CourseEntity> Courses { get; set; }
        public DbSet<SpecializationEntity> Specializations { get; set; }

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
