using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPNETCOREAPP.Migrations
{
    public partial class foridstring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Userid",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Userid",
                table: "AspNetUsers");
        }
    }
}
