using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e8c759c-be98-4c25-a263-2fdc07c82861");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ec8e9a7-47b5-4369-a21d-604571231b1a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce1abbb2-18a6-4067-b141-e2ffc78c590f");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e8c759c-be98-4c25-a263-2fdc07c82861", "3", "Manager", "Manager" },
                    { "6ec8e9a7-47b5-4369-a21d-604571231b1a", "2", "Customer", "Customer" },
                    { "ce1abbb2-18a6-4067-b141-e2ffc78c590f", "1", "Admin", "Admin" }
                });
        }
    }
}
