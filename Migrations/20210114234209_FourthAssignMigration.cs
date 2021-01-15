using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectTracker.Migrations
{
    public partial class FourthAssignMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assigns_Tickets_TicketId",
                table: "Assigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Assigns_Users_UserId",
                table: "Assigns");

            migrationBuilder.DropIndex(
                name: "IX_Assigns_TicketId",
                table: "Assigns");

            migrationBuilder.DropIndex(
                name: "IX_Assigns_UserId",
                table: "Assigns");

            migrationBuilder.DropColumn(
                name: "Admin",
                table: "Assigns");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Assigns");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Assigns");

            migrationBuilder.AddColumn<string>(
                name: "AdminAssigning",
                table: "Assigns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedTicketTicketId",
                table: "Assigns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedUserUserId",
                table: "Assigns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketIdAssigned",
                table: "Assigns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserIdAssigned",
                table: "Assigns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assigns_AssignedTicketTicketId",
                table: "Assigns",
                column: "AssignedTicketTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Assigns_AssignedUserUserId",
                table: "Assigns",
                column: "AssignedUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assigns_Tickets_AssignedTicketTicketId",
                table: "Assigns",
                column: "AssignedTicketTicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assigns_Users_AssignedUserUserId",
                table: "Assigns",
                column: "AssignedUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assigns_Tickets_AssignedTicketTicketId",
                table: "Assigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Assigns_Users_AssignedUserUserId",
                table: "Assigns");

            migrationBuilder.DropIndex(
                name: "IX_Assigns_AssignedTicketTicketId",
                table: "Assigns");

            migrationBuilder.DropIndex(
                name: "IX_Assigns_AssignedUserUserId",
                table: "Assigns");

            migrationBuilder.DropColumn(
                name: "AdminAssigning",
                table: "Assigns");

            migrationBuilder.DropColumn(
                name: "AssignedTicketTicketId",
                table: "Assigns");

            migrationBuilder.DropColumn(
                name: "AssignedUserUserId",
                table: "Assigns");

            migrationBuilder.DropColumn(
                name: "TicketIdAssigned",
                table: "Assigns");

            migrationBuilder.DropColumn(
                name: "UserIdAssigned",
                table: "Assigns");

            migrationBuilder.AddColumn<string>(
                name: "Admin",
                table: "Assigns",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Assigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Assigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assigns_TicketId",
                table: "Assigns",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Assigns_UserId",
                table: "Assigns",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assigns_Tickets_TicketId",
                table: "Assigns",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assigns_Users_UserId",
                table: "Assigns",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
