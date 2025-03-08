using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsGlobalAdminToAccountStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12837b01-f61d-4676-be78-c16e1ec5faec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4986524d-06ac-4256-8b90-c7aba1c8962f");

            migrationBuilder.AddColumn<bool>(
                name: "IsGlobalAdmin",
                table: "AccountStores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "25fd12fc-f0e2-49e9-a674-6d89e711d96b", "1", "Admin", "Admin" },
                    { "82fbafac-4ee1-4cb8-b03a-eb0c7a411eb0", "2", "Customer", "Customer" },
                    { "bb9eff52-c469-47fa-9b72-d47311395b03", "3", "Manager", "Manager" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "25fd12fc-f0e2-49e9-a674-6d89e711d96b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82fbafac-4ee1-4cb8-b03a-eb0c7a411eb0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb9eff52-c469-47fa-9b72-d47311395b03");

            migrationBuilder.DropColumn(
                name: "IsGlobalAdmin",
                table: "AccountStores");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12837b01-f61d-4676-be78-c16e1ec5faec", "2", "Customer", "Customer" },
                    { "4986524d-06ac-4256-8b90-c7aba1c8962f", "1", "Admin", "Admin" }
                });
        }
    }
}
