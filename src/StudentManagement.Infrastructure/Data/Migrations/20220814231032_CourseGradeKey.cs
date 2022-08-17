using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    public partial class CourseGradeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseGrades",
                table: "CourseGrades");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CourseGrades",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseGrades",
                table: "CourseGrades",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGrades_CourseId_StudentId",
                table: "CourseGrades",
                columns: new[] { "CourseId", "StudentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseGrades",
                table: "CourseGrades");

            migrationBuilder.DropIndex(
                name: "IX_CourseGrades_CourseId_StudentId",
                table: "CourseGrades");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CourseGrades");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseGrades",
                table: "CourseGrades",
                columns: new[] { "CourseId", "StudentId" });
        }
    }
}
