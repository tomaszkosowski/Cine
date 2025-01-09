using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cine.Modules.Tickets.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Show_HallId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HallId",
                table: "Shows",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ShowId",
                table: "Seats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HallId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "ShowId",
                table: "Seats");
        }
    }
}
