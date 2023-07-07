using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShop.Data.Migrations
{
    public partial class AtIsFeaturedRow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Products",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("b993b2be-4591-4ec9-b319-5fab47ded296"),
                column: "ConcurrencyStamp",
                value: "420a1ca2-ddad-40bd-b534-bad8d56ef5f2");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffba58d7-2f16-4902-a35d-d5a0cf3b417c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9b0ba8be-b46a-4d52-a414-40d7715712d2", "AQAAAAEAACcQAAAAEOY0GpIHgntqa/SJihAT/EoC5jv2EYsdblPzrmhzzmUJt3DzSnO5ra0L1p4iolPH/Q==" });

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
                value: new DateTime(2023, 7, 5, 17, 46, 51, 708, DateTimeKind.Local).AddTicks(1156));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("b993b2be-4591-4ec9-b319-5fab47ded296"),
                column: "ConcurrencyStamp",
                value: "3800395c-382b-4b70-95e5-a91d94045a7d");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffba58d7-2f16-4902-a35d-d5a0cf3b417c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e00dedfa-9f68-4421-a484-7e5400c7926a", "AQAAAAEAACcQAAAAEOBR2mVTqVCaZy41lovceKDqKbMyoQ/kLyT/m5UP7LdRzLohDX3e5r2q1RxeOVmSjw==" });

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
                value: new DateTime(2023, 7, 5, 9, 53, 40, 536, DateTimeKind.Local).AddTicks(4141));
        }
    }
}
