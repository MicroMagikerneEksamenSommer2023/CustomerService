using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection;
using ThirdParty.Json.LitJson;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CustomerService.Models
{
    public class CustomerNoIdDTO
    {
        [Newtonsoft.Json.JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Newtonsoft.Json.JsonProperty("lastName")]
        public string LastName { get; set; }

        [Newtonsoft.Json.JsonProperty("gender")]
        public string Gender { get; set; }

        [Newtonsoft.Json.JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [Newtonsoft.Json.JsonProperty("address")]
        public string Address { get; set; }

        [Newtonsoft.Json.JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [Newtonsoft.Json.JsonProperty("city")]
        public string City { get; set; }

        [Newtonsoft.Json.JsonProperty("country")]
        public string Country { get; set; }

        [Newtonsoft.Json.JsonProperty("telephone")]
        public string Telephone { get; set; }

        [Newtonsoft.Json.JsonProperty("email")]
        public string Email { get; set; }

        [Newtonsoft.Json.JsonProperty("accessCode")]
        public string AccessCode { get; set; }

  
        public CustomerNoIdDTO(string firstName, string lastName, string gender, DateTime birthDate, string address, string postalCode, string city, string country, string telephone, string email, string accessCode)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.BirthDate = birthDate;
            this.Address = address;
            this.PostalCode = postalCode;
            this.City = city;
            this.Country = country;
            this.Telephone = telephone;
            this.Email = email;
            this.AccessCode = accessCode;
        }
        public override string ToString()
        {
            return $"{FirstName}, {LastName}, {Gender}, {BirthDate.ToString()}, {Address}, {PostalCode}, {City}, {Country}, {Telephone}, {Email}, {AccessCode}";
        }
    }
}
