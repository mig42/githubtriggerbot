﻿// <auto-generated />
using githubtriggerbot.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace githubtriggerbot.Migrations.RepositoriesDb
{
    [DbContext(typeof(RepositoriesDbContext))]
    partial class RepositoriesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("githubtriggerbot.Data.Repositories.Hook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HookUri")
                        .IsRequired();

                    b.Property<string>("Provider")
                        .IsRequired();

                    b.Property<int>("RepositoryId");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RepositoryId");

                    b.ToTable("hook");
                });

            modelBuilder.Entity("githubtriggerbot.Data.Repositories.Repository", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("OwnerId")
                        .IsRequired();

                    b.Property<string>("Provider")
                        .IsRequired();

                    b.Property<int>("ProviderRepoId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("repository");
                });

            modelBuilder.Entity("githubtriggerbot.Data.Users.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUser");
                });

            modelBuilder.Entity("githubtriggerbot.Data.Repositories.Hook", b =>
                {
                    b.HasOne("githubtriggerbot.Data.Repositories.Repository", "Repository")
                        .WithMany()
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("githubtriggerbot.Data.Repositories.Repository", b =>
                {
                    b.HasOne("githubtriggerbot.Data.Users.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
