using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBAccess.Migrations
{
    /// <inheritdoc />
    public partial class Update_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MadeOf",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AudioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MadeOf", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MadeOf_Audio_AudioID",
                        column: x => x.AudioID,
                        principalTable: "Audio",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsedIn",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AudioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedIn", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsedIn_Audio_AudioID",
                        column: x => x.AudioID,
                        principalTable: "Audio",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MadeOf_AudioID",
                table: "MadeOf",
                column: "AudioID");

            migrationBuilder.CreateIndex(
                name: "IX_UsedIn_AudioID",
                table: "UsedIn",
                column: "AudioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MadeOf");

            migrationBuilder.DropTable(
                name: "UsedIn");
        }
    }
}
