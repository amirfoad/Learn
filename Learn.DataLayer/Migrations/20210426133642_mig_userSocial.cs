using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class mig_userSocial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaceBook",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Linkedin",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceBook",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Linkedin",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Twitter",
                schema: "dbo",
                table: "Users");
        }
    }
}
