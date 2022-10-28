using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Titan.Data.Migrations
{
    public partial class ShippingCart_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCartItemModel_ShoppingCart_ShoppingCartID",
                schema: "Warehouse",
                table: "ProductCartItemModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCartItemModel",
                schema: "Warehouse",
                table: "ProductCartItemModel");

            migrationBuilder.RenameTable(
                name: "ProductCartItemModel",
                schema: "Warehouse",
                newName: "CartItem",
                newSchema: "Warehouse");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCartItemModel_ShoppingCartID",
                schema: "Warehouse",
                table: "CartItem",
                newName: "IX_CartItem_ShoppingCartID");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                schema: "Warehouse",
                table: "ShoppingCart",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                schema: "Warehouse",
                table: "CartItem",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_ShoppingCart_ShoppingCartID",
                schema: "Warehouse",
                table: "CartItem",
                column: "ShoppingCartID",
                principalSchema: "Warehouse",
                principalTable: "ShoppingCart",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_ShoppingCart_ShoppingCartID",
                schema: "Warehouse",
                table: "CartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                schema: "Warehouse",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                schema: "Warehouse",
                table: "ShoppingCart");

            migrationBuilder.RenameTable(
                name: "CartItem",
                schema: "Warehouse",
                newName: "ProductCartItemModel",
                newSchema: "Warehouse");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_ShoppingCartID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                newName: "IX_ProductCartItemModel_ShoppingCartID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCartItemModel",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCartItemModel_ShoppingCart_ShoppingCartID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                column: "ShoppingCartID",
                principalSchema: "Warehouse",
                principalTable: "ShoppingCart",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
