using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dinosauria.Domain.Models;

namespace Dinosauria.Infrastructure.Data;

/// <summary>
/// EF-Core-Datenbankkontext für die Domänenobjekte von Dinosauria.
/// Neben den DbSets enthält er die Schlüssel-, Vererbungs- und Beziehungsdefinitionen.
/// </summary>
public class DinosauriaDbContext(DbContextOptions<DinosauriaDbContext> options)
    : DbContext(options)
{
    public DbSet<Spieler> Spieler => Set<Spieler>();
    public DbSet<Spiel> Spiele => Set<Spiel>();
    public DbSet<Spielziel> Spielziele => Set<Spielziel>();
    public DbSet<Ressource> Ressourcen => Set<Ressource>();
    public DbSet<SpielRessource> SpielRessourcen => Set<SpielRessource>();
    public DbSet<Dinosaurierart> Dinosaurierarten => Set<Dinosaurierart>();
    public DbSet<SpielDinosaurier> SpielDinosaurier => Set<SpielDinosaurier>();
    public DbSet<Karte> Karten => Set<Karte>();
    public DbSet<SpielKarte> SpielKarten => Set<SpielKarte>();
    public DbSet<Aufgabenbedingung> Aufgabenbedingungen => Set<Aufgabenbedingung>();
    public DbSet<RessourceProduzieren> RessourcenProduzieren => Set<RessourceProduzieren>();
    public DbSet<DinosaurierKaufen> DinosaurierKaufen => Set<DinosaurierKaufen>();
    public DbSet<Spielbedingung> Spielbedingungen => Set<Spielbedingung>();
    public DbSet<Dinosaurierbedingung> Dinosaurierbedingungen => Set<Dinosaurierbedingung>();
    public DbSet<Kartenbedingung> Kartenbedingungen => Set<Kartenbedingung>();
    public DbSet<SystemKonfiguration> SystemKonfigurationen => Set<SystemKonfiguration>();
    /// <summary>Konfiguriert die SQL-Server-Verbindung für den Kontext.</summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(DatabaseConnection.ConnectionString);

    /// <summary>
    /// Definiert Tabellen, Schlüssel, Enum-Konvertierungen, zusammengesetzte Schlüssel,
    /// Ressourcebeziehungen und die TPH-Vererbung der Aufgabenbedingungen.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Spieler>(e =>
        {
            e.ToTable("Spieler");
            e.HasKey(s => s.SpielerID);
            e.Property(s => s.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Spiel>(e =>
        {
            e.ToTable("Spiel");
            e.HasKey(s => s.SpielID);

            e.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<Ressource>(e =>
        {
            e.ToTable("Ressource");
            e.HasKey(r => r.RessourceID);
            e.Property(r => r.Name).HasMaxLength(50);
            e.Property(r => r.Einheit).HasMaxLength(20);
        });

        modelBuilder.Entity<Dinosaurierart>(e =>
        {
            e.ToTable("Dinosaurierart");
            e.HasKey(d => d.DinosaurierartID);

            e.Property(d => d.Name).HasMaxLength(100);
            e.Property(d => d.Beschreibung).HasMaxLength(500);


            e.Property(d => d.Erdzeitalter).HasConversion<string>().HasMaxLength(30);
            e.Property(d => d.Ernaehrung).HasConversion<string>().HasMaxLength(30);
            e.Property(d => d.Lebensraum).HasConversion<string>().HasMaxLength(30);

            e.HasOne(d => d.KostenRessource)
    .WithMany()
    .HasForeignKey(d => d.KostenRessourceID)
    .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(d => d.Produktionsressource)
                .WithMany()
                .HasForeignKey(d => d.ProduktionsressourceID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Karte>(e =>
        {
            e.ToTable("Karte");
            e.HasKey(k => k.KarteID);

            e.Property(k => k.Name).HasMaxLength(100);
            e.Property(k => k.Beschreibung).HasMaxLength(500);
        });

        modelBuilder.Entity<Spielziel>(e =>
        {
            e.ToTable("Spielziel");
            e.HasKey(s => s.SpielzielID);
            e.Property(s => s.Name).HasMaxLength(100);
            e.Property(s => s.Beschreibung).HasMaxLength(500);
        });

        modelBuilder.Entity<SystemKonfiguration>(e =>
        {
            e.ToTable("SystemKonfiguration");
            e.HasKey(s => s.ID);

            e.HasOne(s => s.Spielziel)
                .WithMany()
                .HasForeignKey(s => s.SpielzielID)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(s => s.Ressource)
                .WithMany()
                .HasForeignKey(s => s.RessourceID)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(s => s.ErsterDinosaurier)
                .WithMany()
                .HasForeignKey(s => s.DinosaurierartID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<SpielDinosaurier>()
            .HasKey(sd => new { sd.SpielID, sd.DinosaurierartID });

        modelBuilder.Entity<SpielKarte>()
            .HasKey(sk => new { sk.SpielID, sk.KarteID });

        modelBuilder.Entity<SpielRessource>()
            .HasKey(sr => new { sr.SpielID, sr.RessourceID });

        modelBuilder.Entity<Aufgabenbedingung>()
            .HasDiscriminator<string>("Bedingungstyp")
            .HasValue<RessourceProduzieren>("RessourceProduzieren")
            .HasValue<DinosaurierKaufen>("DinosaurierKaufen");

        modelBuilder.Entity<Spielbedingung>(e =>
        {
            e.HasKey(sb => new { sb.SpielzielID, sb.AufgabenbedingungID });

            e.HasOne(sb => sb.Spielziel)
                .WithMany(s => s.Bedingungen)
                .HasForeignKey(sb => sb.SpielzielID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Dinosaurierbedingung>(e =>
        {
            e.HasKey(db => new { db.DinosaurierartID, db.AufgabenbedingungID });

            e.HasOne(db => db.Dinosaurierart)
                .WithMany(d => d.Bedingungen)
                .HasForeignKey(db => db.DinosaurierartID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Kartenbedingung>(e =>
        {
            e.HasKey(kb => new { kb.KarteID, kb.AufgabenbedingungID });

            e.HasOne(kb => kb.Karte)
                .WithMany(k => k.Bedingungen)
                .HasForeignKey(kb => kb.KarteID)
                .OnDelete(DeleteBehavior.NoAction);
        });


    }
    
}

/// <summary>Erzeugt einen konfigurierten Datenbankkontext für Infrastrukturzugriffe.</summary>
public static class DinosauriaDbContextFactory
{
    /// <summary>Erstellt einen Kontext mit den SQL-Server-Optionen der Anwendung.</summary>
    public static DinosauriaDbContext Create()
    {
        var options = new DbContextOptionsBuilder<DinosauriaDbContext>()
            .UseSqlServer(DatabaseConnection.ConnectionString)
            .Options;

        return new DinosauriaDbContext(options);
    }
}