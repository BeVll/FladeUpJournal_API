using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FladeUp_API.Migrations
{
    /// <inheritdoc />
    public partial class groupsUPD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormOfStudy",
                table: "Groups",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormOfStudy",
                table: "Groups");
        }
    }
}
