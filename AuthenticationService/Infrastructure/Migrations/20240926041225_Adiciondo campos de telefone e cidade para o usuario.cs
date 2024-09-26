using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.AuthenticationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adiciondocamposdetelefoneecidadeparaousuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "ApplicationUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "ApplicationUsers",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "ApplicationUsers");
        }
    }
}
