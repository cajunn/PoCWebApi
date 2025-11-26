using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PoCWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Oid", "DisplayName", "Email" },
                values: new object[,]
                {
                    { new Guid("6047da8a-10f3-4de0-b3e2-56d9ccb08573"), "MarcoCSR", "MarcoCSR@test.com" },
                    { new Guid("a3495fa6-da05-459c-a0cf-72103747fc78"), "MarcoAdmin", "MarcoAdmin@test.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Oid",
                keyValue: new Guid("6047da8a-10f3-4de0-b3e2-56d9ccb08573"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Oid",
                keyValue: new Guid("a3495fa6-da05-459c-a0cf-72103747fc78"));
        }
    }
}
