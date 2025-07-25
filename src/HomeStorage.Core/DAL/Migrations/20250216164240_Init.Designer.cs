﻿// // <auto-generated />
// using System;
// using HomeStorage.Core.DAL;
// using HomeStorage.Infrastructure.DAL;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Infrastructure;
// using Microsoft.EntityFrameworkCore.Migrations;
// using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
// using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
//
// #nullable disable
//
// namespace HomeStorage.Infrastructure.DAL.Migrations
// {
//     [DbContext(typeof(HomeStorageDbContext))]
//     [Migration("20250216164240_Init")]
//     partial class Init
//     {
//         /// <inheritdoc />
//         protected override void BuildTargetModel(ModelBuilder modelBuilder)
//         {
// #pragma warning disable 612, 618
//             modelBuilder
//                 .HasAnnotation("ProductVersion", "9.0.2")
//                 .HasAnnotation("Relational:MaxIdentifierLength", 63);
//
//             NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);
//
//             modelBuilder.Entity("HomeStorage.Domain.Entities.Location", b =>
//                 {
//                     b.Property<string>("LocationName")
//                         .HasColumnType("text");
//
//                     b.HasKey("LocationName");
//
//                     b.ToTable("Locations");
//                 });
//
//             modelBuilder.Entity("HomeStorage.Domain.Entities.Product", b =>
//                 {
//                     b.Property<Guid>("Id")
//                         .HasColumnType("uuid");
//
//                     b.Property<string>("Description")
//                         .HasColumnType("text");
//
//                     b.Property<string>("LocationName")
//                         .HasColumnType("text");
//
//                     b.Property<string>("Name")
//                         .IsRequired()
//                         .HasColumnType("text");
//
//                     b.Property<string>("Producer")
//                         .HasColumnType("text");
//
//                     b.Property<decimal>("Quantity")
//                         .HasColumnType("numeric");
//
//                     b.HasKey("Id");
//
//                     b.HasIndex("LocationName");
//
//                     b.ToTable("Products");
//                 });
//
//             modelBuilder.Entity("HomeStorage.Domain.Entities.Product", b =>
//                 {
//                     b.HasOne("HomeStorage.Domain.Entities.Location", null)
//                         .WithMany("Products")
//                         .HasForeignKey("LocationName");
//                 });
//
//             modelBuilder.Entity("HomeStorage.Domain.Entities.Location", b =>
//                 {
//                     b.Navigation("Products");
//                 });
// #pragma warning restore 612, 618
//         }
//     }
// }
