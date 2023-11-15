using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FladeUp_API.Migrations
{
    /// <inheritdoc />
    public partial class deletepostsandtags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departaments_DepartamentId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "PostsMedias");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Courses_DepartamentId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "DepartamentId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Courses",
                newName: "SpecilizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SpecilizationId",
                table: "Courses",
                column: "SpecilizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Specializations_SpecilizationId",
                table: "Courses",
                column: "SpecilizationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Specializations_SpecilizationId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SpecilizationId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "SpecilizationId",
                table: "Courses",
                newName: "DepartmentId");

            migrationBuilder.AddColumn<int>(
                name: "DepartamentId",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PostText = table.Column<string>(type: "text", nullable: true),
                    PostTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TagIds = table.Column<List<int>>(type: "integer[]", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostsMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostId = table.Column<int>(type: "integer", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostsMedias_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PostEntityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Posts_PostEntityId",
                        column: x => x.PostEntityId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DepartamentId",
                table: "Courses",
                column: "DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostsMedias_PostId",
                table: "PostsMedias",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_PostEntityId",
                table: "Tags",
                column: "PostEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departaments_DepartamentId",
                table: "Courses",
                column: "DepartamentId",
                principalTable: "Departaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
