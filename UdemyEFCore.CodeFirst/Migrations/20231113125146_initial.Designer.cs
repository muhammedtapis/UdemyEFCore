﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UdemyEFCore.CodeFirst.DataAccessLayer;

#nullable disable

namespace UdemyEFCore.CodeFirst.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231113125146_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.BasePerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BasePerson");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Barcode")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<decimal>("DiscountPrice")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Name");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.ProductFeature", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ProductFeatures");
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.ProductFull", b =>
                {
                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Product_Id")
                        .HasColumnType("int");

                    b.ToTable("ProductFulls");
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.Employee", b =>
                {
                    b.HasBaseType("UdemyEFCore.CodeFirst.DataAccessLayer.BasePerson");

                    b.Property<decimal>("Salary")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasDiscriminator().HasValue("Employee");
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.Manager", b =>
                {
                    b.HasBaseType("UdemyEFCore.CodeFirst.DataAccessLayer.BasePerson");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Manager");
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.Product", b =>
                {
                    b.HasOne("UdemyEFCore.CodeFirst.DataAccessLayer.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.ProductFeature", b =>
                {
                    b.HasOne("UdemyEFCore.CodeFirst.DataAccessLayer.Product", "Product")
                        .WithOne("ProductFeature")
                        .HasForeignKey("UdemyEFCore.CodeFirst.DataAccessLayer.ProductFeature", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("UdemyEFCore.CodeFirst.DataAccessLayer.Product", b =>
                {
                    b.Navigation("ProductFeature")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
