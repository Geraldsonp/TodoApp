using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Database.MIgrations
{
    public partial class AddsOwnerTodo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Todos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Todos");
        }
    }
}
