using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_BlogProject.Migrations
{
    public partial class Createdtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Createdtime",
                table: "Articles",
                newName: "CreatedTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Articles",
                newName: "Createdtime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Createdtime",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");
        }
    }
}
