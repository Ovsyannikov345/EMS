﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogueService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeFlagColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstateTypes",
                table: "EstateFilters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstateTypes",
                table: "EstateFilters");
        }
    }
}