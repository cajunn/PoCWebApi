using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PoCWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRolesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Oid", "RoleId" },
                values: new object[,]
                {
                    { new Guid("6047da8a-10f3-4de0-b3e2-56d9ccb08573"), 1 },
                    { new Guid("a3495fa6-da05-459c-a0cf-72103747fc78"), 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "Oid", "RoleId" },
                keyValues: new object[] { new Guid("6047da8a-10f3-4de0-b3e2-56d9ccb08573"), 1 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "Oid", "RoleId" },
                keyValues: new object[] { new Guid("a3495fa6-da05-459c-a0cf-72103747fc78"), 2 });
        }
    }
}
