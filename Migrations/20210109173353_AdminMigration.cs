using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectTracker.Migrations
{
    public partial class AdminMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Admin",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "Users");
        }
    }
}
