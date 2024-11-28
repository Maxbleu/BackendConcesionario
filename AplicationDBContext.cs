using Backend_Concesionario.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_Concesionario
{
    public class AplicationDBContext : DbContext
    {
        public DbSet<Vehiculo> Vehiculos { get; set; }
        private string _connectionString;

        public AplicationDBContext(IConfiguration configuration) 
        {
            this._connectionString = configuration["ConnectionStrings:Heidi"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(this._connectionString, ServerVersion.AutoDetect(this._connectionString));
        }
    }
}
