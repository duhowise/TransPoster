using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransPorter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistory_AspNetUsers_UserId",
                table: "UserHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory");

            migrationBuilder.RenameTable(
                name: "UserHistory",
                newName: "UserHistories");

            migrationBuilder.RenameIndex(
                name: "IX_UserHistory_UserId",
                table: "UserHistories",
                newName: "IX_UserHistories_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserHistories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHistories",
                table: "UserHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistories_AspNetUsers_UserId",
                table: "UserHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistories_AspNetUsers_UserId",
                table: "UserHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHistories",
                table: "UserHistories");

            migrationBuilder.RenameTable(
                name: "UserHistories",
                newName: "UserHistory");

            migrationBuilder.RenameIndex(
                name: "IX_UserHistories_UserId",
                table: "UserHistory",
                newName: "IX_UserHistory_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserHistory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistory_AspNetUsers_UserId",
                table: "UserHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
