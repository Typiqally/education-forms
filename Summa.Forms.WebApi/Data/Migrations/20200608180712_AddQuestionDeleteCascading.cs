using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApi.Data.Migrations
{
    public partial class AddQuestionDeleteCascading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOption_Question_QuestionId",
                table: "QuestionOption");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "QuestionOption",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOption_Question_QuestionId",
                table: "QuestionOption",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOption_Question_QuestionId",
                table: "QuestionOption");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "QuestionOption",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOption_Question_QuestionId",
                table: "QuestionOption",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
