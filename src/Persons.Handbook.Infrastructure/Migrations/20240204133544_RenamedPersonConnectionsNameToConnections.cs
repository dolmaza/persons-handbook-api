using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persons.Handbook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamedPersonConnectionsNameToConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonConnections",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "Connections",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ConnectedPersonId = table.Column<int>(type: "int", nullable: false),
                    ConnectionType = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connections_Persons_ConnectedPersonId",
                        column: x => x.ConnectedPersonId,
                        principalSchema: "dbo",
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Connections_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_ConnectedPersonId",
                schema: "dbo",
                table: "Connections",
                column: "ConnectedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_PersonId",
                schema: "dbo",
                table: "Connections",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "PersonConnections",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectedPersonId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ConnectionType = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonConnections_Persons_ConnectedPersonId",
                        column: x => x.ConnectedPersonId,
                        principalSchema: "dbo",
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonConnections_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnections_ConnectedPersonId",
                schema: "dbo",
                table: "PersonConnections",
                column: "ConnectedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnections_PersonId",
                schema: "dbo",
                table: "PersonConnections",
                column: "PersonId");
        }
    }
}
