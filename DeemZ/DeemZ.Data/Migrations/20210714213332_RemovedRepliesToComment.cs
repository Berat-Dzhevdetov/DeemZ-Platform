using Microsoft.EntityFrameworkCore.Migrations;

namespace DeemZ.Data.Migrations
{
    public partial class RemovedRepliesToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_AnswerToCommentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AnswerToCommentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AnswerToCommentId",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnswerToCommentId",
                table: "Comments",
                type: "nvarchar(40)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AnswerToCommentId",
                table: "Comments",
                column: "AnswerToCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_AnswerToCommentId",
                table: "Comments",
                column: "AnswerToCommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
