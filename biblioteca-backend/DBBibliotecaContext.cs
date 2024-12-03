using biblioteca_backend.Models;
using biblioteca_backend.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace biblioteca_backend
{
    public class DBBibliotecaContext: DbContext
    {

        public DbSet<Libro> Libro {  get; set; }
        public DbSet<Genero> Genero { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Prestamo> Prestamo { get; set; }
        public DbSet<DetallePrestamoLibro> DetallePrestamoLibro { get; set; }
        public DbSet<Accion> Accion { get; set; }
        public DbSet<Archivo> Archivo { get; set; }
        public DbSet<RegistroArchivoPerfil> RegistroArchivoPerfil { get; set; }
        public DBBibliotecaContext(DbContextOptions<DBBibliotecaContext> options): base(options)
        {

        }

        //public IQueryable PaginarLibros(int pagina, int numeroRegistros, bool mostrarActivos)
        //{
        //    var paginaParameter = new SqlParameter("@pagina", pagina);
        //    var numeroRegistrosParameter = new SqlParameter("@numeroRegistros", numeroRegistros);
        //    var mostrarActivosParameter = new SqlParameter("@soloActivos", mostrarActivos);

        //    return Database.SqlQueryRaw<LibroGenero>("SELECT * FROM Paginar(@pagina, @numeroRegistros, @soloActivos)", paginaParameter,numeroRegistrosParameter, mostrarActivosParameter).AsQueryable();
        //}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Genero)
                .WithMany(g => g.Libros)
                .HasForeignKey(l => l.IdGenero);

            modelBuilder.Entity<Prestamo>()
                .HasOne(pr => pr.Perfil)
                .WithMany(p => p.Prestamos)
                .HasForeignKey(pr => pr.IdPerfil);

            modelBuilder.Entity<DetallePrestamoLibro>()
                .HasOne(d => d.Libro)
                .WithMany(l => l.DetallePrestamoLibro)
                .HasForeignKey(d => d.IdLibro);

            modelBuilder.Entity<DetallePrestamoLibro>()
                .HasOne(d => d.Prestamo)
                .WithMany(pr => pr.DetallePrestamoLibro)
                .HasForeignKey(d => d.IdPrestamo);

            modelBuilder.Entity<Perfil>()
                .HasOne(p => p.Rol)
                .WithMany(r => r.Perfiles)
                .HasForeignKey(p => p.IdRol);

            modelBuilder.Entity<RegistroArchivoPerfil>()
                .HasOne(R => R.Accion)
                .WithMany(A => A.RegistroArchivosPerfil)
                .HasForeignKey(R => R.IdAccion);

            modelBuilder.Entity<RegistroArchivoPerfil>()
                .HasOne(R => R.Archivo)
                .WithMany(A => A.RegistroArchivosPerfil)
                .HasForeignKey(R => R.IdArchivo);

            modelBuilder.Entity<RegistroArchivoPerfil>()
                .HasOne(R => R.Perfil)
                .WithMany(P => P.RegistroArchivosPerfil)
                .HasForeignKey(R => R.IdPerfil);
        }
    }
}
