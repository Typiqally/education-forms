using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Summa.Forms.WebApp.Data.Migrations
{
    public partial class ChangeRepositoryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Repository",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Repository");
        }
    }
}
