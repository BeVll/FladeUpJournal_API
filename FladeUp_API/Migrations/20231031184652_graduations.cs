using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FladeUp_API.Migrations
{
    /// <inheritdoc />
    public partial class graduations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Diplom",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatureCerfiticate",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReleaseDateSchool",
                table: "AspNetUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReleaseDateUniver",
                table: "AspNetUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subspecialization",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TerminationYearSchool",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TerminationYearUniver",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeOfExam",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniversityName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diplom",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MatureCerfiticate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReleaseDateSchool",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReleaseDateUniver",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SchoolName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Subspecialization",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TerminationYearSchool",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TerminationYearUniver",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TypeOfExam",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UniversityName",
                table: "AspNetUsers");
        }
    }
}
