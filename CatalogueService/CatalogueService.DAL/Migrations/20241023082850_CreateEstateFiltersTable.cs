using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogueService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateEstateFiltersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstateFilters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EstateTypes = table.Column<int[]>(type: "integer[]", nullable: false),
                    MinArea = table.Column<int>(type: "integer", nullable: false),
                    MaxArea = table.Column<int>(type: "integer", nullable: false),
                    MinRoomsCount = table.Column<short>(type: "smallint", nullable: false),
                    MaxRoomsCount = table.Column<short>(type: "smallint", nullable: false),
                    MinPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateFilters", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstateFilters");
        }
    }
}
