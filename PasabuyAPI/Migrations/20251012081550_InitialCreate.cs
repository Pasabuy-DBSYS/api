using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserIdPK = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    YearLevel = table.Column<int>(type: "integer", nullable: false),
                    RatingAverage = table.Column<decimal>(type: "numeric(2,1)", nullable: false),
                    TotalDeliveries = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserIdPK);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderIdPK = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CourierId = table.Column<long>(type: "bigint", nullable: true),
                    Request = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderIdPK);
                    table.ForeignKey(
                        name: "FK_Orders_Users_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Users",
                        principalColumn: "UserIdPK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "UserIdPK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    RoomIdPK = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderIdFK = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.RoomIdPK);
                    table.ForeignKey(
                        name: "FK_ChatRooms_Orders_OrderIdFK",
                        column: x => x.OrderIdFK,
                        principalTable: "Orders",
                        principalColumn: "OrderIdPK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryDetails",
                columns: table => new
                {
                    DeliveryIdPk = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderIdFK = table.Column<long>(type: "bigint", nullable: false),
                    LocationLatitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    LocationLongitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    CourierLatitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    CourierLongitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    CustomerLatitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    CustomerLongitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    ActualDistance = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualPickupTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualDeliveryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveryNotes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDetails", x => x.DeliveryIdPk);
                    table.ForeignKey(
                        name: "FK_DeliveryDetails_Orders_OrderIdFK",
                        column: x => x.OrderIdFK,
                        principalTable: "Orders",
                        principalColumn: "OrderIdPK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentIdPK = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderIdFK = table.Column<long>(type: "bigint", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaseFee = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    UrgencyFee = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    DeliveryFee = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    TipAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    ItemsFee = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    ProposedItemsFee = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    IsItemsFeeConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    PaymentStatus = table.Column<int>(type: "integer", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentIdPK);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderIdFK",
                        column: x => x.OrderIdFK,
                        principalTable: "Orders",
                        principalColumn: "OrderIdPK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    MessageIdPK = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomIdFK = table.Column<long>(type: "bigint", nullable: false),
                    SenderIdFK = table.Column<long>(type: "bigint", nullable: false),
                    ReceiverIdFK = table.Column<long>(type: "bigint", nullable: false),
                    MessageText = table.Column<string>(type: "text", nullable: false),
                    MessageType = table.Column<int>(type: "integer", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.MessageIdPK);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatRooms_RoomIdFK",
                        column: x => x.RoomIdFK,
                        principalTable: "ChatRooms",
                        principalColumn: "RoomIdPK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Users_ReceiverIdFK",
                        column: x => x.ReceiverIdFK,
                        principalTable: "Users",
                        principalColumn: "UserIdPK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Users_SenderIdFK",
                        column: x => x.SenderIdFK,
                        principalTable: "Users",
                        principalColumn: "UserIdPK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ReceiverIdFK",
                table: "ChatMessages",
                column: "ReceiverIdFK");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_RoomIdFK",
                table: "ChatMessages",
                column: "RoomIdFK");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderIdFK",
                table: "ChatMessages",
                column: "SenderIdFK");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_OrderIdFK",
                table: "ChatRooms",
                column: "OrderIdFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetails_OrderIdFK",
                table: "DeliveryDetails",
                column: "OrderIdFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CourierId",
                table: "Orders",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderIdFK",
                table: "Payments",
                column: "OrderIdFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Phone",
                table: "Users",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "DeliveryDetails");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
