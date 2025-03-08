using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMomoPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

         


           

            migrationBuilder.AddColumn<string>(
                name: "MomoStatus",
                table: "Carts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MomoTransactionId",
                table: "Carts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3b586042-c431-4df5-b847-6b746f279f40", "2", "Customer", "Customer" },
                    { "7726840d-39cc-44d7-ad08-ce17150887ce", "3", "Manager", "Manager" },
                    { "b84b97ab-3732-4bc9-998a-1e1eaaa5343e", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b586042-c431-4df5-b847-6b746f279f40");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7726840d-39cc-44d7-ad08-ce17150887ce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b84b97ab-3732-4bc9-998a-1e1eaaa5343e");

            migrationBuilder.DropColumn(
                name: "MomoStatus",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "MomoTransactionId",
                table: "Carts");

         

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Carts",
                type: "int",
                nullable: true);

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
    }
}
