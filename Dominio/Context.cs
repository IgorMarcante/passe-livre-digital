using Dominio.Models;
using Microsoft.EntityFrameworkCore;

namespace Dominio
{
    public class Context: DbContext
    {
         public Context(DbContextOptions<Context> options) : base(options)  
        {  
  
        } 
         public DbSet<Anexo> Anexos { get; set; }
         public DbSet<Usuario> Usuarios { get; set; }
    }
}