using Backend_Concesionario.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace Backend_Concesionario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly AplicationDBContext _dBContext;

        public VehiculosController(AplicationDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        /// <summary>
        /// Este método devuelve todos los vehículos 
        /// de la base de datos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this._dBContext.Vehiculos);
        }

        /// <summary>
        /// Este método devuelve un vehículo en concreto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            //  Obtenemos un vehículo por su id
            Vehiculo? vehiculo = await this._dBContext.Vehiculos.FirstOrDefaultAsync(vehiculo => vehiculo.Id == id);

            //  Si no existe el vehículo, devolvemos un error 404
            if (vehiculo == null)
                return NotFound();

            //  Si existe el vehículo, lo devolvemos
            return Ok(vehiculo);
        }

        /// <summary>
        /// Este método añade un vehículo a la base de datos
        /// </summary>
        /// <param name="vehiculo"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] Vehiculo vehiculo)
        {
            //  Añadimos un id al vehículo
            vehiculo.Id = this._dBContext.Vehiculos.Count() + 1;

            //  Añadimos el vehículo a la base de datos
            await this._dBContext.Vehiculos.AddAsync(vehiculo);

            //  Guardamos los cambios   
            await this._dBContext.SaveChangesAsync();

            //  Devolvemos el vehículo añadido
            return CreatedAtAction(nameof(Get), vehiculo);
        }

        /// <summary>
        /// Este método se encarga de actualizar los vehiculos
        /// a través del id del coche que recibe y los campos
        /// que se desea actualizar a partir de un diccionario de las
        /// propiedades con sus valores a actualizar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="datosVehiculoActualizado"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Dictionary<string, JsonElement> datosVehiculoActualizado)
        {
            //  Obtenemos el vehiculo
            Vehiculo? vehiculo = await this._dBContext.Vehiculos.FirstOrDefaultAsync(vehiculo => vehiculo.Id == id);

            //  Comprobamos que no se ha encontrado el vehiculo búscado
            if (vehiculo == null)
            {

                //  Notificamos que no hemos encontrado ningún registro con ese id
                return NotFound();
            }

            //  Por cada propiedad dentro del diccionario se 
            //  actualizará en el vehiculo seleccinado
            foreach (string clave in datosVehiculoActualizado.Keys)
            {
                PropertyInfo propiedad = typeof(Vehiculo).GetProperty(clave);
                JsonElement jsonElement = datosVehiculoActualizado[clave];
                object value = jsonElement.ValueKind == JsonValueKind.String ? jsonElement.GetString() : jsonElement.GetInt32();
                propiedad.SetValue(vehiculo, value);
            }

            await this._dBContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), vehiculo);
        }

        /// <summary>
        /// Este método elimina un vehículo a través
        /// del id que recibe como parámetro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            //  Obtenemos el vehiculo
            Vehiculo? vehiculo = await this._dBContext.Vehiculos.FirstOrDefaultAsync(vehiculo => vehiculo.Id == id);
            
            //  Comprobamos que no se ha encontrado un vehiculo
            if (vehiculo == null)
            {
                //  Notificamos que no hemos encontrado ningún registro con ese id
                return NotFound();
            }

            //  Eliminamos el vehiculo
            this._dBContext.Remove(vehiculo);

            //  Guardamos los cambios
            await this._dBContext.SaveChangesAsync();

            //  Devolvemos un mensaje de que se ha eliminado el vehiculo
            return NoContent();
        }
    }
}
