using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FladeUp_API.Migrations
{
    /// <inheritdoc />
    public partial class work : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkExp",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkExp",
                table: "AspNetUsers");
        }
    }
}
