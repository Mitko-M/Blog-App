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
                    { "1", "09387403-33d0-42f0-a617-5af491693fc9", "Admin", "ADMIN" },
                    { "2", "0d65e9d8-7b91-4a0c-ac2c-f80e85be7922", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "03cdc70e-2c5b-4306-9477-966ac9c13bfc", "ApplicationUser", "admin@blog.com", false, "Mitko", "Mitkov", false, null, "ADMIN@BLOG.COM", "ADMIN", "AQAAAAEAACcQAAAAEE3wyrsCJOXUs1/JQTAh5bFwd/aSXmlJL7QwGw5X9sbb9a0y1nY+BNiCuJCHOaA/Yg==", null, false, "c99a8f18-d157-4c81-b9a9-90af60de086d", false, "admin" });

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
                values: new object[] { "1", "1" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "ShortDescription", "Title", "UpdatedOn", "UserId" },
                values: new object[,]
                {
                    { 1, "This is my first post's content", new DateTime(2019, 3, 21, 23, 23, 40, 549, DateTimeKind.Local).AddTicks(8410), "This is my post's short description", "My First Post", new DateTime(2021, 9, 21, 23, 23, 40, 549, DateTimeKind.Local).AddTicks(8446), "1" },
                    { 2, "This is my second post's content", new DateTime(2019, 3, 21, 23, 23, 40, 549, DateTimeKind.Local).AddTicks(8453), "This is my post's short description", "My Second Post", new DateTime(2023, 5, 21, 23, 23, 40, 549, DateTimeKind.Local).AddTicks(8455), "1" },
                    { 3, "This is my third post's content", new DateTime(2024, 1, 21, 23, 23, 40, 549, DateTimeKind.Local).AddTicks(8459), "This is my post's short description", "My Third Post", new DateTime(2024, 3, 16, 23, 23, 40, 549, DateTimeKind.Local).AddTicks(8460), "1" }
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
                keyValue: "1");
        }
    }
}
