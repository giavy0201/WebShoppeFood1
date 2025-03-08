using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _updateAccountStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e4e9367-2e9e-4763-89a8-1d7f5a576fa9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "673b8984-eaf9-4a65-8ed2-781a59dcf53b");

            migrationBuilder.AddColumn<DateTime>(
                name: "AccountActivatedAt",
                table: "AccountStores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AccountCreatedAt",
                table: "AccountStores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AccountStores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "AccountStores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AccountStores",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AccountStores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12837b01-f61d-4676-be78-c16e1ec5faec", "2", "Customer", "Customer" },
                    { "4986524d-06ac-4256-8b90-c7aba1c8962f", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12837b01-f61d-4676-be78-c16e1ec5faec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4986524d-06ac-4256-8b90-c7aba1c8962f");

            migrationBuilder.DropColumn(
                name: "AccountActivatedAt",
                table: "AccountStores");

            migrationBuilder.DropColumn(
                name: "AccountCreatedAt",
                table: "AccountStores");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AccountStores");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "AccountStores");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AccountStores");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AccountStores");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3e4e9367-2e9e-4763-89a8-1d7f5a576fa9", "2", "Customer", "Customer" },
                    { "673b8984-eaf9-4a65-8ed2-781a59dcf53b", "1", "Admin", "Admin" }
                });
        }
    }
}
