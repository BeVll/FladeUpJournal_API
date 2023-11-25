using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FladeUp_API.Migrations
{
    /// <inheritdoc />
    public partial class subjectcolor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Subjects",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Subjects");
        }
    }
}
