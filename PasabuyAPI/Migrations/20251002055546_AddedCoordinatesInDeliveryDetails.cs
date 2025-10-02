using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedCoordinatesInDeliveryDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualDeliveryTime",
                table: "DeliveryDetails",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<decimal>(
                name: "CourierLatitude",
                table: "DeliveryDetails",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CourierLongitude",
                table: "DeliveryDetails",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CustomerLatitude",
                table: "DeliveryDetails",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CustomerLongitude",
                table: "DeliveryDetails",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourierLatitude",
                table: "DeliveryDetails");

            migrationBuilder.DropColumn(
                name: "CourierLongitude",
                table: "DeliveryDetails");

            migrationBuilder.DropColumn(
                name: "CustomerLatitude",
                table: "DeliveryDetails");

            migrationBuilder.DropColumn(
                name: "CustomerLongitude",
                table: "DeliveryDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualDeliveryTime",
                table: "DeliveryDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
