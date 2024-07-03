﻿// <auto-generated />
using System;
using FunctionLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FunctionLibrary.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240703082141_se")]
    partial class se
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FunctionLibrary.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductTypeID")
                        .HasColumnType("int");

                    b.HasKey("ProductID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("FunctionLibrary.Models.Transaction", b =>
                {
                    b.Property<string>("TransactionID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CVR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DigitalSignature")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentMethodID")
                        .HasColumnType("int");

                    b.Property<int>("SalespersonID")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("SystemSerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionID");

                    b.ToTable("Transaction_");
                });

            modelBuilder.Entity("FunctionLibrary.Models.TransactionDetail", b =>
                {
                    b.Property<int>("TransactionDetailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionDetailID"));

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("TransactionID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionDetailID");

                    b.ToTable("TransactionDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
