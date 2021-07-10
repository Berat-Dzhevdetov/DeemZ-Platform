using Microsoft.EntityFrameworkCore.Migrations;

namespace DeemZ.Data.Migrations
{
    public partial class AddedCreditsToCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Credits",
                table: "ApplicationUserExams",
                newName: "EarnedPoints");

            migrationBuilder.AddColumn<int>(
                name: "Credits",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EarnedCredits",
                table: "ApplicationUserExams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credits",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "EarnedCredits",
                table: "ApplicationUserExams");

            migrationBuilder.RenameColumn(
                name: "EarnedPoints",
                table: "ApplicationUserExams",
                newName: "Credits");
        }
    }
}
