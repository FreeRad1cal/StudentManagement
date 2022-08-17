using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    public partial class CourseName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_SchoolId_Year",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SchoolId_Name_Year",
                table: "Courses",
                columns: new[] { "SchoolId", "Name", "Year" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_SchoolId_Name_Year",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Courses");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SchoolId_Year",
                table: "Courses",
                columns: new[] { "SchoolId", "Year" },
                unique: true);
        }
    }
}
