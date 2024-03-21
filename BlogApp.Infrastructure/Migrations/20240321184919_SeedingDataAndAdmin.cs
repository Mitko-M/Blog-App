using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Infrastructure.Migrations
{
    public partial class SeedingDataAndAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "54007d7c-5d94-4d4a-9a76-93a4a844eddc", "Admin", "ADMIN" },
                    { "2", "0dad8e03-fa5b-4d0a-b4ed-a9d441f4972a", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "9452d8d9-63c5-479f-8558-740da9951372", "ApplicationUser", "admin@blog.com", false, "Mitko", "Mitkov", false, null, "ADMIN@BLOG.COM", "ADMIN", "AQAAAAEAACcQAAAAEL3P5uiSpyTRZq/f6u9P5ChA/iEPRG5ya8Js/h+/rXPtqVnKpRFXz/RmJj6eGb8XEg==", null, false, "12706ad9-db3b-49e6-a906-45f832bfab19", false, "admin" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Science" },
                    { 2, "Nature" },
                    { 3, "IT and Computer Science" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Funny" },
                    { 2, "Interesting" },
                    { 3, "Boring" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "1" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "ShortDescription", "Title", "UpdatedOn", "UserId" },
                values: new object[,]
                {
                    { 1, "This is my first post's content", new DateTime(2019, 3, 21, 20, 49, 18, 913, DateTimeKind.Local).AddTicks(5208), "This is my post's short description", "My First Post", new DateTime(2021, 9, 21, 20, 49, 18, 913, DateTimeKind.Local).AddTicks(5239), "1" },
                    { 2, "This is my second post's content", new DateTime(2019, 3, 21, 20, 49, 18, 913, DateTimeKind.Local).AddTicks(5244), "This is my post's short description", "My Second Post", new DateTime(2023, 5, 21, 20, 49, 18, 913, DateTimeKind.Local).AddTicks(5247), "1" },
                    { 3, "This is my third post's content", new DateTime(2024, 1, 21, 20, 49, 18, 913, DateTimeKind.Local).AddTicks(5250), "This is my post's short description", "My Third Post", new DateTime(2024, 3, 16, 20, 49, 18, 913, DateTimeKind.Local).AddTicks(5251), "1" }
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
                keyValues: new object[] { "1", "1" });

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
                keyValue: "1");
        }
    }
}
