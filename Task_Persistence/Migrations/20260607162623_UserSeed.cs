using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4afead8d-098f-4cd5-b95d-b1156411fda1"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Username" },
                values: new object[] { new Guid("6f3a3e68-80f2-49af-bb08-2e8b2b71569a"), "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1f8c4e3e5a9b7d2d8b1c5d", "saeed" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6f3a3e68-80f2-49af-bb08-2e8b2b71569a"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Username" },
                values: new object[] { new Guid("4afead8d-098f-4cd5-b95d-b1156411fda1"), "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1f8c4e3e5a9b7d2d8b1c5d", "saeed" });
        }
    }
}
