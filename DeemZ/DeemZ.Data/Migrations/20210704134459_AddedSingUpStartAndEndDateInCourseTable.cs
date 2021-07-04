using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeemZ.Data.Migrations
{
    public partial class AddedSingUpStartAndEndDateInCourseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Exams");

            migrationBuilder.AddColumn<DateTime>(
                name: "SignUpEndDate",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SignUpStartDate",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "InformativeMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShowTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformativeMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InformativeMessages");

            migrationBuilder.DropColumn(
                name: "SignUpEndDate",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SignUpStartDate",
                table: "Courses");

            migrationBuilder.AddColumn<DateTime>(
                name: "MyProperty",
                table: "Exams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
