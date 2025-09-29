using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiOrderService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrderItemDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderItems",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
