using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shopping_Carts_Users_UserId",
                table: "Shopping_Carts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Shopping_Carts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shopping_Carts_Users_UserId",
                table: "Shopping_Carts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shopping_Carts_Users_UserId",
                table: "Shopping_Carts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Shopping_Carts",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Shopping_Carts_Users_UserId",
                table: "Shopping_Carts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
