using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    public partial class RemoveMiddleNameFromIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_LastName_FirstName_DateOfBirth_MiddleName",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Students_LastName_FirstName_DateOfBirth",
                table: "Students",
                columns: new[] { "LastName", "FirstName", "DateOfBirth" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_LastName_FirstName_DateOfBirth",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Students_LastName_FirstName_DateOfBirth_MiddleName",
                table: "Students",
                columns: new[] { "LastName", "FirstName", "DateOfBirth", "MiddleName" },
                unique: true);
        }
    }
}
