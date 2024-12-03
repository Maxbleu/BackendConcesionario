using Backend_Concesionario.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;

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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this._dBContext.Vehiculos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            Vehiculo? vehiculo = await this._dBContext.Vehiculos.FirstOrDefaultAsync(vehiculo => vehiculo.Id == id);
            if (vehiculo == null)
                return NotFound();

            return Ok(vehiculo);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] Vehiculo vehiculo)
        {
            vehiculo.Id = this._dBContext.Vehiculos.Count() + 1;
            await this._dBContext.Vehiculos.AddAsync(vehiculo);
            
            await this._dBContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), vehiculo);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Dictionary<string, JsonElement> datosVehiculoActualizado)
        {
            Vehiculo? vehiculo = await this._dBContext.Vehiculos.FirstOrDefaultAsync(vehiculo => vehiculo.Id == id);

            if (vehiculo == null)
            {
                return NotFound();
            }

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

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Vehiculo? vehiculo = await this._dBContext.Vehiculos.FirstOrDefaultAsync(vehiculo => vehiculo.Id == id);
            
            if (vehiculo == null)
            {
                return NotFound();
            }

            this._dBContext.Remove(vehiculo);
            await this._dBContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
