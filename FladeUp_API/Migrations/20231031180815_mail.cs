using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FladeUp_API.Migrations
{
    /// <inheritdoc />
    public partial class mail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MailCity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailCountry",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailPostalCode",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailStreet",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailCity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MailCountry",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MailPostalCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MailStreet",
                table: "AspNetUsers");
        }
    }
}
