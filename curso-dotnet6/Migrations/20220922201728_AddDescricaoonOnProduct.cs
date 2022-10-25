using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curso_dotnet6.Migrations
{
    public partial class AddDescricaoonOnProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Products");
        }
    }
}
