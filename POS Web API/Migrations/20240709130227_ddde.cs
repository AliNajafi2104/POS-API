using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_API.Migrations
{
    /// <inheritdoc />
    public partial class ddde : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_productCount",
                table: "productCount");

            migrationBuilder.RenameTable(
                name: "productCount",
                newName: "ProductCount");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCount",
                table: "ProductCount",
                column: "ProductCountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCount",
                table: "ProductCount");

            migrationBuilder.RenameTable(
                name: "ProductCount",
                newName: "productCount");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productCount",
                table: "productCount",
                column: "ProductCountID");
        }
    }
}
