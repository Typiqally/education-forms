using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApi.Data.Migrations
{
    public partial class AddQuestionCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Question",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuestionCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FormId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionCategory_Form_FormId",
                        column: x => x.FormId,
                        principalTable: "Form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_CategoryId",
                table: "Question",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCategory_FormId",
                table: "QuestionCategory",
                column: "FormId");

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

            migrationBuilder.DropTable(
                name: "QuestionCategory");

            migrationBuilder.DropIndex(
                name: "IX_Question_CategoryId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Question");
        }
    }
}
