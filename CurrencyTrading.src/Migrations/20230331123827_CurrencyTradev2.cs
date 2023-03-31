using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyTrading.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyTradev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AJOmqN6/0tmMy/XP6Fw2yoM1GdEHDXKePhO2Sej+8JIea5DbemXD8S6QlIQpKfPwFw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "ANGmMRfnq/X11FUBexTAeixK/ttTV2wGpbC8o8WvZ8dPxGQ/HXKQJvk7b6HWGhIQXQ==");
        }
    }
}
