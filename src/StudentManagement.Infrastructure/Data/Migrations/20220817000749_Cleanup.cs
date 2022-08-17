using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    public partial class Cleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolEnrollments",
                table: "SchoolEnrollments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolEnrollments",
                table: "SchoolEnrollments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolEnrollments_StudentId_SchoolId",
                table: "SchoolEnrollments",
                columns: new[] { "StudentId", "SchoolId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolEnrollments",
                table: "SchoolEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_SchoolEnrollments_StudentId_SchoolId",
                table: "SchoolEnrollments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolEnrollments",
                table: "SchoolEnrollments",
                columns: new[] { "StudentId", "SchoolId" });
        }
    }
}
