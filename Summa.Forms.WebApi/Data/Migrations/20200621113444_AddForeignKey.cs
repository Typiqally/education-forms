using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApi.Data.Migrations
{
    public partial class AddForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionCategory_CategoryId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionCategory_Form_FormId",
                table: "QuestionCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "FormId",
                table: "QuestionCategory",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Question",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuestionCategory_CategoryId",
                table: "Question",
                column: "CategoryId",
                principalTable: "QuestionCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionCategory_Form_FormId",
                table: "QuestionCategory",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionCategory_CategoryId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionCategory_Form_FormId",
                table: "QuestionCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "FormId",
                table: "QuestionCategory",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Question",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuestionCategory_CategoryId",
                table: "Question",
                column: "CategoryId",
                principalTable: "QuestionCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionCategory_Form_FormId",
                table: "QuestionCategory",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
