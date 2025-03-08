using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "164aa17e-2938-4472-8495-09caa478db63");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c4317ef-3dee-4b63-9de5-5fc8778320e2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb035825-51e0-41b0-8f5b-df846467f005");

            migrationBuilder.AlterColumn<string>(
                name: "ShipperId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentTime",
                table: "Carts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0564f60d-fa99-499a-ba92-bb2ffc8cf492", "2", "Customer", "Customer" },
                    { "3b50c084-97c8-4aea-8a48-46dd543188aa", "3", "Manager", "Manager" },
                    { "c6471533-6e72-4137-944c-4f2444476994", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0564f60d-fa99-499a-ba92-bb2ffc8cf492");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b50c084-97c8-4aea-8a48-46dd543188aa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6471533-6e72-4137-944c-4f2444476994");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "PaymentTime",
                table: "Carts");

            migrationBuilder.AlterColumn<string>(
                name: "ShipperId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "164aa17e-2938-4472-8495-09caa478db63", "2", "Customer", "Customer" },
                    { "2c4317ef-3dee-4b63-9de5-5fc8778320e2", "3", "Manager", "Manager" },
                    { "bb035825-51e0-41b0-8f5b-df846467f005", "1", "Admin", "Admin" }
                });
        }
    }
}
