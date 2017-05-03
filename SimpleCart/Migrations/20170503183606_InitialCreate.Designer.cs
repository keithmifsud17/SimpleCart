﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SimpleCart.Context;

namespace SimpleCart.Migrations
{
    [DbContext(typeof(SimpleCartContext))]
    [Migration("20170503183606_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("SimpleCart.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<decimal>("Price");

                    b.HasKey("ID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SimpleCart.Models.ShoppingCart", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ProductID");

                    b.Property<int>("Quantity");

                    b.Property<int>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("SimpleCart.Models.ShoppingCart", b =>
                {
                    b.HasOne("SimpleCart.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");
                });
        }
    }
}
