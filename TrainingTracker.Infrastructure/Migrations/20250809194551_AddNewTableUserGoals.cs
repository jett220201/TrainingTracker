using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTableUserGoals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_Goals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    target_value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    goal_type = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    goal_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_achieved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Goals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Goals_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Goals_user_id",
                table: "User_Goals",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Goals");
        }
    }
}
