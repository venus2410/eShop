using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShop.Data.Migrations
{
    public partial class ChangeFileLengthType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("b993b2be-4591-4ec9-b319-5fab47ded296"),
                column: "ConcurrencyStamp",
                value: "7ddbd318-232a-43de-97f6-04a053321337");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffba58d7-2f16-4902-a35d-d5a0cf3b417c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "dab0c4dc-978e-4c4a-9adc-bac1600501d3", "AQAAAAEAACcQAAAAENyWsj/JXadLocIUkerHISlb+0QmM5SMO7m2PHwmx3ojo9KARl1FmBpCyvxgSLxu9w==" });

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
                value: new DateTime(2023, 6, 15, 10, 51, 38, 87, DateTimeKind.Local).AddTicks(9374));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
