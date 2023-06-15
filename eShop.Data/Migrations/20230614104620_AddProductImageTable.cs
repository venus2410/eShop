using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShop.Data.Migrations
{
    public partial class AddProductImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 6, 14, 8, 37, 49, 243, DateTimeKind.Local).AddTicks(4009));

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    ImagePath = table.Column<string>(maxLength: 200, nullable: false),
                    Caption = table.Column<string>(maxLength: 200, nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    FileSize = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("b993b2be-4591-4ec9-b319-5fab47ded296"),
                column: "ConcurrencyStamp",
                value: "1a1f1bd8-f803-4705-b8a8-b85eec9428ef");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffba58d7-2f16-4902-a35d-d5a0cf3b417c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "819730e7-2ff3-4210-a545-b8cd0b265b0d", "AQAAAAEAACcQAAAAEKBwal78gikZ5IP8sOXOsNN6+6IQEkIqg8i/evl7fZNKsvJW2MQMPm6O1lL+ayJCPQ==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 6, 14, 17, 46, 19, 605, DateTimeKind.Local).AddTicks(5258));

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 14, 8, 37, 49, 243, DateTimeKind.Local).AddTicks(4009),
                oldClrType: typeof(DateTime));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("b993b2be-4591-4ec9-b319-5fab47ded296"),
                column: "ConcurrencyStamp",
                value: "980cadc1-61a3-4398-8e62-5f362e65272f");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffba58d7-2f16-4902-a35d-d5a0cf3b417c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e668cd12-72f4-4965-b15b-bc44a3281e77", "AQAAAAEAACcQAAAAELujt7jgTr2KqMUW5eR7tN6oP8BDbZmBLqrnGV0t9ixgux59jU2dG2PBhF7RbUw73g==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 6, 14, 8, 37, 49, 259, DateTimeKind.Local).AddTicks(6294));
        }
    }
}
