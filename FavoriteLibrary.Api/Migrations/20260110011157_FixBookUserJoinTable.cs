using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FavoriteLibrary.Migrations
{
    /// <inheritdoc />
    public partial class FixBookUserJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookUser_Books_BooksId",
                table: "BookUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BookUser_User_UsersId",
                table: "BookUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookUser",
                table: "BookUser");

            migrationBuilder.RenameTable(
                name: "BookUser",
                newName: "BookUsers");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "BookUsers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "BooksId",
                table: "BookUsers",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookUser_UsersId",
                table: "BookUsers",
                newName: "IX_BookUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookUsers",
                table: "BookUsers",
                columns: new[] { "BookId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookUsers_Books_BookId",
                table: "BookUsers",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookUsers_User_UserId",
                table: "BookUsers",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookUsers_Books_BookId",
                table: "BookUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookUsers_User_UserId",
                table: "BookUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookUsers",
                table: "BookUsers");

            migrationBuilder.RenameTable(
                name: "BookUsers",
                newName: "BookUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BookUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "BookUser",
                newName: "BooksId");

            migrationBuilder.RenameIndex(
                name: "IX_BookUsers_UserId",
                table: "BookUser",
                newName: "IX_BookUser_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookUser",
                table: "BookUser",
                columns: new[] { "BooksId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookUser_Books_BooksId",
                table: "BookUser",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookUser_User_UsersId",
                table: "BookUser",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
