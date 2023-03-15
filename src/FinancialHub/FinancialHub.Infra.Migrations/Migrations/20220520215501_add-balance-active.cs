﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialHub.Infra.Migrations.Migrations
{
    public partial class addbalanceactive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "balances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                table: "balances");
        }
    }
}
