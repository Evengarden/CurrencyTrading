﻿// <auto-generated />
using System;
using CurrencyTrading.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CurrencyTrading.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230413104446_AutomatchingFieldForLot")]
    partial class AutomatchingFieldForLot
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CurrencyTrading.Models.Balance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "Currency")
                        .IsUnique();

                    b.ToTable("Balances");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 10000m,
                            Currency = "RUB",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Amount = 10000m,
                            Currency = "RUB",
                            UserId = 2
                        });
                });

            modelBuilder.Entity("CurrencyTrading.Models.Lot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Automatch")
                        .HasColumnType("integer");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("CurrencyAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Lots");
                });

            modelBuilder.Entity("CurrencyTrading.Models.Trade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BuyerId")
                        .HasColumnType("integer");

                    b.Property<int>("LotId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TradeDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("LotId")
                        .IsUnique();

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("CurrencyTrading.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Login = "test1",
                            Password = "ALTxFXCYscS7WroaHNKwIt8tyk+hEDnCTFhEFQOW8+KeLoQMsmntAl533XATDazh2g=="
                        },
                        new
                        {
                            Id = 2,
                            Login = "test2",
                            Password = "ACw7NtPp0etOyUG3izqMPd2Y9mnXP0K/y1Q/XVP0vRBuqSIzk5ebLmbW31c3Crj44g=="
                        });
                });

            modelBuilder.Entity("CurrencyTrading.Models.Balance", b =>
                {
                    b.HasOne("CurrencyTrading.Models.User", "User")
                        .WithMany("Balance")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CurrencyTrading.Models.Lot", b =>
                {
                    b.HasOne("CurrencyTrading.Models.User", "Owner")
                        .WithMany("Lots")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("CurrencyTrading.Models.Trade", b =>
                {
                    b.HasOne("CurrencyTrading.Models.User", "Buyer")
                        .WithMany("Trades")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CurrencyTrading.Models.Lot", "TradeLot")
                        .WithOne("Trade")
                        .HasForeignKey("CurrencyTrading.Models.Trade", "LotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Buyer");

                    b.Navigation("TradeLot");
                });

            modelBuilder.Entity("CurrencyTrading.Models.Lot", b =>
                {
                    b.Navigation("Trade");
                });

            modelBuilder.Entity("CurrencyTrading.Models.User", b =>
                {
                    b.Navigation("Balance");

                    b.Navigation("Lots");

                    b.Navigation("Trades");
                });
#pragma warning restore 612, 618
        }
    }
}
