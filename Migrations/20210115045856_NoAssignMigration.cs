using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectTracker.Migrations
{
    public partial class NoAssignMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assigns");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assigns",
                columns: table => new
                {
                    AssignId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdminAssigning = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    AssignedTicketTicketId = table.Column<int>(type: "int", nullable: true),
                    AssignedUserUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TicketIdAssigned = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserIdAssigned = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assigns", x => x.AssignId);
                    table.ForeignKey(
                        name: "FK_Assigns_Tickets_AssignedTicketTicketId",
                        column: x => x.AssignedTicketTicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assigns_Users_AssignedUserUserId",
                        column: x => x.AssignedUserUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assigns_AssignedTicketTicketId",
                table: "Assigns",
                column: "AssignedTicketTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Assigns_AssignedUserUserId",
                table: "Assigns",
                column: "AssignedUserUserId");
        }
    }
}
