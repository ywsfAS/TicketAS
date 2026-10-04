using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations;

[DbContext(typeof(TicketDbContext))]
[Migration("20261004181500_CreateNetworkIncidentCategories")]
public partial class CreateNetworkIncidentCategories : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "NetworkIncidentCategories",
            columns: table => new
            {
                CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Symptom = table.Column<byte>(type: "tinyint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NetworkIncidentCategories", x => x.CategoryId);
                table.ForeignKey(
                    name: "FK_NetworkIncidentCategories_IncidentCategories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "IncidentCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "NetworkIncidentCategories");
    }
}
