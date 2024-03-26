using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Infrastructure.Migrations
{
    public partial class SeedingDataWithAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "9660295d-812e-4af2-bd99-a7837f872695", "Admin", "ADMIN" },
                    { "2", "f9caf067-2c42-41a8-a902-378628f95352", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7850d3f5-bbc1-409e-9237-91e8afb5ef70", 0, "06f03200-4dc3-45fc-a3b6-782c073ee847", "ApplicationUser", "admin@blog.com", false, "Mitko", "Mitkov", false, null, "ADMIN@BLOG.COM", "ADMIN", "AQAAAAEAACcQAAAAEPNezvQpiQzJZLIoNzrwnr0Bu0OeuctLfuqIdxP/gp0uCrgICvoGG0SEv/ItLYD2pA==", null, false, "c912d0f1-bc81-433e-8dfd-7638b1291aa8", false, "admin" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Technology" },
                    { 2, "Science" },
                    { 3, "Art" },
                    { 4, "Travel" },
                    { 5, "Lifestyle" },
                    { 6, "Education" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ASP.NET" },
                    { 2, "C#" },
                    { 3, "JavaScript" },
                    { 4, "HTML" },
                    { 5, "CSS" },
                    { 6, "SQL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "7850d3f5-bbc1-409e-9237-91e8afb5ef70" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "ShortDescription", "Title", "UpdatedOn", "UserId" },
                values: new object[,]
                {
                    { 1, "This is my first post's content", new DateTime(2019, 3, 26, 20, 2, 3, 312, DateTimeKind.Local).AddTicks(8517), "This is my post's short description", "My First Post", new DateTime(2021, 9, 26, 20, 2, 3, 312, DateTimeKind.Local).AddTicks(8554), "7850d3f5-bbc1-409e-9237-91e8afb5ef70" },
                    { 2, "This is my second post's content", new DateTime(2019, 3, 26, 20, 2, 3, 312, DateTimeKind.Local).AddTicks(8562), "This is my post's short description", "My Second Post", new DateTime(2023, 5, 26, 20, 2, 3, 312, DateTimeKind.Local).AddTicks(8564), "7850d3f5-bbc1-409e-9237-91e8afb5ef70" },
                    { 3, "This is my third post's content", new DateTime(2024, 1, 26, 20, 2, 3, 312, DateTimeKind.Local).AddTicks(8567), "This is my post's short description", "My Third Post", new DateTime(2024, 3, 21, 20, 2, 3, 312, DateTimeKind.Local).AddTicks(8569), "7850d3f5-bbc1-409e-9237-91e8afb5ef70" }
                });

            migrationBuilder.InsertData(
                table: "PostsCategories",
                columns: new[] { "CategoryId", "PostId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "PostsTags",
                columns: new[] { "PostId", "TagId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "7850d3f5-bbc1-409e-9237-91e8afb5ef70" });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PostsCategories",
                keyColumns: new[] { "CategoryId", "PostId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "PostsCategories",
                keyColumns: new[] { "CategoryId", "PostId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "PostsCategories",
                keyColumns: new[] { "CategoryId", "PostId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "PostsTags",
                keyColumns: new[] { "PostId", "TagId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "PostsTags",
                keyColumns: new[] { "PostId", "TagId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "PostsTags",
                keyColumns: new[] { "PostId", "TagId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7850d3f5-bbc1-409e-9237-91e8afb5ef70");
        }
    }
}
