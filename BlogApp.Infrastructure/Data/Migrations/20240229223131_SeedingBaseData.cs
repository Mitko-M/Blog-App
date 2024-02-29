using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Data.Migrations
{
    public partial class SeedingBaseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "81d2205d-a4b2-4414-92bc-23c7f8b31765", "b1d34529-e7b0-487b-a950-4c04ac671e41", "Admin", "ADMIN" },
                    { "ab918c42-c901-4cba-b099-8d4c5000dbc1", "204e11c1-ae65-4adb-9e81-ced692aee18c", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "6fec0510-8926-4572-b88e-9b181bf68bf6", 0, "cfc5a840-e16f-4e67-91cc-f116166685d9", "admin@blog.com", false, false, null, "ADMIN@BLOG.COM", null, "AQAAAAEAACcQAAAAEAM18L3b8mLET4zs5UThNymiVIAubBwsjnorYt4u6gEaUDR6wHs0qWbzYw5dWuaRZQ==", null, false, "99011de3-91a8-471a-87cc-aadaaf9143d5", false, null },
                    { "b967ba8f-ad12-44fb-8d19-cf4bf9f022d9", 0, "ad9ba667-1b43-467d-a3d1-db9d8016cc46", "user@blog.com", false, false, null, "USER@BLOG.COM", null, "AQAAAAEAACcQAAAAEKCMoyoaSSNPAQUsXAAeuiz7NC6LVO512xSc6LTiNLk/mhlgIPXZN2JfN8UzucIgMA==", null, false, "9563e41c-7028-4bee-a560-7de61f6cfac3", false, null }
                });

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
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "ShortDescription", "Title", "UpdatedOn", "UserId" },
                values: new object[] { 1, "This is my first post's content", new DateTime(2019, 3, 1, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4565), "This is my post's short description", "My First Post", new DateTime(2021, 9, 1, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4598), "b967ba8f-ad12-44fb-8d19-cf4bf9f022d9" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "ShortDescription", "Title", "UpdatedOn", "UserId" },
                values: new object[] { 2, "This is my second post's content", new DateTime(2019, 3, 1, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4602), "This is my post's short description", "My Second Post", new DateTime(2023, 5, 1, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4604), "b967ba8f-ad12-44fb-8d19-cf4bf9f022d9" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "ShortDescription", "Title", "UpdatedOn", "UserId" },
                values: new object[] { 3, "This is my third post's content", new DateTime(2024, 1, 1, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4607), "This is my post's short description", "My Third Post", new DateTime(2024, 2, 25, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4609), "b967ba8f-ad12-44fb-8d19-cf4bf9f022d9" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81d2205d-a4b2-4414-92bc-23c7f8b31765");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab918c42-c901-4cba-b099-8d4c5000dbc1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6fec0510-8926-4572-b88e-9b181bf68bf6");

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
                keyValue: "b967ba8f-ad12-44fb-8d19-cf4bf9f022d9");
        }
    }
}
