using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiOrderService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_InProgress_OrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_Status",
                table: "Orders");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_Status",
                table: "Orders",
                sql: "\"Status\" IN ('Pending', 'InProgress', 'Confirmed', 'Shipped', 'Delivered', 'Cancelled')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_Status",
                table: "Orders");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_Status",
                table: "Orders",
                sql: "\"Status\" IN ('Pending', 'Confirmed', 'Shipped', 'Delivered', 'Cancelled')");
        }
    }
}
