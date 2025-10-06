using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedProposedItemsInPaymentsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsItemsFeeConfirmed",
                table: "Payments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ProposedItemsFee",
                table: "Payments",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsItemsFeeConfirmed",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProposedItemsFee",
                table: "Payments");
        }
    }
}
