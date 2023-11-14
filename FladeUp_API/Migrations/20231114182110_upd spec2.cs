using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FladeUp_API.Migrations
{
    /// <inheritdoc />
    public partial class updspec2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specializations_Departaments_DepartamentId",
                table: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_Specializations_DepartamentId",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "DepartamentId",
                table: "Specializations");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_DepartmentId",
                table: "Specializations",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specializations_Departaments_DepartmentId",
                table: "Specializations",
                column: "DepartmentId",
                principalTable: "Departaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specializations_Departaments_DepartmentId",
                table: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_Specializations_DepartmentId",
                table: "Specializations");

            migrationBuilder.AddColumn<int>(
                name: "DepartamentId",
                table: "Specializations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_DepartamentId",
                table: "Specializations",
                column: "DepartamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specializations_Departaments_DepartamentId",
                table: "Specializations",
                column: "DepartamentId",
                principalTable: "Departaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
