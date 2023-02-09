using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class mig_manba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManbaId",
                schema: "dbo",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Manba",
                schema: "dbo",
                columns: table => new
                {
                    ManbaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manba", x => x.ManbaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManbaId",
                schema: "dbo",
                table: "Products",
                column: "ManbaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Manba_ManbaId",
                schema: "dbo",
                table: "Products",
                column: "ManbaId",
                principalSchema: "dbo",
                principalTable: "Manba",
                principalColumn: "ManbaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Manba_ManbaId",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Manba",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Products_ManbaId",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ManbaId",
                schema: "dbo",
                table: "Products");
        }
    }
}
