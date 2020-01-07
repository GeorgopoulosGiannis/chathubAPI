using Microsoft.EntityFrameworkCore.Migrations;

namespace chathubAPI.Migrations
{
    public partial class Fcmtokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FcmTokens",
                columns: table => new
                {
                    Token = table.Column<string>(nullable: false),
                    TokenOwner = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FcmTokens", x => x.Token);
                    table.ForeignKey(
                        name: "FK_FcmTokens_AspNetUsers_TokenOwner",
                        column: x => x.TokenOwner,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FcmTokens_TokenOwner",
                table: "FcmTokens",
                column: "TokenOwner");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FcmTokens");
        }
    }
}
