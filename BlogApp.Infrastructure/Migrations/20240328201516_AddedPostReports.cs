using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Infrastructure.Migrations
{
    public partial class AddedPostReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostsReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostsReports_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostsReports_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostsReports_PostId",
                table: "PostsReports",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostsReports_UserId",
                table: "PostsReports",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostsReports");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "79f84b72-a493-4acd-8dd2-f4ac73e42c52" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "79f84b72-a493-4acd-8dd2-f4ac73e42c52");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "685752fe-e2f3-4c10-92ac-62b65fb13fa5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "d6b2de01-f334-4804-a3b6-ea53a04876f6");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6da7a3e5-8411-4229-a9aa-ff572d6cba23", 0, "6306f5d1-331e-4b50-991e-5c9f6103e820", "admin@blog.com", false, "Mitko", "Mitkov", false, null, "ADMIN@BLOG.COM", "ADMIN", "AQAAAAEAACcQAAAAEOjIlZV41ilbbtr8FXChSnermxUNiGrTeYiSFsqlT7/4mUb9QOXqmSKc8DcKGu0idw==", null, false, "977c0b25-c2fb-4aec-aa3b-7062e730ba4c", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "6da7a3e5-8411-4229-a9aa-ff572d6cba23" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2019, 3, 28, 14, 38, 29, 954, DateTimeKind.Local).AddTicks(6399), new DateTime(2021, 9, 28, 14, 38, 29, 954, DateTimeKind.Local).AddTicks(6437), "6da7a3e5-8411-4229-a9aa-ff572d6cba23" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2019, 3, 28, 14, 38, 29, 954, DateTimeKind.Local).AddTicks(6444), new DateTime(2023, 5, 28, 14, 38, 29, 954, DateTimeKind.Local).AddTicks(6446), "6da7a3e5-8411-4229-a9aa-ff572d6cba23" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2024, 1, 28, 14, 38, 29, 954, DateTimeKind.Local).AddTicks(6449), new DateTime(2024, 3, 23, 14, 38, 29, 954, DateTimeKind.Local).AddTicks(6450), "6da7a3e5-8411-4229-a9aa-ff572d6cba23" });
        }
    }
}
