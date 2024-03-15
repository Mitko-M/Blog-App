using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Data.Migrations
{
    public partial class AddedCommentDateAndChangePostContentMaxChars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b967ba8f-ad12-44fb-8d19-cf4bf9f022d9");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                comment: "Post content",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "Post content");

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentUploadDate",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4486552e-cc43-47ad-b894-4b3bfcea59e0", "5b1df686-9b39-4e16-88f9-cf858683aa73", "User", "USER" },
                    { "cdb58854-68f0-4d92-936d-2b171dbf1b04", "dd8624e5-6bcb-4948-b197-fc0ab3824dd5", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "2f71e6c3-9554-4eeb-b967-f6d31f764b3e", 0, "966ee9b0-243f-4a0f-b271-37a798c60744", "admin@blog.com", false, false, null, "ADMIN@BLOG.COM", null, "AQAAAAEAACcQAAAAEFl7vmkWNqXdJJ79LTUv1W0SaaEyJRyrWO3za+ynfGkZHRq9R1KFPllwO1wf2Fh0cw==", null, false, "de5aaf7b-6540-452d-b3be-da3c6477e7e0", false, null },
                    { "aa3eea82-6364-4821-a1d3-f5e11170c209", 0, "4080d65c-e806-4575-8efb-d7fa605dc985", "user@blog.com", false, false, null, "USER@BLOG.COM", null, "AQAAAAEAACcQAAAAEIEtg4X93SHWV00LdM80lHI/1mSY1JYISTcRX6IsoRl2nsLu/B9I4u+qF95uIUdjHA==", null, false, "eed126e7-535b-4f40-b488-19e7fdad54e2", false, null }
                });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2019, 3, 16, 0, 11, 44, 257, DateTimeKind.Local).AddTicks(98), new DateTime(2021, 9, 16, 0, 11, 44, 257, DateTimeKind.Local).AddTicks(132), "aa3eea82-6364-4821-a1d3-f5e11170c209" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2019, 3, 16, 0, 11, 44, 257, DateTimeKind.Local).AddTicks(137), new DateTime(2023, 5, 16, 0, 11, 44, 257, DateTimeKind.Local).AddTicks(139), "aa3eea82-6364-4821-a1d3-f5e11170c209" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2024, 1, 16, 0, 11, 44, 257, DateTimeKind.Local).AddTicks(142), new DateTime(2024, 3, 11, 0, 11, 44, 257, DateTimeKind.Local).AddTicks(144), "aa3eea82-6364-4821-a1d3-f5e11170c209" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4486552e-cc43-47ad-b894-4b3bfcea59e0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cdb58854-68f0-4d92-936d-2b171dbf1b04");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2f71e6c3-9554-4eeb-b967-f6d31f764b3e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aa3eea82-6364-4821-a1d3-f5e11170c209");

            migrationBuilder.DropColumn(
                name: "CommentUploadDate",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "Post content",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldComment: "Post content");

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

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2019, 3, 1, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4565), new DateTime(2021, 9, 1, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4598), "b967ba8f-ad12-44fb-8d19-cf4bf9f022d9" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2019, 3, 1, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4602), new DateTime(2023, 5, 1, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4604), "b967ba8f-ad12-44fb-8d19-cf4bf9f022d9" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "UpdatedOn", "UserId" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4607), new DateTime(2024, 2, 25, 0, 31, 31, 622, DateTimeKind.Local).AddTicks(4609), "b967ba8f-ad12-44fb-8d19-cf4bf9f022d9" });
        }
    }
}
