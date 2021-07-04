using Microsoft.EntityFrameworkCore.Migrations;

namespace DeemZ.Data.Migrations
{
    public partial class MadeMappingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_ApplicationUserId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ApplicationUserId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Exams");

            migrationBuilder.CreateTable(
                name: "ApplicationUserExam",
                columns: table => new
                {
                    ExamsId = table.Column<string>(type: "nvarchar(40)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserExam", x => new { x.ExamsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserExam_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserExam_Exams_ExamsId",
                        column: x => x.ExamsId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserExam_UsersId",
                table: "ApplicationUserExam",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserExam");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Exams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ApplicationUserId",
                table: "Exams",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_ApplicationUserId",
                table: "Exams",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
