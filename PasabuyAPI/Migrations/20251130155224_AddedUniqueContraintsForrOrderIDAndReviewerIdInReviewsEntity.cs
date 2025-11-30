using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueContraintsForrOrderIDAndReviewerIdInReviewsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrderIDFK",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderIDFK_ReviewerIDFK",
                table: "Reviews",
                columns: new[] { "OrderIDFK", "ReviewerIDFK" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrderIDFK_ReviewerIDFK",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderIDFK",
                table: "Reviews",
                column: "OrderIDFK");
        }
    }
}
