﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ReadersEdition.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Definition", b =>
                {
                    b.Property<Guid>("DefinitionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gloss")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GlossLanguageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("WordLanguageId")
                        .HasColumnType("TEXT");

                    b.HasKey("DefinitionId");

                    b.ToTable("Definitions");
                });

            modelBuilder.Entity("ReadersEdition.Domain.DictionaryModels.Language", b =>
                {
                    b.Property<Guid>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("LanguageCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LanguageName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LanguageId");

                    b.ToTable("Languages");
                });
#pragma warning restore 612, 618
        }
    }
}
