using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyTrading.Migrations
{
    /// <inheritdoc />
    public partial class AutomatchingFieldForLot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TradeDate",
                table: "Trades",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "Automatch",
                table: "Lots",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "ALTxFXCYscS7WroaHNKwIt8tyk+hEDnCTFhEFQOW8+KeLoQMsmntAl533XATDazh2g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "ACw7NtPp0etOyUG3izqMPd2Y9mnXP0K/y1Q/XVP0vRBuqSIzk5ebLmbW31c3Crj44g==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Automatch",
                table: "Lots");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TradeDate",
                table: "Trades",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

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
    }
}
