using Backend_Concesionario.Data;
using Backend_Concesionario.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Backend_Concesionario.Services
{
    //  Esta clase se encarga de cargar los datos de los vehículos
    //  desde un archivo CSV a la base de datos, al iniciar la aplicación.
    //  Ya que, cuenta con la interfaz IHostedService, la cual permite
    //  ejecutar código al iniciar la aplicación.
    public class VehiculoService : IHostedService
    {
        private readonly AplicationDBContext _context;

        public VehiculoService(AplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Este método se encarga de cargar los vehículos
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.CargarVehiculosAsync();
        }

        /// <summary>
        /// Este método se encarga de detener el servicio
        /// cuando se ha completado la tarea
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        /// <summary>
        /// Este método se encarga de cargar los vehículos
        /// del fichero CSV a la base de datos
        /// </summary>
        /// <returns></returns>
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
