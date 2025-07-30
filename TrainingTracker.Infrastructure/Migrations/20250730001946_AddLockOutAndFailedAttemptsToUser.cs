using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLockOutAndFailedAttemptsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "failed_login_attempts",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "lock_out_end",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "failed_login_attempts",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "lock_out_end",
                table: "Users");
        }
    }
}
