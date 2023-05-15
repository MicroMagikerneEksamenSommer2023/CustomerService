using CustomerService.Controllers;
using CustomerService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CustomerService.Services
{ 
    public class CustomerDBService
{
    private readonly ILogger<CustomerDBService> _logger;

    private readonly IMongoCollection<Customer> _customers;

    private readonly IConfiguration _config;

    public CustomerDBService(ILogger<CustomerDBService> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
     
        var mongoClient = new MongoClient(_config["connectionsstring"]);
        var database = mongoClient.GetDatabase(_config["database"]);
        _customers = database.GetCollection<Customer>(_config["collection"]);
    }
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
    public async Task<Customer> UpdateCustomer(Customer data)
    {
         bool alreadyExisting = CheckIfExists(data.Email);
        if(alreadyExisting){
            throw new Exception("There is already a customer with this email: " + data.Email);
        }
        string lowerEmail = data.Email.ToLower();
        var filter = Builders<Customer>.Filter.Eq(c => c.Id, data.Id);
        var update = Builders<Customer>.Update.Set(c => c.FirstName, data.FirstName).Set(c => c.LastName, data.LastName).Set(c => c.Gender, data.Gender).Set(c => c.BirthDate, data.BirthDate).Set(c => c.Address, data.Address).Set(c => c.PostalCode, data.PostalCode).Set(c => c.City, data.City).Set(c => c.Country, data.Country).Set(c => c.Telephone, data.Telephone).Set(c => c.Email, lowerEmail).Set(c => c.AccessCode, data.AccessCode);
        var dbData = await _customers.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<Customer>{ ReturnDocument = ReturnDocument.After });
         if (dbData == null)
        {
        throw new ItemsNotFoundException($"No item with ID {data.Id} was found in the database for deletion.");
        }
        return dbData;
      
    }
        public async Task<bool> CreateCustomer(Customer data)
    {
        bool alreadyExisting = CheckIfExists(data.Email);
        if(alreadyExisting){
            throw new Exception("There is already a customer with this email: " + data.Email);
        }
        Customer temp = data;
        temp.Id = null;
        temp.Email = temp.Email.ToLower();
        try
        {
            await _customers.InsertOneAsync(temp);
            return true;
        }
        catch(Exception ex)
        {
            throw new Exception("Failed to insert document: " + ex.Message);
        }

        

    }
    public bool CheckIfExists(string email)
    {
        string lowerEmail = email.ToLower();
        var filter = Builders<Customer>.Filter.Eq(c=>c.Email, lowerEmail);
        List<Customer> checker = new List<Customer>();
        checker = _customers.Find(filter).ToList();
        if(checker.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckCredentials(string email, string password)
    {
        string lowerEmail = email.ToLower();
        var filter = Builders<Customer>.Filter.Eq(c=> c.Email, lowerEmail) & Builders<Customer>.Filter.Eq(c=>c.AccessCode, password);
        var temp = _customers.Find(filter).FirstOrDefault();
        if (temp == null)
        {
            return false;
        }
        else {
            return true;
        }
    }
}
}
