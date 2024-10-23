using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogueService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFilterTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstateTypes",
                table: "EstateFilters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "EstateTypes",
                table: "EstateFilters",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }
    }
}
