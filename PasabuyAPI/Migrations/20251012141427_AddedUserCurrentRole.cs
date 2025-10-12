using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserCurrentRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentRole",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "TotalOrders",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRole",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalOrders",
                table: "Users");
        }
    }
}
