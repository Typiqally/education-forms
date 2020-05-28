using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApp.Data.Migrations
{
    public partial class AddQuestionValueProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Question",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Question");
        }
    }
}
