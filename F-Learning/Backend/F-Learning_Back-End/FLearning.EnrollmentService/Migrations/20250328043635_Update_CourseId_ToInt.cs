using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLearning.EnrollmentService.Migrations
{
    /// <inheritdoc />
    public partial class Update_CourseId_ToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    enrolled_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    total_courses_enrolled = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    completed_courses = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__students__3213E83F2AB97E7D", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "course_progress",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    student_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    course_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    progress_percentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    last_accessed = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__course_p__3213E83F022232C9", x => x.id);
                    table.ForeignKey(
                        name: "FK__course_pr__stude__33D4B598",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "enrollments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    student_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    enrolled_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__enrollme__3213E83F420A6D34", x => x.id);
                    table.ForeignKey(
                        name: "FK__enrollmen__stude__2E1BDC42",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_progress_student_id",
                table: "course_progress",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_student_id",
                table: "enrollments",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "UQ__students__B9BE370E2550203D",
                table: "students",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_progress");

            migrationBuilder.DropTable(
                name: "enrollments");

            migrationBuilder.DropTable(
                name: "students");
        }
    }
}
