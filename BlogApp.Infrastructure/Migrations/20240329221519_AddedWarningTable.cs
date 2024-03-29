using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Infrastructure.Migrations
{
    public partial class AddedWarningTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Warning",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Warning identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Warned user's identifier"),
                    HiddenPostId = table.Column<int>(type: "int", nullable: false, comment: "A post which was reported then checked hence the owner was warned and then the post hidden and left for the owner to delete it"),
                    WarningReason = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The reason for adding a warning")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warning", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warning_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warning_UserId",
                table: "Warning",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Warning");

            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "Posts");
        }
    }
}
