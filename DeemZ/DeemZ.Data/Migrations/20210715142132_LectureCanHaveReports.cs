using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeemZ.Data.Migrations
{
    public partial class LectureCanHaveReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Reports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LectureId",
                table: "Reports",
                type: "nvarchar(40)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_LectureId",
                table: "Reports",
                column: "LectureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Lectures_LectureId",
                table: "Reports",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Lectures_LectureId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_LectureId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "Reports");
        }
    }
}
