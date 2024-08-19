﻿// <auto-generated />
using System;
using GerenciadorProcessos.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GerenciadorProcessos.Infra.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240819072307_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GerenciadorProcessos.Domain.Entidades.Area", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Areas", (string)null);
                });

            modelBuilder.Entity("GerenciadorProcessos.Domain.Entidades.Processo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AreaId")
                        .HasColumnType("uuid");

                    b.Property<string>("AreaResponsavel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataUltimaAlteracao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Ferramenta")
                        .HasColumnType("text");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ProcessoPaiId")
                        .HasColumnType("uuid");

                    b.Property<int>("Tipo")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.HasIndex("ProcessoPaiId");

                    b.ToTable("Processos", (string)null);
                });

            modelBuilder.Entity("GerenciadorProcessos.Domain.Entidades.Processo", b =>
                {
                    b.HasOne("GerenciadorProcessos.Domain.Entidades.Area", "Area")
                        .WithMany("Processos")
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GerenciadorProcessos.Domain.Entidades.Processo", "ProcessoPai")
                        .WithMany("Subprocessos")
                        .HasForeignKey("ProcessoPaiId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Area");

                    b.Navigation("ProcessoPai");
                });

            modelBuilder.Entity("GerenciadorProcessos.Domain.Entidades.Area", b =>
                {
                    b.Navigation("Processos");
                });

            modelBuilder.Entity("GerenciadorProcessos.Domain.Entidades.Processo", b =>
                {
                    b.Navigation("Subprocessos");
                });
#pragma warning restore 612, 618
        }
    }
}
