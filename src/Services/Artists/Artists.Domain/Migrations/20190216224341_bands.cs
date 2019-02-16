using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Artists.Domain.Migrations
{
    public partial class bands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Artists",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Artists");

            migrationBuilder.AddColumn<Guid>(
                name: "BandId",
                table: "Artists",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Artists",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Artists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Artists",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artists",
                table: "Artists",
                column: "ArtistId");

            migrationBuilder.CreateTable(
                name: "Bands",
                columns: table => new
                {
                    BandId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bands", x => x.BandId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artists_BandId",
                table: "Artists",
                column: "BandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Bands_BandId",
                table: "Artists",
                column: "BandId",
                principalTable: "Bands",
                principalColumn: "BandId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Bands_BandId",
                table: "Artists");

            migrationBuilder.DropTable(
                name: "Bands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artists",
                table: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_Artists_BandId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "BandId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Artists");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Artists",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artists",
                table: "Artists",
                column: "Id");
        }
    }
}
