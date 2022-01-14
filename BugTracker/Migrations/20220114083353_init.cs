using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "newdescription",
                table: "history");

            migrationBuilder.DropColumn(
                name: "newtitle",
                table: "history");

            migrationBuilder.DropColumn(
                name: "oldescription",
                table: "history");

            migrationBuilder.DropColumn(
                name: "oldtitle",
                table: "history");

            migrationBuilder.AlterColumn<string>(
                name: "oldvalue",
                table: "history",
                unicode: false,
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "newvalue",
                table: "history",
                unicode: false,
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "attribute",
                table: "history",
                unicode: false,
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "oldvalue",
                table: "history",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "newvalue",
                table: "history",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "attribute",
                table: "history",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "newdescription",
                table: "history",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "newtitle",
                table: "history",
                type: "varchar(80)",
                unicode: false,
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "oldescription",
                table: "history",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "oldtitle",
                table: "history",
                type: "varchar(80)",
                unicode: false,
                maxLength: 80,
                nullable: true);
        }
    }
}
