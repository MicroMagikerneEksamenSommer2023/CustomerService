using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection;
using ThirdParty.Json.LitJson;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CustomerService.Models
{
    public class Credentials
    {
       
        [Newtonsoft.Json.JsonProperty("email")]
        public string Email { get; set; }

        [Newtonsoft.Json.JsonProperty("accessCode")]
        public string AccessCode { get; set; }

  
        public Credentials(string email, string accessCode)
        {
            
            this.Email = email;
            this.AccessCode = accessCode;
        }
    
    }
}
