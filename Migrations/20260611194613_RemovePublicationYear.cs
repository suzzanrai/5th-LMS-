using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_Project.Migrations
{
    /// <inheritdoc />
    public partial class RemovePublicationYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicationYear",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublicationYear",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
