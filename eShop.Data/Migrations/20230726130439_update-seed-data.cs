using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShop.Data.Migrations
{
    public partial class updateseeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("b993b2be-4591-4ec9-b319-5fab47ded296"),
                column: "ConcurrencyStamp",
                value: "5789201e-024c-43c6-8af7-08a57fc71c91");

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("919c87c2-7256-43b6-8889-0473962f26f9"), "1b7169f5-39ae-46a5-86b2-330694c80cca", "User role", "user", "user" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffba58d7-2f16-4902-a35d-d5a0cf3b417c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d808432f-1101-446e-b2cb-a771164516cd", "AQAAAAEAACcQAAAAEEic5/+7cQBsckFLD6MIF5+4tsGs+8E/7YzKlBtrIv4+PAB3OOFsFx4yuRN3PfqkgQ==" });

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
                value: new DateTime(2023, 7, 26, 20, 4, 38, 661, DateTimeKind.Local).AddTicks(5888));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("919c87c2-7256-43b6-8889-0473962f26f9"));

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
    }
}
