using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiOrderService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameOrderShipping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingState",
                table: "Orders",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "ShippingPostCode",
                table: "Orders",
                newName: "PostCode");

            migrationBuilder.RenameColumn(
                name: "ShippingCountry",
                table: "Orders",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "ShippingCity",
                table: "Orders",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressLine2",
                table: "Orders",
                newName: "AddressLine2");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressLine1",
                table: "Orders",
                newName: "AddressLine1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Orders",
                newName: "ShippingState");

            migrationBuilder.RenameColumn(
                name: "PostCode",
                table: "Orders",
                newName: "ShippingPostCode");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Orders",
                newName: "ShippingCountry");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Orders",
                newName: "ShippingCity");

            migrationBuilder.RenameColumn(
                name: "AddressLine2",
                table: "Orders",
                newName: "ShippingAddressLine2");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                table: "Orders",
                newName: "ShippingAddressLine1");
        }
    }
}
