using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyTrading.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyTrade_AddTypeOfLot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Lots",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Balances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Amount", "Currency" },
                values: new object[] { 100000m, "RUB" });

            migrationBuilder.UpdateData(
                table: "Balances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Amount", "Currency" },
                values: new object[] { 100000m, "RUB" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "ACoGLBnMkrVPHgpthX3n3C/QzAo36Uso9zyM9oHMd4rymXIQucAHBvpCuFnsLpxjVQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "ANDImAQOzHe3utRfWnnuXAu1z0zrCfoVRcvxao+BRvaoTHSeOPA12HZ72KRgDMcwIg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Lots");

            migrationBuilder.UpdateData(
                table: "Balances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Amount", "Currency" },
                values: new object[] { 10m, "USD" });

            migrationBuilder.UpdateData(
                table: "Balances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Amount", "Currency" },
                values: new object[] { 20m, "USD" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AOPGWnOfZL+TS9CFLpuvipe/YIHo/ueemUhVAxFrAS8ZSXdE6N8WHP2pvGGm42CRPA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "AKqRutMy/DJtbFs+zHNV9Kpir+OhyAG/k85VFuMB6/8kS+sTIZwfvuS/Uv1o+KBwIw==");
        }
    }
}
