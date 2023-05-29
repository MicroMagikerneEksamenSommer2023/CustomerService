using CustomerService.Controllers;
using CustomerService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace CustomerService.Services
{
    public interface ICustomerDBService
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(string id);
        Task<Customer> GetCustomerByEmail(string email);
        Task<Customer> DeleteById(string id);
        Task<Customer> DeleteByEmail(string email);
        Task<Customer> UpdateCustomer(Customer data);
        Task<bool> CreateCustomer(Customer data);
        bool CheckIfExists(string email);
        bool CheckCredentials(string email, string password);
    }
    public class CustomerDBService : ICustomerDBService
    {
        // Attributter
        private readonly ILogger<CustomerDBService> _logger;

        private readonly IMongoCollection<Customer> _customers;

        private readonly IConfiguration _config;

        // Constructor
        public CustomerDBService(ILogger<CustomerDBService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            var mongoClient = new MongoClient(_config["connectionsstring"]);
            var database = mongoClient.GetDatabase(_config["database"]);
            _customers = database.GetCollection<Customer>(_config["collection"]);
        }

        // Henter alle kunder fra databasen
        public async Task<List<Customer>> GetAllCustomers()
        {
            var filter = Builders<Customer>.Filter.Empty;
            var dbData = (await _customers.FindAsync(filter)).ToList();
            if (dbData.Count == 0)
            {
                throw new ItemsNotFoundException("No customers were found in the database.");
            }
            return dbData;
        }

        // Henter en kunde fra databasen baseret på id
        public async Task<Customer> GetCustomerById(string id)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, id);
            var dbData = (await _customers.FindAsync(filter)).FirstOrDefault();
            if (dbData == null)
            {
                throw new ItemsNotFoundException($"No customer with ID {id} was found in the database.");
            }
            return dbData;
        }

        // Henter en kunde fra databasen baseret på e-mail
        public async Task<Customer> GetCustomerByEmail(string email)
        {
            string lowerEmail = email.ToLower();
            var filter = Builders<Customer>.Filter.Eq(c => c.Email, lowerEmail);
            var dbData = (await _customers.FindAsync(filter)).FirstOrDefault();
            if (dbData == null)
            {
                throw new ItemsNotFoundException($"No customer with Email {email} was found in the database.");
            }
            return dbData;
        }

        // Sletter en kunde fra databasen baseret på id
        public async Task<Customer> DeleteById(string id)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, id);
            var dbData = await _customers.FindOneAndDeleteAsync(filter);
            if (dbData == null)
            {
                throw new ItemsNotFoundException($"No item with ID {id} was found in the database for deletion.");
            }
            return dbData;
        }

        // Sletter en kunde fra databasen baseret på e-mail
        public async Task<Customer> DeleteByEmail(string email)
        {
            string lowerEmail = email.ToLower();
            var filter = Builders<Customer>.Filter.Eq(c => c.Email, lowerEmail);
            var dbData = await _customers.FindOneAndDeleteAsync(filter);
            if (dbData == null)
            {
                throw new ItemsNotFoundException($"No item with Email {email} was found in the database for deletion.");
            }
            return dbData;
        }

        // Opdaterer en kunde i databasen
        public async Task<Customer> UpdateCustomer(Customer data)
        {
            bool alreadyExisting = CheckIfExists(data.Email);
            if (alreadyExisting)
            {
                throw new Exception("There is already a customer with this email: " + data.Email);
            }
            if (!IsEmail(data.Email) || data.BirthDate > DateTime.Now.AddYears(-18))
            {
                throw new Exception("Email or Age ar not allowed");
            }
            string lowerEmail = data.Email.ToLower();
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, data.Id);
            var update = Builders<Customer>.Update.Set(c => c.FirstName, data.FirstName).Set(c => c.LastName, data.LastName).Set(c => c.Gender, data.Gender).Set(c => c.BirthDate, data.BirthDate).Set(c => c.Address, data.Address).Set(c => c.PostalCode, data.PostalCode).Set(c => c.City, data.City).Set(c => c.Country, data.Country).Set(c => c.Telephone, data.Telephone).Set(c => c.Email, lowerEmail).Set(c => c.AccessCode, data.AccessCode);
            var dbData = await _customers.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<Customer> { ReturnDocument = ReturnDocument.After });
            if (dbData == null)
            {
                throw new ItemsNotFoundException($"No item with ID {data.Id} was found in the database for deletion.");
            }
            return dbData;
        }

        // Opretter en kunde i databasen
        public async Task<bool> CreateCustomer(Customer data)
        {
            bool alreadyExisting = CheckIfExists(data.Email);
            if (alreadyExisting)
            {
                throw new Exception("There is already a customer with this email: " + data.Email);
            }
            Customer temp = data;
            temp.Id = null;
            temp.Email = temp.Email.ToLower();
            if (!IsEmail(temp.Email) || temp.BirthDate > DateTime.Now.AddYears(-18))
            {
                throw new Exception("Email or Age ar not allowed");
            }
            try
            {
                await _customers.InsertOneAsync(temp);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert document: " + ex.Message);
            }
        }

        // Tjekker om en kunde allerede eksisterer i databasen baseret på email
        public bool CheckIfExists(string email)
        {
            string lowerEmail = email.ToLower();
            var filter = Builders<Customer>.Filter.Eq(c => c.Email, lowerEmail);
            List<Customer> checker = new List<Customer>();
            checker = _customers.Find(filter).ToList();
            if (checker.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Tjekker om kundeoplysningerne er gyldige ved at sammenligne email og adgangskode
        public bool CheckCredentials(string email, string password)
        {
            string lowerEmail = email.ToLower();
            var filter = Builders<Customer>.Filter.Eq(c => c.Email, lowerEmail) & Builders<Customer>.Filter.Eq(c => c.AccessCode, password);
            var temp = _customers.Find(filter).FirstOrDefault();
            if (temp == null)
            {

                throw new ItemsNotFoundException("No customer matched the email and password");
            }
            else
            {
                return true;
            }
        }

        // Tjekker om input er en e-mail
        static bool IsEmail(string input)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern);

            // Use the Regex.IsMatch() method to check if the input matches the pattern
            return regex.IsMatch(input);
        }
    }
}
