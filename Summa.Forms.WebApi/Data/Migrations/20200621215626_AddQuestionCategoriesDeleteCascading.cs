using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApi.Data.Migrations
{
    public partial class AddQuestionCategoriesDeleteCascading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionCategory_Form_FormId",
                table: "QuestionCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "FormId",
                table: "QuestionCategory",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionCategory_Form_FormId",
                table: "QuestionCategory",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionCategory_Form_FormId",
                table: "QuestionCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "FormId",
                table: "QuestionCategory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionCategory_Form_FormId",
                table: "QuestionCategory",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
