using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class EpisodSeason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEpisodes_Products_ProductId",
                schema: "dbo",
                table: "ProductEpisodes");

            migrationBuilder.DropIndex(
                name: "IX_ProductEpisodes_ProductId",
                schema: "dbo",
                table: "ProductEpisodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductEpisodes_ProductId",
                schema: "dbo",
                table: "ProductEpisodes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEpisodes_Products_ProductId",
                schema: "dbo",
                table: "ProductEpisodes",
                column: "ProductId",
                principalSchema: "dbo",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
