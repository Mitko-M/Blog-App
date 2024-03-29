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

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "e12b58bc-530b-4246-89f4-e4383154d41b" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e12b58bc-530b-4246-89f4-e4383154d41b");

            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "Posts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "6ccc0560-c7e1-439c-b43e-7473a6cc353b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "7d288b08-53d9-4e4f-ab59-06dd666222e5");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "79f84b72-a493-4acd-8dd2-f4ac73e42c52", 0, "49e367ca-8c67-4ca0-839f-c8c3277cccc4", "admin@blog.com", false, "Mitko", "Mitkov", false, null, "ADMIN@BLOG.COM", "ADMIN", "AQAAAAEAACcQAAAAELl3IKJlp+j0eYWLlbTiCY+2PJhl7zGP892qiMsXTSr5dtlkPGkA3B5IRMRTabNXTw==", null, false, "d7e81149-b708-48bb-83c7-0943bcfef05b", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "79f84b72-a493-4acd-8dd2-f4ac73e42c52" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2019, 3, 28, 22, 15, 16, 309, DateTimeKind.Local).AddTicks(3507), new DateTime(2021, 9, 28, 22, 15, 16, 309, DateTimeKind.Local).AddTicks(3539), "79f84b72-a493-4acd-8dd2-f4ac73e42c52" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2019, 3, 28, 22, 15, 16, 309, DateTimeKind.Local).AddTicks(3545), new DateTime(2023, 5, 28, 22, 15, 16, 309, DateTimeKind.Local).AddTicks(3547), "79f84b72-a493-4acd-8dd2-f4ac73e42c52" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2024, 1, 28, 22, 15, 16, 309, DateTimeKind.Local).AddTicks(3550), new DateTime(2024, 3, 23, 22, 15, 16, 309, DateTimeKind.Local).AddTicks(3552), "79f84b72-a493-4acd-8dd2-f4ac73e42c52" });
        }
    }
}
