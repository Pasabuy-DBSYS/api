using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedBool1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCustomerReviewd",
                table: "Orders",
                newName: "IsCustomerReviewed");

            migrationBuilder.RenameColumn(
                name: "IsCourierReviewd",
                table: "Orders",
                newName: "IsCourierReviewed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCustomerReviewed",
                table: "Orders",
                newName: "IsCustomerReviewd");

            migrationBuilder.RenameColumn(
                name: "IsCourierReviewed",
                table: "Orders",
                newName: "IsCourierReviewd");
        }
    }
}
