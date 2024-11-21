using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogueService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDateTimesToAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Estates",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Estates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EstateFilters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EstateFilters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Estates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldNullable: false,
                oldDefaultValueSql: "NOW()");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Estates");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EstateFilters");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "EstateFilters");
        }
    }
}
