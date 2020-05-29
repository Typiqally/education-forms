using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApp.Data.Migrations
{
    public partial class AddIndexProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "QuestionOption",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Question",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "QuestionOption");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Question");
        }
    }
}
