using System;
using System.Collections.Generic;
using APBD_CW_5.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_CW_5.Data;

public partial class S30756Context : DbContext
{
    public S30756Context()
    {
    }

    public S30756Context(DbContextOptions<S30756Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTrip> ClientTrips { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=db-mssql;Database=s30756;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("Client_pk");

            entity.ToTable("Client");

            entity.Property(e => e.IdClient).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.FirstName).HasMaxLength(120);
            entity.Property(e => e.LastName).HasMaxLength(120);
            entity.Property(e => e.Pesel).HasMaxLength(120);
            entity.Property(e => e.Telephone).HasMaxLength(120);
        });

        modelBuilder.Entity<ClientTrip>(entity =>
        {
            entity.HasKey(e => new { e.ClientIdClient, e.TripIdTrip }).HasName("Client_Trip_pk");

            entity.ToTable("Client_Trip");

            entity.Property(e => e.ClientIdClient).HasColumnName("Client_IdClient");
            entity.Property(e => e.TripIdTrip).HasColumnName("Trip_IdTrip");

            entity.HasOne(d => d.ClientIdClientNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.ClientIdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Client_Trip_Client");

            entity.HasOne(d => d.TripIdTripNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.TripIdTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Client_Trip_Trip");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("Country_pk");

            entity.ToTable("Country");

            entity.Property(e => e.IdCountry).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(120);

            entity.HasMany(d => d.TripIdTrips).WithMany(p => p.CountryIdCountries)
                .UsingEntity<Dictionary<string, object>>(
                    "CountryTrip",
                    r => r.HasOne<Trip>().WithMany()
                        .HasForeignKey("TripIdTrip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Trip"),
                    l => l.HasOne<Country>().WithMany()
                        .HasForeignKey("CountryIdCountry")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Country"),
                    j =>
                    {
                        j.HasKey("CountryIdCountry", "TripIdTrip").HasName("Country_Trip_pk");
                        j.ToTable("Country_Trip");
                        j.IndexerProperty<int>("CountryIdCountry").HasColumnName("Country_IdCountry");
                        j.IndexerProperty<int>("TripIdTrip").HasColumnName("Trip_IdTrip");
                    });
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.IdTrip).HasName("Trip_pk");

            entity.ToTable("Trip");

            entity.Property(e => e.IdTrip).ValueGeneratedNever();
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DateTo).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(220);
            entity.Property(e => e.Name).HasMaxLength(120);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
