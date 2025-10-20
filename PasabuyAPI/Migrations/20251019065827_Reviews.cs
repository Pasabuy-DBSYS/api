using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PasabuyAPI.Migrations
{
    /// <inheritdoc />
    public partial class Reviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewIDPK = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderIDFK = table.Column<long>(type: "bigint", nullable: false),
                    ReviewerIDFK = table.Column<long>(type: "bigint", nullable: false),
                    ReviewedUserID = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<byte>(type: "smallint", nullable: false),
                    Comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewIDPK);
                    table.ForeignKey(
                        name: "FK_Reviews_Orders_OrderIDFK",
                        column: x => x.OrderIDFK,
                        principalTable: "Orders",
                        principalColumn: "OrderIdPK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_ReviewedUserID",
                        column: x => x.ReviewedUserID,
                        principalTable: "Users",
                        principalColumn: "UserIdPK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_ReviewerIDFK",
                        column: x => x.ReviewerIDFK,
                        principalTable: "Users",
                        principalColumn: "UserIdPK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderIDFK",
                table: "Reviews",
                column: "OrderIDFK");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewedUserID",
                table: "Reviews",
                column: "ReviewedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerIDFK",
                table: "Reviews",
                column: "ReviewerIDFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
