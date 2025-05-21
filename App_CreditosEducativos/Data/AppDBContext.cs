using System;
using System.Collections.Generic;
using App_CreditosEducativos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;

namespace App_CreditosEducativos.Data;

public partial class AppDBContext : DbContext
{
    public AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UsuariosModel> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=tcp:10.10.7.50\\MSSQLSERVER,1433;Database=CreditoE;persist security info=True;User ID=creditoeducati;Password=js84rx69;Trusted_Connection=False;MultipleActiveResultSets=true;Connection Timeout=30");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsuariosModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuarios__3213E83F7F60ED59");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
