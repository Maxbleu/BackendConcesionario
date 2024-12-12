using System.Text;
using Backend_Concesionario;
using Backend_Concesionario.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


// ***********************************************
// Configuración del proyecto
// ***********************************************

// 1. Cargar las clases necesarias para inyección de dependencias
// --------------------------------------------------------------
// `AplicationDBContext` es nuestro contexto de base de datos, el
//  cual registro como Singleton para que sea compartido por toda la aplicación
builder.Services.AddSingleton<AplicationDBContext>();

// `VehiculoService` aprovechando la interfaz que implementa
// podremos obtener los vehículos e insertarlos en la base de datos
builder.Services.AddSingleton<IHostedService, VehiculoService>();


// 2. Configurar controladores y endpoints
// ----------------------------------------
// Se habilitan los controladores para procesar solicitudes HTTP
builder.Services.AddControllers();

// Cargar la API para exploración y pruebas (Swagger u otros métodos)
builder.Services.AddEndpointsApiExplorer();


// 3. Configurar la documentación de Swagger
// ------------------------------------------
// Swagger permite generar y visualizar documentación interactiva de la API.
builder.Services.AddSwaggerGen();


// 4. Configurar autenticación JWT
// --------------------------------
// Obtenemos la clave secreta desde la configuración para firmar los tokens JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,                              // Verificar el emisor
        ValidateAudience = true,                            // Verificar el destinatario
        ValidateLifetime = true,                            // Verificar la vigencia del token
        ValidateIssuerSigningKey = true,                    // Validar la clave de firma
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)    // Clave de firma
    };
});

// Registrar el servicio de tokens, que se encargará de crear y validar JWT
builder.Services.AddScoped<TokenService>();


// 5. Construcción de la aplicación
// ----------------------------------
var app = builder.Build();

// Configuración para el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();                        // Página de errores amigable para desarrollo
    app.UseSwagger();                                       // Habilitar Swagger para pruebas
    app.UseSwaggerUI();                                     // Interfaz de usuario para Swagger
}
else
{
    // Manejo de errores en producción
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/html";

            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerFeature != null)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(exceptionHandlerFeature.Error, "Se produjo una excepción.");
                await context.Response.WriteAsync("Ocurrió un error en el servidor.");
            }
        });
    });
}


// 5.1 Habilitar el enrutado
app.UseRouting();

// 5.2 Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// 5.3 Configuración de endpoints
// Aquí se procesan las rutas asociadas a los controladores
app.MapControllers();

// 5.4 Ejecutar la aplicación
app.Run();