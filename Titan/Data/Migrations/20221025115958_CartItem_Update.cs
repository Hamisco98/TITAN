using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Titan.Data.Migrations
{
    public partial class CartItem_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCartItemModel_Prodycts_ProductsID",
                schema: "Warehouse",
                table: "ProductCartItemModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCartItemModel_ShoppingCart_ShoppingCartID",
                schema: "Warehouse",
                table: "ProductCartItemModel");

            migrationBuilder.DropIndex(
                name: "IX_ProductCartItemModel_ProductsID",
                schema: "Warehouse",
                table: "ProductCartItemModel");

            migrationBuilder.RenameColumn(
                name: "ProductsID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                newName: "ProductID");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShoppingCartID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductBarcode",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCartItemModel_ShoppingCart_ShoppingCartID",
                schema: "Warehouse",
                table: "ProductCartItemModel");

            migrationBuilder.DropColumn(
                name: "ProductBarcode",
                schema: "Warehouse",
                table: "ProductCartItemModel");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                newName: "ProductsID");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShoppingCartID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCartItemModel_ProductsID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                column: "ProductsID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCartItemModel_Prodycts_ProductsID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                column: "ProductsID",
                principalSchema: "Warehouse",
                principalTable: "Prodycts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCartItemModel_ShoppingCart_ShoppingCartID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                column: "ShoppingCartID",
                principalSchema: "Warehouse",
                principalTable: "ShoppingCart",
                principalColumn: "ID");
        }
    }
}
