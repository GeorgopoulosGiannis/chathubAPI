using Microsoft.EntityFrameworkCore.Migrations;

namespace chathubAPI.Migrations
{
    public partial class Comments2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageComments_AspNetUsers_CommentById",
                table: "ImageComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageComments",
                table: "ImageComments");

            migrationBuilder.DropIndex(
                name: "IX_ImageComments_CommentById",
                table: "ImageComments");

            migrationBuilder.DropColumn(
                name: "CommentById",
                table: "ImageComments");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "ImageComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageComments",
                table: "ImageComments",
                columns: new[] { "CommentId", "ImageId" });

            migrationBuilder.CreateIndex(
                name: "IX_ImageComments_ImageId",
                table: "ImageComments",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageComments_Images_ImageId",
                table: "ImageComments",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageComments_Images_ImageId",
                table: "ImageComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageComments",
                table: "ImageComments");

            migrationBuilder.DropIndex(
                name: "IX_ImageComments_ImageId",
                table: "ImageComments");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ImageComments");

            migrationBuilder.AddColumn<string>(
                name: "CommentById",
                table: "ImageComments",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageComments",
                table: "ImageComments",
                columns: new[] { "CommentId", "CommentById" });

            migrationBuilder.CreateIndex(
                name: "IX_ImageComments_CommentById",
                table: "ImageComments",
                column: "CommentById");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageComments_AspNetUsers_CommentById",
                table: "ImageComments",
                column: "CommentById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
