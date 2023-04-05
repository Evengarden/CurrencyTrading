using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyTrading.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyTradeSeedV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AKovNpXBsaJdA9+zukJXIgIo0xWtFeOs12LKVVTDe7j5PIkRNlM2iM1gimaZ4zu3IQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "AEqb5bgI3mOi3B+YsGNRgb8sK4chsA22dUI9Yy7YBFSbH93KfGjfbeLdNoaVIg7Tyg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "ALxC+CAXv+I9Ag1gmgSU8PbkvRDVC+E11LqXhVXmgAmnxaGhOKetnZ2OfzSSRmzDQg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "APynB/yVqGPGClIk1XsiRAlbFiyeVFyinBS4KonF9ak18TyHkl7I3p1dgb+9QDTcgg==");
        }
    }
}
