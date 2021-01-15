using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectTracker.Migrations
{
    public partial class ThirdAssignMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Assigns");

            migrationBuilder.AddColumn<string>(
                name: "Admin",
                table: "Assigns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "Assigns");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Assigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
