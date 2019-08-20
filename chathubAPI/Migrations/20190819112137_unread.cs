using Microsoft.EntityFrameworkCore.Migrations;

namespace chathubAPI.Migrations
{
    public partial class unread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Messages",
                newName: "timeStamp");

            migrationBuilder.AddColumn<bool>(
                name: "unread",
                table: "Messages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unread",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "timeStamp",
                table: "Messages",
                newName: "TimeStamp");
        }
    }
}
