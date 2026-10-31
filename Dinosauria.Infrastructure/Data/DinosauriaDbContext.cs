using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dinosauria.Domain.Models;

namespace Dinosauria.Infrastructure.Data;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(DatabaseConnection.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
        modelBuilder.Entity<Spielbedingung>()
            .HasKey(sb => new { sb.SpielzielID, sb.AufgabenbedingungID });

        modelBuilder.Entity<Dinosaurierbedingung>()
            .HasKey(db => new { db.DinosaurierartID, db.AufgabenbedingungID });

        modelBuilder.Entity<Kartenbedingung>()
            .HasKey(kb => new { kb.KarteID, kb.AufgabenbedingungID });
        
        modelBuilder.Entity<Dinosaurierart>()
            .HasOne(d => d.KostenRessource)
            .WithMany()
            .HasForeignKey(d => d.KostenRessourceID);

        modelBuilder.Entity<Dinosaurierart>()
            .HasOne(d => d.Produktionsressource)
            .WithMany()
            .HasForeignKey(d => d.ProduktionsressourceID);
    }
}
