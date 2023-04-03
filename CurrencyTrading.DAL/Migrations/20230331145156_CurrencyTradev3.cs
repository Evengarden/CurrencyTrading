using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyTrading.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyTradev3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Lots_Lot_Id",
                table: "Trades");

            migrationBuilder.RenameColumn(
                name: "Lot_Id",
                table: "Trades",
                newName: "LotId");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_Lot_Id",
                table: "Trades",
                newName: "IX_Trades_LotId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Lots_LotId",
                table: "Trades",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Lots_LotId",
                table: "Trades");

            migrationBuilder.RenameColumn(
                name: "LotId",
                table: "Trades",
                newName: "Lot_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_LotId",
                table: "Trades",
                newName: "IX_Trades_Lot_Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "ABZ67xgApXqpIXf5Xu/oH827IhhYHUbCssetlS7DwGz+uC7fgbVh2oVIB21J7zRHCw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "AEy/K6Z7gFbkMllxgHU0UhjNQcPlWBN1YyLmRst0De6NSkoED1E4clPvny0taQHElQ==");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Lots_Lot_Id",
                table: "Trades",
                column: "Lot_Id",
                principalTable: "Lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
