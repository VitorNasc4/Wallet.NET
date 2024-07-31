using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.NET.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStockRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_AspNetUsers_UserId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_UserId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserStocks",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStocks", x => new { x.UserId, x.StockId });
                    table.ForeignKey(
                        name: "FK_UserStocks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStocks_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7c1656d0-c5fa-499a-9c41-bdc2bdefc198",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a13bf4e2-97fc-46b1-b3da-3253202d4b76", "AQAAAAIAAYagAAAAEJzDAOmCn7zVEXW9dY81De3G1FqkATemw3EmjarY1wmHhGhBFBTrovA61jAiNSFltg==", "578693a2-97cb-486a-9ff3-5e4ce7d2789b" });

            migrationBuilder.CreateIndex(
                name: "IX_UserStocks_StockId",
                table: "UserStocks",
                column: "StockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStocks");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Stocks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7c1656d0-c5fa-499a-9c41-bdc2bdefc198",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp" },
                values: new object[] { "846c6589-92ed-4460-8150-5b87d00bc3cd", "Vitor Marciano", "AQAAAAIAAYagAAAAED+70EDEerAj/pqbc12vOhGuMnHGnAvTJUySmA+XwfojKy0eq0VPoRQ89QtgOzBCBw==", "157b104d-957f-4cc9-868f-c021479be87d" });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_UserId",
                table: "Stocks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_AspNetUsers_UserId",
                table: "Stocks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
