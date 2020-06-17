using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApi.Data.Migrations
{
    public partial class AddFormResponseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Question_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_FormCategory_CategoryId",
                table: "Forms");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Forms_FormId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Repository_Forms_FormId",
                table: "Repository");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Forms",
                table: "Forms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answers",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Answers");

            migrationBuilder.RenameTable(
                name: "Forms",
                newName: "Form");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "QuestionAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_Forms_CategoryId",
                table: "Form",
                newName: "IX_Form_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_QuestionId",
                table: "QuestionAnswer",
                newName: "IX_QuestionAnswer_QuestionId");

            migrationBuilder.AddColumn<Guid>(
                name: "FormResponseId",
                table: "QuestionAnswer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Form",
                table: "Form",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionAnswer",
                table: "QuestionAnswer",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FormResponse",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FormId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormResponse_Form_FormId",
                        column: x => x.FormId,
                        principalTable: "Form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_FormResponseId",
                table: "QuestionAnswer",
                column: "FormResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_FormResponse_FormId",
                table: "FormResponse",
                column: "FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Form_FormCategory_CategoryId",
                table: "Form",
                column: "CategoryId",
                principalTable: "FormCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Form_FormId",
                table: "Question",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_FormResponse_FormResponseId",
                table: "QuestionAnswer",
                column: "FormResponseId",
                principalTable: "FormResponse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_Question_QuestionId",
                table: "QuestionAnswer",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repository_Form_FormId",
                table: "Repository",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Form_FormCategory_CategoryId",
                table: "Form");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Form_FormId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_FormResponse_FormResponseId",
                table: "QuestionAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_Question_QuestionId",
                table: "QuestionAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_Repository_Form_FormId",
                table: "Repository");

            migrationBuilder.DropTable(
                name: "FormResponse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionAnswer",
                table: "QuestionAnswer");

            migrationBuilder.DropIndex(
                name: "IX_QuestionAnswer_FormResponseId",
                table: "QuestionAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Form",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "FormResponseId",
                table: "QuestionAnswer");

            migrationBuilder.RenameTable(
                name: "QuestionAnswer",
                newName: "Answers");

            migrationBuilder.RenameTable(
                name: "Form",
                newName: "Forms");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAnswer_QuestionId",
                table: "Answers",
                newName: "IX_Answers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Form_CategoryId",
                table: "Forms",
                newName: "IX_Forms_CategoryId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answers",
                table: "Answers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forms",
                table: "Forms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Question_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_FormCategory_CategoryId",
                table: "Forms",
                column: "CategoryId",
                principalTable: "FormCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Forms_FormId",
                table: "Question",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repository_Forms_FormId",
                table: "Repository",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
