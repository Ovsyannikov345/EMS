﻿// <auto-generated />
using System;
using CatalogueService.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CatalogueService.DAL.Migrations
{
    [DbContext(typeof(EstateDbContext))]
    [Migration("20241118133105_AddDateTimesToAllEntities")]
    partial class AddDateTimesToAllEntities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CatalogueService.DAL.Models.Entities.Estate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Area")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<short>("RoomsCount")
                        .HasColumnType("smallint");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Estates");
                });

            modelBuilder.Entity("CatalogueService.DAL.Models.Entities.EstateFilter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EstateTypes")
                        .HasColumnType("integer");

                    b.Property<int>("MaxArea")
                        .HasColumnType("integer");

                    b.Property<decimal>("MaxPrice")
                        .HasColumnType("numeric");

                    b.Property<short>("MaxRoomsCount")
                        .HasColumnType("smallint");

                    b.Property<int>("MinArea")
                        .HasColumnType("integer");

                    b.Property<decimal>("MinPrice")
                        .HasColumnType("numeric");

                    b.Property<short>("MinRoomsCount")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("EstateFilters");
                });
#pragma warning restore 612, 618
        }
    }
}
