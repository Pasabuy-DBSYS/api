using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexingInOrderStatusAndUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_CourierId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CourierId_Status_Updated_at",
                table: "Orders",
                columns: new[] { "CourierId", "Status", "Updated_at" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId_Status_Updated_at",
                table: "Orders",
                columns: new[] { "CustomerId", "Status", "Updated_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_CourierId_Status_Updated_at",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId_Status_Updated_at",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CourierId",
                table: "Orders",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }
    }
}
