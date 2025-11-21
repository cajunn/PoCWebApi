using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoCWebApi.Migrations
{
    /// <inheritdoc />
    public partial class addPIITodos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PIITodos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerOid = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PIITitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PIITodos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PIITodos_OwnerOid_IsDone",
                table: "PIITodos",
                columns: new[] { "OwnerOid", "IsDone" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PIITodos");
        }
    }
}
