// using System;
// using Microsoft.EntityFrameworkCore.Migrations;
//
// #nullable disable
//
// namespace HomeStorage.Infrastructure.DAL.Migrations
// {
//     /// <inheritdoc />
//     public partial class Init : Migration
//     {
//         /// <inheritdoc />
//         protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.CreateTable(
//                 name: "Locations",
//                 columns: table => new
//                 {
//                     LocationName = table.Column<string>(type: "text", nullable: false)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Locations", x => x.LocationName);
//                 });
//
//             migrationBuilder.CreateTable(
//                 name: "Products",
//                 columns: table => new
//                 {
//                     Id = table.Column<Guid>(type: "uuid", nullable: false),
//                     Name = table.Column<string>(type: "text", nullable: false),
//                     Quantity = table.Column<decimal>(type: "numeric", nullable: false),
//                     Description = table.Column<string>(type: "text", nullable: true),
//                     Producer = table.Column<string>(type: "text", nullable: true),
//                     LocationName = table.Column<string>(type: "text", nullable: true)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Products", x => x.Id);
//                     table.ForeignKey(
//                         name: "FK_Products_Locations_LocationName",
//                         column: x => x.LocationName,
//                         principalTable: "Locations",
//                         principalColumn: "LocationName");
//                 });
//
//             migrationBuilder.CreateIndex(
//                 name: "IX_Products_LocationName",
//                 table: "Products",
//                 column: "LocationName");
//         }
//
//         /// <inheritdoc />
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.DropTable(
//                 name: "Products");
//
//             migrationBuilder.DropTable(
//                 name: "Locations");
//         }
//     }
// }
