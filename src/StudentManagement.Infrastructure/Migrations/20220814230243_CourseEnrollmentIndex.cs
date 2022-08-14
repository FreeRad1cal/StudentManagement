using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    public partial class CourseEnrollmentIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseEnrollments",
                table: "CourseEnrollments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseEnrollments",
                table: "CourseEnrollments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_CourseId_StudentId",
                table: "CourseEnrollments",
                columns: new[] { "CourseId", "StudentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseEnrollments",
                table: "CourseEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_CourseEnrollments_CourseId_StudentId",
                table: "CourseEnrollments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseEnrollments",
                table: "CourseEnrollments",
                columns: new[] { "CourseId", "StudentId" });
        }
    }
}
