using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cine.Modules.Tickets.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Show_StartAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartAt",
                table: "Shows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartAt",
                table: "Shows");
        }
    }
}
