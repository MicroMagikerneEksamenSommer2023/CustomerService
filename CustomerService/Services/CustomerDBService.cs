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
     
       /* var mongoClient = new MongoClient("mongodb://localhost:27017/");
        var database = mongoClient.GetDatabase("TEST");
        _customers = database.GetCollection<Customer>("Customers");*/
            var mongoClient = new MongoClient(_config["connectionsstring"]);
            var database = mongoClient.GetDatabase(_config["database"]);
            _customers = database.GetCollection<Customer>(_config["collection"]);
    }
    public List<Customer> GetAllCustomers()
    {
        var filter = Builders<Customer>.Filter.Empty;
        return _customers.Find(filter).ToList();
    }
    public Customer GetCustomerById(string id)
    {
        var filter = Builders<Customer>.Filter.Eq(c => c.Id, id);
        return _customers.Find(filter).FirstOrDefault();
    }
    public Customer GetCustomerByEmail(string email)
    {
        var filter = Builders<Customer>.Filter.Eq(c => c.Email, email.ToLower());
        return _customers.Find(filter).FirstOrDefault();
    }
    public Customer DeleteById(string id)
    {
        var filter = Builders<Customer>.Filter.Eq(c => c.Id, id);
        return _customers.FindOneAndDelete(filter);
    }
    public Customer DeleteByEmail(string email)
    {
        var filter = Builders<Customer>.Filter.Eq(c => c.Email, email);
        return _customers.FindOneAndDelete(filter);
    }
    public Customer UpdateCustomer(Customer data)
    {
        var filter = Builders<Customer>.Filter.Eq(c => c.Id, data.Id);
        var update = Builders<Customer>.Update.Set(c => c.FirstName, data.FirstName).Set(c => c.LastName, data.LastName).Set(c => c.Gender, data.Gender).Set(c => c.BirthDate, data.BirthDate).Set(c => c.Address, data.Address).Set(c => c.PostalCode, data.PostalCode).Set(c => c.City, data.City).Set(c => c.Country, data.Country).Set(c => c.Telephone, data.Telephone).Set(c => c.Email, data.Email).Set(c => c.AccessCode, data.AccessCode);
            return _customers.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<Customer>{ ReturnDocument = ReturnDocument.After });
      
            
       
        }
        public void CreateCustomer(Customer data)
        //public void CreateCustomer(string firstname, string lastname, string gender, DateTime birthdate, string address, string postalcode, string city, string country, string telephone, string email, string accesscode)
    {
        _logger.LogInformation("jaja");
        //Customer temp = new Customer(firstname, lastname, gender, birthdate, address, postalcode, city, country, telephone, email, accesscode);
        _customers.InsertOneAsync(data);


    }
}
}
