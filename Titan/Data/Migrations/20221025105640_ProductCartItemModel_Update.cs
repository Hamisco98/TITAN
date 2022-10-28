using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Titan.Data.Migrations
{
    public partial class ProductCartItemModel_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems",
                schema: "Warehouse");

            migrationBuilder.CreateTable(
                name: "ProductCartItemModel",
                schema: "Warehouse",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SubTotalCost = table.Column<double>(type: "float", nullable: false),
                    SubTotalPrice = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShoppingCartID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCartItemModel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductCartItemModel_Prodycts_ProductsID",
                        column: x => x.ProductsID,
                        principalSchema: "Warehouse",
                        principalTable: "Prodycts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCartItemModel_ShoppingCart_ShoppingCartID",
                        column: x => x.ShoppingCartID,
                        principalSchema: "Warehouse",
                        principalTable: "ShoppingCart",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCartItemModel_ProductsID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                column: "ProductsID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCartItemModel_ShoppingCartID",
                schema: "Warehouse",
                table: "ProductCartItemModel",
                column: "ShoppingCartID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCartItemModel",
                schema: "Warehouse");

            migrationBuilder.CreateTable(
                name: "CartItems",
                schema: "Warehouse",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CartItems_Prodycts_ProductsID",
                        column: x => x.ProductsID,
                        principalSchema: "Warehouse",
                        principalTable: "Prodycts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_ShoppingCart_ShoppingCartID",
                        column: x => x.ShoppingCartID,
                        principalSchema: "Warehouse",
                        principalTable: "ShoppingCart",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductsID",
                schema: "Warehouse",
                table: "CartItems",
                column: "ProductsID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShoppingCartID",
                schema: "Warehouse",
                table: "CartItems",
                column: "ShoppingCartID");
        }
    }
}
