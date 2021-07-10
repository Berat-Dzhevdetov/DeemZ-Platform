using Microsoft.EntityFrameworkCore.Migrations;

namespace DeemZ.Data.Migrations
{
    public partial class AddedMappingTableBetweenUsersAndSurveys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserSurvey_AspNetUsers_UsersId",
                table: "ApplicationUserSurvey");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserSurvey_Surveys_SurveysId",
                table: "ApplicationUserSurvey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserSurvey",
                table: "ApplicationUserSurvey");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserSurvey_UsersId",
                table: "ApplicationUserSurvey");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ApplicationUserSurvey",
                newName: "ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "SurveysId",
                table: "ApplicationUserSurvey",
                newName: "SurveyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserSurvey",
                table: "ApplicationUserSurvey",
                columns: new[] { "ApplicationUserId", "SurveyId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserSurvey_SurveyId",
                table: "ApplicationUserSurvey",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserSurvey_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserSurvey",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserSurvey_Surveys_SurveyId",
                table: "ApplicationUserSurvey",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserSurvey_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserSurvey");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserSurvey_Surveys_SurveyId",
                table: "ApplicationUserSurvey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserSurvey",
                table: "ApplicationUserSurvey");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserSurvey_SurveyId",
                table: "ApplicationUserSurvey");

            migrationBuilder.RenameColumn(
                name: "SurveyId",
                table: "ApplicationUserSurvey",
                newName: "SurveysId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "ApplicationUserSurvey",
                newName: "UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserSurvey",
                table: "ApplicationUserSurvey",
                columns: new[] { "SurveysId", "UsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserSurvey_UsersId",
                table: "ApplicationUserSurvey",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserSurvey_AspNetUsers_UsersId",
                table: "ApplicationUserSurvey",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserSurvey_Surveys_SurveysId",
                table: "ApplicationUserSurvey",
                column: "SurveysId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
