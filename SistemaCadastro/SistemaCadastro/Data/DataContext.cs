using Microsoft.EntityFrameworkCore;
using SistemaCadastro.Models;

namespace SistemaCadastro.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<ContatoModel> Contatos { get; set; }
    }
}
