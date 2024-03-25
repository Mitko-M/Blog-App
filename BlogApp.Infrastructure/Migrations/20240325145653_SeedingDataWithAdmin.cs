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
                    { "1", "202dd7eb-6d03-4cac-b669-23c6b1d9106b", "Admin", "ADMIN" },
                    { "2", "68b91506-4f96-4eb8-9f1d-158eb6432b25", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "32dd459c-ff9f-47f1-ad59-dac146787d30", 0, "1a26ea0d-374d-4449-9418-5345317d1975", "ApplicationUser", "admin@blog.com", false, "Mitko", "Mitkov", false, null, "ADMIN@BLOG.COM", "ADMIN", "AQAAAAEAACcQAAAAEFgmPYxvjQtHow8XQ8FKoVuT/DqRPX6pSLtyQ1nSj2+f6Hjj2UhO6S3t+7E++MYdJQ==", null, false, "efa2669b-9937-4e01-ae56-1cbbd428ca8a", false, "admin" });

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
                values: new object[] { "1", "32dd459c-ff9f-47f1-ad59-dac146787d30" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "ShortDescription", "Title", "UpdatedOn", "UserId" },
                values: new object[,]
                {
                    { 1, "This is my first post's content", new DateTime(2019, 3, 25, 16, 56, 52, 884, DateTimeKind.Local).AddTicks(357), "This is my post's short description", "My First Post", new DateTime(2021, 9, 25, 16, 56, 52, 884, DateTimeKind.Local).AddTicks(393), "32dd459c-ff9f-47f1-ad59-dac146787d30" },
                    { 2, "This is my second post's content", new DateTime(2019, 3, 25, 16, 56, 52, 884, DateTimeKind.Local).AddTicks(399), "This is my post's short description", "My Second Post", new DateTime(2023, 5, 25, 16, 56, 52, 884, DateTimeKind.Local).AddTicks(401), "32dd459c-ff9f-47f1-ad59-dac146787d30" },
                    { 3, "This is my third post's content", new DateTime(2024, 1, 25, 16, 56, 52, 884, DateTimeKind.Local).AddTicks(405), "This is my post's short description", "My Third Post", new DateTime(2024, 3, 20, 16, 56, 52, 884, DateTimeKind.Local).AddTicks(406), "32dd459c-ff9f-47f1-ad59-dac146787d30" }
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
                keyValues: new object[] { "1", "32dd459c-ff9f-47f1-ad59-dac146787d30" });

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
                keyValue: "32dd459c-ff9f-47f1-ad59-dac146787d30");
        }
    }
}
