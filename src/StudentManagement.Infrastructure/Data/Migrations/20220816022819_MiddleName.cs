using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    public partial class MiddleName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_LastName_FirstName_DateOfBirth",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Students_LastName_FirstName_DateOfBirth_MiddleName",
                table: "Students",
                columns: new[] { "LastName", "FirstName", "DateOfBirth", "MiddleName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_LastName_FirstName_DateOfBirth_MiddleName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Students");

            migrationBuilder.CreateIndex(
                name: "IX_Students_LastName_FirstName_DateOfBirth",
                table: "Students",
                columns: new[] { "LastName", "FirstName", "DateOfBirth" },
                unique: true);
        }
    }
}
