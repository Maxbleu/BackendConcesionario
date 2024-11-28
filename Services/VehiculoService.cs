using Backend_Concesionario.Data;
using Backend_Concesionario.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Backend_Concesionario.Services
{
    public class VehiculoService : IHostedService
    {
        private readonly AplicationDBContext _context;

        public VehiculoService(AplicationDBContext context)
        {
            _context = context;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.CargarVehiculosAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task CargarVehiculosAsync()
        {
            List<Vehiculo> vehiculos = CSVDataReader.LeerCSVVehiculos();

            DatabaseFacade db = _context.Database;
            await db.ExecuteSqlRawAsync("TRUNCATE TABLE Vehiculos");

            await _context.Vehiculos.AddRangeAsync(vehiculos);
            await _context.SaveChangesAsync();
        }
    }
}
