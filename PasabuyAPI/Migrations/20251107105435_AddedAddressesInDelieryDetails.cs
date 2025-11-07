using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedAddressesInDelieryDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "DeliveryDetails",
                newName: "DestinationAddress");

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddress",
                table: "DeliveryDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerAddress",
                table: "DeliveryDetails");

            migrationBuilder.RenameColumn(
                name: "DestinationAddress",
                table: "DeliveryDetails",
                newName: "Address");
        }
    }
}
