using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApi.Data.Migrations
{
    public partial class FixDeleteBehaviorAddEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionCategory_CategoryId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_CategoryId",
                table: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Form",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_CategoryId",
                table: "Question",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuestionCategory_CategoryId",
                table: "Question",
                column: "CategoryId",
                principalTable: "QuestionCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionCategory_CategoryId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_CategoryId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Form");

            migrationBuilder.CreateIndex(
                name: "IX_Question_CategoryId",
                table: "Question",
                column: "CategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuestionCategory_CategoryId",
                table: "Question",
                column: "CategoryId",
                principalTable: "QuestionCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
