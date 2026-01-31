using System;
using System.Collections.Generic;
using JournalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JournalApi.Data;

public partial class JournalDbContext : DbContext
{
    public JournalDbContext()
    {
    }

    public JournalDbContext(DbContextOptions<JournalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<JournalEntry> JournalEntries { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__journalE__3213E83FFAD8AFAB");

            entity.ToTable("journalEntry");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.User).WithMany(p => p.JournalEntries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_JournalEntry_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FD115B033");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC57213029D9D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
