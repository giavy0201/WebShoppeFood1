using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddShipperIdToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "006d34ac-7b5e-4341-9eaf-9bc669a4a7df");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c01110d-67b5-463f-b6d2-0134608ff2cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89fec195-390e-427c-a40a-a1edf5d30a59");

            migrationBuilder.AddColumn<string>(
                name: "ShipperId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ShipperId",
                table: "Carts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "006d34ac-7b5e-4341-9eaf-9bc669a4a7df", "2", "Customer", "Customer" },
                    { "0c01110d-67b5-463f-b6d2-0134608ff2cd", "1", "Admin", "Admin" },
                    { "89fec195-390e-427c-a40a-a1edf5d30a59", "3", "Manager", "Manager" }
                });
        }
    }
}
