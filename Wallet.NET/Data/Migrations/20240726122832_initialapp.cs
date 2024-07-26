using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.NET.Migrations
{
    /// <inheritdoc />
    public partial class initialapp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ticker = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    Exchange = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d61420cf-0b6e-409e-bf51-be51a7285626", null, "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7c1656d0-c5fa-499a-9c41-bdc2bdefc198", 0, "846c6589-92ed-4460-8150-5b87d00bc3cd", "User", "vitornascimento321@hotmail.com", true, false, null, "Vitor Marciano", "VITORNASCIMENTO321@HOTMAIL.COM", "VITORNASCIMENTO321@HOTMAIL.COM", "AQAAAAIAAYagAAAAED+70EDEerAj/pqbc12vOhGuMnHGnAvTJUySmA+XwfojKy0eq0VPoRQ89QtgOzBCBw==", null, false, "157b104d-957f-4cc9-868f-c021479be87d", false, "vitornascimento321@hotmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d61420cf-0b6e-409e-bf51-be51a7285626", "7c1656d0-c5fa-499a-9c41-bdc2bdefc198" });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_UserId",
                table: "Stocks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d61420cf-0b6e-409e-bf51-be51a7285626", "7c1656d0-c5fa-499a-9c41-bdc2bdefc198" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d61420cf-0b6e-409e-bf51-be51a7285626");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7c1656d0-c5fa-499a-9c41-bdc2bdefc198");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");
        }
    }
}
