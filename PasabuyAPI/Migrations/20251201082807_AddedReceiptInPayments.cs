using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedReceiptInPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageKey",
                table: "Payments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageKey",
                table: "Payments");
        }
    }
}
