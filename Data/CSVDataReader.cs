using Backend_Concesionario.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Backend_Concesionario.Data
{
    public static class CSVDataReader
    {

        private static string csvFilePath = Path.Combine(Environment.CurrentDirectory, "Data", "cars.csv");

        public static List<Vehiculo> LeerCSVVehiculos()
        {
            List<Vehiculo> vehiculos = null;

            using (var reader = new StreamReader(csvFilePath))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ",",
                };

                using (var csv = new CsvReader(reader, config))
                {
                    vehiculos = csv.GetRecords<Vehiculo>().ToList();
                }
            }

            return vehiculos;
        }

    }
}
