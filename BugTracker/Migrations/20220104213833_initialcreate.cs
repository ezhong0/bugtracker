using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace BugTracker.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    userid = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    roleid = table.Column<int>(nullable: false),
                    username = table.Column<string>(unicode: false, maxLength: 80, nullable: false),
                    password = table.Column<string>(unicode: false, maxLength: 80, nullable: false),
                    name = table.Column<string>(unicode: false, maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    projectid = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(unicode: false, maxLength: 80, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    datemodified = table.Column<DateTime>(type: "date", nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project", x => x.projectid);
                    table.ForeignKey(
                        name: "FK_user_project",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ticket",
                columns: table => new
                {
                    ticketid = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(unicode: false, maxLength: 80, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    priorityid = table.Column<int>(nullable: false),
                    statusid = table.Column<int>(nullable: false),
                    typeid = table.Column<int>(nullable: false),
                    datemodified = table.Column<DateTime>(type: "date", nullable: false),
                    UserCreatedId = table.Column<int>(nullable: false),
                    UserAssignedId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket", x => x.ticketid);
                    table.ForeignKey(
                        name: "FK_ticket_project",
                        column: x => x.ProjectId,
                        principalTable: "project",
                        principalColumn: "projectid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ticket_userAssigned",
                        column: x => x.UserAssignedId,
                        principalTable: "user",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ticket_userCreated",
                        column: x => x.UserCreatedId,
                        principalTable: "user",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    commentid = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    text = table.Column<string>(type: "text", nullable: true),
                    datecreated = table.Column<DateTime>(type: "date", nullable: false),
                    userid = table.Column<int>(nullable: false),
                    TicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.commentid);
                    table.ForeignKey(
                        name: "FK_ticket_comment",
                        column: x => x.TicketId,
                        principalTable: "ticket",
                        principalColumn: "ticketid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "history",
                columns: table => new
                {
                    historyid = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    attribute = table.Column<int>(nullable: false),
                    oldvalue = table.Column<int>(nullable: false),
                    newvalue = table.Column<int>(nullable: false),
                    oldtitle = table.Column<string>(unicode: false, maxLength: 80, nullable: true),
                    newtitle = table.Column<string>(unicode: false, maxLength: 80, nullable: true),
                    oldescription = table.Column<string>(type: "text", nullable: true),
                    newdescription = table.Column<string>(type: "text", nullable: true),
                    DateModified = table.Column<DateTime>(nullable: false),
                    userid = table.Column<int>(nullable: false),
                    TicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_history", x => x.historyid);
                    table.ForeignKey(
                        name: "FK_ticket_history",
                        column: x => x.TicketId,
                        principalTable: "ticket",
                        principalColumn: "ticketid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "media",
                columns: table => new
                {
                    mediaid = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(unicode: false, maxLength: 80, nullable: false),
                    data = table.Column<byte[]>(nullable: true),
                    TicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media", x => x.mediaid);
                    table.ForeignKey(
                        name: "FK_ticket_media",
                        column: x => x.TicketId,
                        principalTable: "ticket",
                        principalColumn: "ticketid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comment_TicketId",
                table: "comment",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_history_TicketId",
                table: "history",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_media_TicketId",
                table: "media",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_project_UserId",
                table: "project",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_ProjectId",
                table: "ticket",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_UserAssignedId",
                table: "ticket",
                column: "UserAssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_UserCreatedId",
                table: "ticket",
                column: "UserCreatedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "history");

            migrationBuilder.DropTable(
                name: "media");

            migrationBuilder.DropTable(
                name: "ticket");

            migrationBuilder.DropTable(
                name: "project");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
