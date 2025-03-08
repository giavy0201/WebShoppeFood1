using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDistrictAndCityToStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stores_StoreId",
                table: "Comments");

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

            migrationBuilder.AddColumn<int>(
                name: "CityID",
                table: "Stores",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DistrictID",
                table: "Stores",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e8c759c-be98-4c25-a263-2fdc07c82861", "3", "Manager", "Manager" },
                    { "6ec8e9a7-47b5-4369-a21d-604571231b1a", "2", "Customer", "Customer" },
                    { "ce1abbb2-18a6-4067-b141-e2ffc78c590f", "1", "Admin", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CityID",
                table: "Stores",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_DistrictID",
                table: "Stores",
                column: "DistrictID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stores_StoreId",
                table: "Comments",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Cities_CityID",
                table: "Stores",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Restrict); // Sửa từ Cascade thành Restrict

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Districts_DistrictID",
                table: "Stores",
                column: "DistrictID",
                principalTable: "Districts",
                principalColumn: "DistrictID",
               onDelete: ReferentialAction.Restrict);  // Sửa từ Cascade thành Restrict
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stores_StoreId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Cities_CityID",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Districts_DistrictID",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_CityID",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_DistrictID",
                table: "Stores");

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

            migrationBuilder.DropColumn(
                name: "CityID",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "DistrictID",
                table: "Stores");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "25fd12fc-f0e2-49e9-a674-6d89e711d96b", "1", "Admin", "Admin" },
                    { "82fbafac-4ee1-4cb8-b03a-eb0c7a411eb0", "2", "Customer", "Customer" },
                    { "bb9eff52-c469-47fa-9b72-d47311395b03", "3", "Manager", "Manager" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stores_StoreId",
                table: "Comments",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "StoreID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
