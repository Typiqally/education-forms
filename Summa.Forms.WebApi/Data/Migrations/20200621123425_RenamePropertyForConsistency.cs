using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApi.Data.Migrations
{
    public partial class RenamePropertyForConsistency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "QuestionCategory");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "QuestionCategory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "QuestionCategory");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "QuestionCategory",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
