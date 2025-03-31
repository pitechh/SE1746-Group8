using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementService.Migrations
{
    public partial class InitialCreateV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoFileName",
                table: "Lessons");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoFileName",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
