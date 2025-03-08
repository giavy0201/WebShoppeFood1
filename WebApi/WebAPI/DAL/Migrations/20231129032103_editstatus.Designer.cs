﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231129032103_editstatus")]
    partial class editstatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entities.AccountStore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AccountStoreID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("LoginName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("LoginName");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Password");

                    b.Property<int>("StoreID")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("UserName");

                    b.HasKey("Id");

                    b.HasIndex("StoreID");

                    b.ToTable("AccountStores");
                });

            modelBuilder.Entity("DAL.Entities.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CartID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Delivery")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("DeliveryAddress");

                    b.Property<int?>("PhoneNumber")
                        .HasColumnType("int")
                        .HasColumnName("Phone");

                    b.Property<double?>("Status")
                        .HasColumnType("float");

                    b.Property<int>("StoreId")
                        .HasColumnType("int")
                        .HasColumnName("StoreID");

                    b.Property<DateTime?>("TimeOrder")
                        .HasColumnType("datetime2")
                        .HasColumnName("TimeOrder");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("DAL.Entities.CategoryProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CategoryID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AddorUpdateBy");

                    b.Property<DateTime?>("AdminTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("AddorUpdateAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("CategoryName");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CategoryProducts");
                });

            modelBuilder.Entity("DAL.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CityID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AddorUpdateBy");

                    b.Property<DateTime?>("AdminTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("AddorUpdateAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("CityName");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("DAL.Entities.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AddorUpdateBy");

                    b.Property<DateTime?>("AdminTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("AddorUpdateAt");

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<int>("CityID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CollectionDes");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CollectionImg");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("CollectionName");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryID");

                    b.HasIndex("CityID");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("DAL.Entities.CollectionStore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CollectionStoreID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AddorUpdateBy");

                    b.Property<DateTime?>("AdminTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("AddorUpdateAt");

                    b.Property<int?>("CollectionID")
                        .HasColumnType("int");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("StoreID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CollectionID");

                    b.HasIndex("StoreID");

                    b.ToTable("CollectionStores");
                });

            modelBuilder.Entity("DAL.Entities.ContentProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ContentID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AddorUpdateBy");

                    b.Property<DateTime?>("AdminTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("AddorUpdateAt");

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("ContentName");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryID");

                    b.ToTable("ContentProducts");
                });

            modelBuilder.Entity("DAL.Entities.Customer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("CodeForgotPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("DAL.Entities.DetailCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DetailCartID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CartID")
                        .HasColumnType("int");

                    b.Property<int>("FoodId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float")
                        .HasColumnName("PriceFood");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("CartID");

                    b.HasIndex("FoodId");

                    b.ToTable("DetailCarts");
                });

            modelBuilder.Entity("DAL.Entities.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DistrictID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AddorUpdateBy");

                    b.Property<DateTime?>("AdminTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("AddorUpdateAt");

                    b.Property<int>("CityID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("DistrictName");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CityID");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("DAL.Entities.Food", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AddorUpdateBy");

                    b.Property<DateTime?>("AdminTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("AddorUpdateAt");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("DescriptionFood");

                    b.Property<int?>("Discount")
                        .HasColumnType("int")
                        .HasColumnName("DiscountFood");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("imgFood");

                    b.Property<int>("MenuID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("NameFood");

                    b.Property<double>("Price")
                        .HasColumnType("float")
                        .HasColumnName("PriceFood");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MenuID");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("DAL.Entities.ListMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AddorUpdateBy");

                    b.Property<DateTime?>("AdminTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("AddorUpdateAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("MenuName");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<int>("StoreID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoreID");

                    b.ToTable("ListMenus");
                });

            modelBuilder.Entity("DAL.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("StoreID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("StoreAddress");

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AddorUpdateBy");

                    b.Property<DateTime?>("AdminTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("AddorUpdateAt");

                    b.Property<int>("ContentID")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("StoreImage");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("StoreName");

                    b.Property<string>("Preferential")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PreferentialStore");

                    b.Property<double>("StarEvaluate")
                        .HasColumnType("float")
                        .HasColumnName("StarEvaluateStore");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("TimeClose")
                        .HasColumnType("time")
                        .HasColumnName("TimeClose");

                    b.Property<TimeSpan>("TimeOpen")
                        .HasColumnType("time")
                        .HasColumnName("TimeOpen");

                    b.Property<int>("WardID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContentID");

                    b.HasIndex("WardID");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("DAL.Entities.Ward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("WardID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AddorUpdateBy");

                    b.Property<DateTime?>("AdminTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("AddorUpdateAt");

                    b.Property<int>("DistrictID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("WardName");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DistrictID");

                    b.ToTable("Wards");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "39d1b2d5-39fc-4ae7-8093-5e3466dfaccb",
                            ConcurrencyStamp = "1",
                            Name = "Admin",
                            NormalizedName = "Admin"
                        },
                        new
                        {
                            Id = "d5ae2b5b-3930-43c9-a3bb-94b8a995812d",
                            ConcurrencyStamp = "2",
                            Name = "Customer",
                            NormalizedName = "Customer"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DAL.Entities.AccountStore", b =>
                {
                    b.HasOne("DAL.Entities.Store", "Store")
                        .WithMany("AccountStores")
                        .HasForeignKey("StoreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("DAL.Entities.Cart", b =>
                {
                    b.HasOne("DAL.Entities.Customer", "Customer")
                        .WithMany("Carts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("DAL.Entities.Collection", b =>
                {
                    b.HasOne("DAL.Entities.CategoryProduct", "CategoryProduct")
                        .WithMany("Collections")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.City", "City")
                        .WithMany("Collections")
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryProduct");

                    b.Navigation("City");
                });

            modelBuilder.Entity("DAL.Entities.CollectionStore", b =>
                {
                    b.HasOne("DAL.Entities.Collection", "Collection")
                        .WithMany("collectionStores")
                        .HasForeignKey("CollectionID");

                    b.HasOne("DAL.Entities.Store", "Store")
                        .WithMany("CollectionStores")
                        .HasForeignKey("StoreID");

                    b.Navigation("Collection");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("DAL.Entities.ContentProduct", b =>
                {
                    b.HasOne("DAL.Entities.CategoryProduct", "CategoryProduct")
                        .WithMany("ContentProducts")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryProduct");
                });

            modelBuilder.Entity("DAL.Entities.DetailCart", b =>
                {
                    b.HasOne("DAL.Entities.Cart", "Cart")
                        .WithMany("DetailCarts")
                        .HasForeignKey("CartID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Food", "Food")
                        .WithMany("DetailCarts")
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Food");
                });

            modelBuilder.Entity("DAL.Entities.District", b =>
                {
                    b.HasOne("DAL.Entities.City", "City")
                        .WithMany("Districts")
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("DAL.Entities.Food", b =>
                {
                    b.HasOne("DAL.Entities.ListMenu", "ListMenu")
                        .WithMany("Foods")
                        .HasForeignKey("MenuID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ListMenu");
                });

            modelBuilder.Entity("DAL.Entities.ListMenu", b =>
                {
                    b.HasOne("DAL.Entities.Store", "Store")
                        .WithMany("ListMenu")
                        .HasForeignKey("StoreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("DAL.Entities.Store", b =>
                {
                    b.HasOne("DAL.Entities.ContentProduct", "ContentProduct")
                        .WithMany("Stores")
                        .HasForeignKey("ContentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Ward", "Ward")
                        .WithMany("Stores")
                        .HasForeignKey("WardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContentProduct");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("DAL.Entities.Ward", b =>
                {
                    b.HasOne("DAL.Entities.District", "District")
                        .WithMany("Ward")
                        .HasForeignKey("DistrictID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DAL.Entities.Customer", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DAL.Entities.Customer", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Customer", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DAL.Entities.Customer", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DAL.Entities.Cart", b =>
                {
                    b.Navigation("DetailCarts");
                });

            modelBuilder.Entity("DAL.Entities.CategoryProduct", b =>
                {
                    b.Navigation("Collections");

                    b.Navigation("ContentProducts");
                });

            modelBuilder.Entity("DAL.Entities.City", b =>
                {
                    b.Navigation("Collections");

                    b.Navigation("Districts");
                });

            modelBuilder.Entity("DAL.Entities.Collection", b =>
                {
                    b.Navigation("collectionStores");
                });

            modelBuilder.Entity("DAL.Entities.ContentProduct", b =>
                {
                    b.Navigation("Stores");
                });

            modelBuilder.Entity("DAL.Entities.Customer", b =>
                {
                    b.Navigation("Carts");
                });

            modelBuilder.Entity("DAL.Entities.District", b =>
                {
                    b.Navigation("Ward");
                });

            modelBuilder.Entity("DAL.Entities.Food", b =>
                {
                    b.Navigation("DetailCarts");
                });

            modelBuilder.Entity("DAL.Entities.ListMenu", b =>
                {
                    b.Navigation("Foods");
                });

            modelBuilder.Entity("DAL.Entities.Store", b =>
                {
                    b.Navigation("AccountStores");

                    b.Navigation("CollectionStores");

                    b.Navigation("ListMenu");
                });

            modelBuilder.Entity("DAL.Entities.Ward", b =>
                {
                    b.Navigation("Stores");
                });
#pragma warning restore 612, 618
        }
    }
}
