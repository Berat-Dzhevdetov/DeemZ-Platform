using Microsoft.EntityFrameworkCore.Migrations;

namespace DeemZ.Data.Migrations
{
    public partial class MadeMappingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Survey_AspNetUsers_ApplicationUserId",
                table: "Survey");

            migrationBuilder.DropIndex(
                name: "IX_Survey_ApplicationUserId",
                table: "Survey");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Survey");

            migrationBuilder.CreateTable(
                name: "ApplicationUserSurvey",
                columns: table => new
                {
                    SurveysId = table.Column<string>(type: "nvarchar(40)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserSurvey", x => new { x.SurveysId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserSurvey_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserSurvey_Survey_SurveysId",
                        column: x => x.SurveysId,
                        principalTable: "Survey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserSurvey_UsersId",
                table: "ApplicationUserSurvey",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserSurvey");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Survey",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Survey_ApplicationUserId",
                table: "Survey",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Survey_AspNetUsers_ApplicationUserId",
                table: "Survey",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
