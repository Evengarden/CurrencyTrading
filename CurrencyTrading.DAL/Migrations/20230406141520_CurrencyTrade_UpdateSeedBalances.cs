using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyTrading.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyTrade_UpdateSeedBalances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Balances",
                keyColumn: "Id",
                keyValue: 1,
                column: "Amount",
                value: 10000m);

            migrationBuilder.UpdateData(
                table: "Balances",
                keyColumn: "Id",
                keyValue: 2,
                column: "Amount",
                value: 10000m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "ABvlouHZkmoL9qWroce0rUnvhnY+84FbuYzhxavygoassww0w2Wi3eYlb9jAxIibfg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "AKzoOotgaVAYYD3uu9RfEBiRX7m5sWMX6z4k5UBNQgosLrHiLFogAxuOpJUcbfli9w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Balances",
                keyColumn: "Id",
                keyValue: 1,
                column: "Amount",
                value: 100000m);

            migrationBuilder.UpdateData(
                table: "Balances",
                keyColumn: "Id",
                keyValue: 2,
                column: "Amount",
                value: 100000m);

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
    }
}
