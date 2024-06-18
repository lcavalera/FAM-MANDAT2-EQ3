using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acef.Raisons.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class corrModelRaison : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NomRaison",
                table: "Raison",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Raison",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Raison",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Raison",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Raison");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Raison");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Raison",
                newName: "NomRaison");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Raison",
                newName: "IsActive");
        }
    }
}
