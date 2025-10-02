using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class RefinedOrdersModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CourierId",
                table: "Orders",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "DeliveryDetails",
                columns: table => new
                {
                    DeliveryIdPk = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderIdPK = table.Column<long>(type: "bigint", nullable: true),
                    EstimatedDistance = table.Column<decimal>(type: "numeric", nullable: false),
                    ActualDistance = table.Column<decimal>(type: "numeric", nullable: false),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualDeliveryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "numeric", nullable: false),
                    DeliveryNotes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDetails", x => x.DeliveryIdPk);
                    table.ForeignKey(
                        name: "FK_DeliveryDetails_Orders_OrderIdPK",
                        column: x => x.OrderIdPK,
                        principalTable: "Orders",
                        principalColumn: "OrderIdPK");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetails_OrderIdPK",
                table: "DeliveryDetails",
                column: "OrderIdPK",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryDetails");

            migrationBuilder.AlterColumn<long>(
                name: "CourierId",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
