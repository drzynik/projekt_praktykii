using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace praktyki_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GamemasterRequests",
                columns: table => new
                {
                    Request_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamemasterRequests", x => x.Request_Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Users_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Names = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Users_Id);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Decks_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Deck_Name = table.Column<string>(type: "TEXT", nullable: false),
                    Users_Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Decks_Id);
                    table.ForeignKey(
                        name: "FK_Decks_Users_Users_Id",
                        column: x => x.Users_Id,
                        principalTable: "Users",
                        principalColumn: "Users_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Cards_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Decks_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Card_Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Cards_Id);
                    table.ForeignKey(
                        name: "FK_Cards_Decks_Decks_Id",
                        column: x => x.Decks_Id,
                        principalTable: "Decks",
                        principalColumn: "Decks_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gameevents",
                columns: table => new
                {
                    Games_Events_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Decks_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Events_Short_Desc = table.Column<string>(type: "TEXT", nullable: false),
                    Events_Long_Desc = table.Column<string>(type: "TEXT", nullable: false),
                    Turns_Time = table.Column<int>(type: "INTEGER", nullable: false),
                    Decisions_Cost_Bits_Weight = table.Column<double>(type: "REAL", nullable: true),
                    Hardware_Cost_Bits_Weight = table.Column<double>(type: "REAL", nullable: true),
                    Software_Cost_Bits_Weight = table.Column<double>(type: "REAL", nullable: true),
                    Boosters_X = table.Column<double>(type: "REAL", nullable: true),
                    Boosters_Y = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gameevents", x => x.Games_Events_Id);
                    table.ForeignKey(
                        name: "FK_Gameevents_Decks_Decks_Id",
                        column: x => x.Decks_Id,
                        principalTable: "Decks",
                        principalColumn: "Decks_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Processes_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Processes_Desc = table.Column<string>(type: "TEXT", nullable: false),
                    Processes_Desc_Long = table.Column<string>(type: "TEXT", nullable: false),
                    Processes_Color = table.Column<string>(type: "TEXT", nullable: false),
                    Processes_Weight = table.Column<double>(type: "REAL", nullable: false),
                    Decks_Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Processes_Id);
                    table.ForeignKey(
                        name: "FK_Processes_Decks_Decks_Id",
                        column: x => x.Decks_Id,
                        principalTable: "Decks",
                        principalColumn: "Decks_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cardenablers",
                columns: table => new
                {
                    Cards_Enablers_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cards_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Enablers_Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardenablers", x => x.Cards_Enablers_Id);
                    table.ForeignKey(
                        name: "FK_Cardenablers_Cards_Cards_Id",
                        column: x => x.Cards_Id,
                        principalTable: "Cards",
                        principalColumn: "Cards_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Decisions",
                columns: table => new
                {
                    Decisions_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cards_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Decisions_Short_Desc = table.Column<string>(type: "TEXT", nullable: false),
                    Decisions_Long_Desc = table.Column<string>(type: "TEXT", nullable: false),
                    Decisions_Cost_Bits = table.Column<double>(type: "REAL", nullable: false),
                    Decisions_Cost_Bits_Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    Decks_Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decisions", x => x.Decisions_Id);
                    table.ForeignKey(
                        name: "FK_Decisions_Cards_Cards_Id",
                        column: x => x.Cards_Id,
                        principalTable: "Cards",
                        principalColumn: "Cards_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Decisions_Decks_Decks_Id",
                        column: x => x.Decks_Id,
                        principalTable: "Decks",
                        principalColumn: "Decks_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Feedbacks_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cards_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    Feedbacks_Long_Description = table.Column<string>(type: "TEXT", nullable: true),
                    Decks_Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Feedbacks_Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Cards_Cards_Id",
                        column: x => x.Cards_Id,
                        principalTable: "Cards",
                        principalColumn: "Cards_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Decks_Decks_Id",
                        column: x => x.Decks_Id,
                        principalTable: "Decks",
                        principalColumn: "Decks_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hardwares",
                columns: table => new
                {
                    Hardwares_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cards_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Hardwares_Short_Desc = table.Column<string>(type: "TEXT", nullable: false),
                    Hardwares_Long_Desc = table.Column<string>(type: "TEXT", nullable: false),
                    Hardwares_Cost_Bits = table.Column<double>(type: "REAL", nullable: false),
                    Hardwares_Cost_Bits_Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    Decks_Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardwares", x => x.Hardwares_Id);
                    table.ForeignKey(
                        name: "FK_Hardwares_Cards_Cards_Id",
                        column: x => x.Cards_Id,
                        principalTable: "Cards",
                        principalColumn: "Cards_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hardwares_Decks_Decks_Id",
                        column: x => x.Decks_Id,
                        principalTable: "Decks",
                        principalColumn: "Decks_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Softwares",
                columns: table => new
                {
                    Softwares_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cards_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Softwares_Short_Desc = table.Column<string>(type: "TEXT", nullable: false),
                    Softwares_Long_Desc = table.Column<string>(type: "TEXT", nullable: false),
                    Softwares_Cost_Bits = table.Column<double>(type: "REAL", nullable: false),
                    Softwares_Cost_Bits_Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    Decks_Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Softwares", x => x.Softwares_Id);
                    table.ForeignKey(
                        name: "FK_Softwares_Cards_Cards_Id",
                        column: x => x.Cards_Id,
                        principalTable: "Cards",
                        principalColumn: "Cards_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Softwares_Decks_Decks_Id",
                        column: x => x.Decks_Id,
                        principalTable: "Decks",
                        principalColumn: "Decks_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cardweights",
                columns: table => new
                {
                    Cards_Weights_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cards_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Processes_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Weights_X = table.Column<int>(type: "INTEGER", nullable: true),
                    Weights_Y = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardweights", x => x.Cards_Weights_Id);
                    table.ForeignKey(
                        name: "FK_Cardweights_Cards_Cards_Id",
                        column: x => x.Cards_Id,
                        principalTable: "Cards",
                        principalColumn: "Cards_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cardweights_Processes_Processes_Id",
                        column: x => x.Processes_Id,
                        principalTable: "Processes",
                        principalColumn: "Processes_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cardenablers_Cards_Id",
                table: "Cardenablers",
                column: "Cards_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Decks_Id",
                table: "Cards",
                column: "Decks_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Cardweights_Cards_Id",
                table: "Cardweights",
                column: "Cards_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Cardweights_Processes_Id",
                table: "Cardweights",
                column: "Processes_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_Cards_Id",
                table: "Decisions",
                column: "Cards_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_Decks_Id",
                table: "Decisions",
                column: "Decks_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_Users_Id",
                table: "Decks",
                column: "Users_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_Cards_Id",
                table: "Feedbacks",
                column: "Cards_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_Decks_Id",
                table: "Feedbacks",
                column: "Decks_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Gameevents_Decks_Id",
                table: "Gameevents",
                column: "Decks_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Hardwares_Cards_Id",
                table: "Hardwares",
                column: "Cards_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Hardwares_Decks_Id",
                table: "Hardwares",
                column: "Decks_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_Decks_Id",
                table: "Processes",
                column: "Decks_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_Cards_Id",
                table: "Softwares",
                column: "Cards_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_Decks_Id",
                table: "Softwares",
                column: "Decks_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cardenablers");

            migrationBuilder.DropTable(
                name: "Cardweights");

            migrationBuilder.DropTable(
                name: "Decisions");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Gameevents");

            migrationBuilder.DropTable(
                name: "GamemasterRequests");

            migrationBuilder.DropTable(
                name: "Hardwares");

            migrationBuilder.DropTable(
                name: "Softwares");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
