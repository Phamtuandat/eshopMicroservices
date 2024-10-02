﻿// <auto-generated />
using System;
using Discount.GRPC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Discount.GRPC.Migrations
{
    [DbContext(typeof(DiscountContext))]
    [Migration("20240607112617_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("Discount.GRPC.Models.Coupon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DiscountPercentage")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Coupons");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9529c72c-ffcb-46a6-a58e-ab279114a17c"),
                            Code = "NEWBIE24",
                            Description = "Newbie Discount",
                            DiscountPercentage = 15,
                            Quantity = 100
                        },
                        new
                        {
                            Id = new Guid("09fd4c2f-ff4c-4dc5-81ec-67747d3207cd"),
                            Code = "FREESHIP05",
                            Description = "Freeship Discount",
                            DiscountPercentage = 10,
                            Quantity = 100
                        });
                });
#pragma warning restore 612, 618
        }
    }
}