using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuthModule.Migrations
{
    /// <inheritdoc />
    public partial class FirstTry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "Claim<IUser<int>>",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claim<IUser<int>>", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role<IUser<int>>",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role<IUser<int>>", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Claim<IUser<int>>Role<IUser<int>>",
                schema: "Auth",
                columns: table => new
                {
                    ClaimsId = table.Column<string>(type: "text", nullable: false),
                    RolesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claim<IUser<int>>Role<IUser<int>>", x => new { x.ClaimsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_Claim<IUser<int>>Role<IUser<int>>_Claim<IUser<int>>_ClaimsId",
                        column: x => x.ClaimsId,
                        principalSchema: "Auth",
                        principalTable: "Claim<IUser<int>>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Claim<IUser<int>>Role<IUser<int>>_Role<IUser<int>>_RolesId",
                        column: x => x.RolesId,
                        principalSchema: "Auth",
                        principalTable: "Role<IUser<int>>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Auth",
                columns: table => new
                {
                    ClaimsId = table.Column<string>(type: "text", nullable: false),
                    RolesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => new { x.ClaimsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_RoleClaims_Claims_ClaimsId",
                        column: x => x.ClaimsId,
                        principalSchema: "Auth",
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RolesId",
                        column: x => x.RolesId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Handle = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ClaimUserId = table.Column<string>(name: "Claim<User>Id", type: "text", nullable: true),
                    RoleUserId = table.Column<int>(name: "Role<User>Id", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Claims_Claim<User>Id",
                        column: x => x.ClaimUserId,
                        principalSchema: "Auth",
                        principalTable: "Claims",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Roles_Role<User>Id",
                        column: x => x.RoleUserId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "Auth",
                columns: table => new
                {
                    ClaimsId = table.Column<string>(type: "text", nullable: false),
                    UsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => new { x.ClaimsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserClaims_Claim<IUser<int>>_ClaimsId",
                        column: x => x.ClaimsId,
                        principalSchema: "Auth",
                        principalTable: "Claim<IUser<int>>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Auth",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Role<IUser<int>>_RolesId",
                        column: x => x.RolesId,
                        principalSchema: "Auth",
                        principalTable: "Role<IUser<int>>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claim<IUser<int>>Role<IUser<int>>_RolesId",
                schema: "Auth",
                table: "Claim<IUser<int>>Role<IUser<int>>",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RolesId",
                schema: "Auth",
                table: "RoleClaims",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UsersId",
                schema: "Auth",
                table: "UserClaims",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UsersId",
                schema: "Auth",
                table: "UserRoles",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Claim<User>Id",
                schema: "Auth",
                table: "Users",
                column: "Claim<User>Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role<User>Id",
                schema: "Auth",
                table: "Users",
                column: "Role<User>Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Claim<IUser<int>>Role<IUser<int>>",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Claim<IUser<int>>",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Role<IUser<int>>",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Claims",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Auth");
        }
    }
}
