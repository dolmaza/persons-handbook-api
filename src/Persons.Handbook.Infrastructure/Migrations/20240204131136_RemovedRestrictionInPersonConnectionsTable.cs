using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persons.Handbook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRestrictionInPersonConnectionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonConnections_Persons_ConnectedPersonId",
                schema: "dbo",
                table: "PersonConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonConnections_Persons_PersonId",
                schema: "dbo",
                table: "PersonConnections");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonConnections_Persons_ConnectedPersonId",
                schema: "dbo",
                table: "PersonConnections",
                column: "ConnectedPersonId",
                principalSchema: "dbo",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonConnections_Persons_PersonId",
                schema: "dbo",
                table: "PersonConnections",
                column: "PersonId",
                principalSchema: "dbo",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonConnections_Persons_ConnectedPersonId",
                schema: "dbo",
                table: "PersonConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonConnections_Persons_PersonId",
                schema: "dbo",
                table: "PersonConnections");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonConnections_Persons_ConnectedPersonId",
                schema: "dbo",
                table: "PersonConnections",
                column: "ConnectedPersonId",
                principalSchema: "dbo",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonConnections_Persons_PersonId",
                schema: "dbo",
                table: "PersonConnections",
                column: "PersonId",
                principalSchema: "dbo",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
