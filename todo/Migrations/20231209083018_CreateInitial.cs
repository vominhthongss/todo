using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todo.Migrations
{
    public partial class CreateInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Misson",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Misson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HashedPassword = table.Column<string>(type: "varchar(128)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Account",
                table: "User",
                column: "Account");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Misson");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
