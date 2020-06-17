using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApi.Data.Migrations
{
    public partial class RenameQuestionValuePropertyToKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Question",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Question",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
