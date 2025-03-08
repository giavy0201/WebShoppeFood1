using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIntPaymentIdToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2f137cc3-c4c6-4254-a5c3-d3076b3fe418", "1", "Admin", "Admin" },
                    { "a9d3f240-3bba-4658-a484-a0d04cdd962a", "3", "Manager", "Manager" },
                    { "f5ea555f-2d12-438d-91d3-2c43097f10b9", "2", "Customer", "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f137cc3-c4c6-4254-a5c3-d3076b3fe418");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a9d3f240-3bba-4658-a484-a0d04cdd962a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5ea555f-2d12-438d-91d3-2c43097f10b9");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Carts");

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
    }
}
