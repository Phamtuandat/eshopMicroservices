﻿// <auto-generated />
using System;
using Discount.GRPC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Discount.GRPC.Migrations
{
    [DbContext(typeof(DiscountContext))]
    partial class DiscountContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                            Id = new Guid("0fd362d0-04db-4d07-87b1-041ff2ffb283"),
                            Code = "NEWBIE24",
                            Description = "Newbie Discount",
                            DiscountPercentage = 15,
                            Quantity = 100
                        },
                        new
                        {
                            Id = new Guid("a5e8e538-fa7f-4774-be47-220a336e2e75"),
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
