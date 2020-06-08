using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApi.Data.Migrations
{
    public partial class RenameKeyPropertyToTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "QuestionOption");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "QuestionOption",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Question",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "QuestionOption");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "QuestionOption",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Question",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
