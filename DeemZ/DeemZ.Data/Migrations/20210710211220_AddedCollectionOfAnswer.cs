using Microsoft.EntityFrameworkCore.Migrations;

namespace DeemZ.Data.Migrations
{
    public partial class AddedCollectionOfAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserSurveyApplicationUserId",
                table: "SurveyAnswers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserSurveySurveyId",
                table: "SurveyAnswers",
                type: "nvarchar(40)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswers_ApplicationUserSurveyApplicationUserId_ApplicationUserSurveySurveyId",
                table: "SurveyAnswers",
                columns: new[] { "ApplicationUserSurveyApplicationUserId", "ApplicationUserSurveySurveyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_ApplicationUserSurvey_ApplicationUserSurveyApplicationUserId_ApplicationUserSurveySurveyId",
                table: "SurveyAnswers",
                columns: new[] { "ApplicationUserSurveyApplicationUserId", "ApplicationUserSurveySurveyId" },
                principalTable: "ApplicationUserSurvey",
                principalColumns: new[] { "ApplicationUserId", "SurveyId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_ApplicationUserSurvey_ApplicationUserSurveyApplicationUserId_ApplicationUserSurveySurveyId",
                table: "SurveyAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAnswers_ApplicationUserSurveyApplicationUserId_ApplicationUserSurveySurveyId",
                table: "SurveyAnswers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserSurveyApplicationUserId",
                table: "SurveyAnswers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserSurveySurveyId",
                table: "SurveyAnswers");
        }
    }
}
