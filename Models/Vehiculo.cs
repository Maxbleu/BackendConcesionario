
using CsvHelper.Configuration.Attributes;
using System.Text.Json.Serialization;

namespace Backend_Concesionario.Models
{
    public class Vehiculo
    {
        [Ignore]
        [JsonIgnore]
        public int Id { get; set; }

        [Name("First Name")]
        public string FirstName { get; set; }

        [Name("Last Name")]
        public string LastName { get; set; }

        [Name("Country")]
        public string Country { get; set; }

        [Name("Car Brand")]
        public string CarBrand { get; set; }

        [Name("Car Model")]
        public string CarModel { get; set; }

        [Name("Car Color")]
        public string CarColor { get; set; }

        [Name("Year of Manufacture")]
        public int YearOfManufacture { get; set; }

        [Name("Credit Card Type")]
        public string CreditCardType { get; set; }
    }
}
