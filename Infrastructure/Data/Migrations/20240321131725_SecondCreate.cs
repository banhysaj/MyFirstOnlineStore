using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "Cart_Items",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shopping_Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shopping_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shopping_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_Items_ShoppingCartId",
                table: "Cart_Items",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Shopping_Carts_UserId",
                table: "Shopping_Carts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Items_Shopping_Carts_ShoppingCartId",
                table: "Cart_Items",
                column: "ShoppingCartId",
                principalTable: "Shopping_Carts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Items_Shopping_Carts_ShoppingCartId",
                table: "Cart_Items");

            migrationBuilder.DropTable(
                name: "Shopping_Carts");

            migrationBuilder.DropIndex(
                name: "IX_Cart_Items_ShoppingCartId",
                table: "Cart_Items");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "Cart_Items");
        }
    }
}
