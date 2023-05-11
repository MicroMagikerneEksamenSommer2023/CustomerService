using CustomerService.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using CustomerService.Services;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Runtime.CompilerServices;

namespace CustomerService.Controllers;

[ApiController]
[Route("customerservice/v1")]
public class CustomerController : ControllerBase
{


    private readonly ILogger<CustomerController> _logger;

    private readonly CustomerDBService dBService;

    public CustomerController(ILogger<CustomerController> logger, CustomerDBService service)
    {
        _logger = logger;
        dBService = service;
    }

    [HttpGet("getall")]
    public IEnumerable<Customer> GetAll()
    {
        _logger.LogInformation("'getall'-call recieved in controller");
        return dBService.GetAllCustomers();
    }


    [HttpGet("getbyid/{id}")]
    public Customer GetById([FromRoute]string id)
    {
        ObjectId oi = new ObjectId("" + id);
        _logger.LogInformation("Call recieved, the converted oi is:" + oi.ToString);
        return dBService.GetCustomerById(id);
    }


    [HttpGet("getbyemail/{email}")]
    public Customer GetByEmail([FromRoute]string email)
    {
        _logger.LogInformation("Call recieved, the email is " + email);
        return dBService.GetCustomerByEmail(email);
    }


    [HttpDelete("deletebyid/{id}")]
    public Customer DeleteById([FromRoute]string id)
    {
        ObjectId oi = new ObjectId("" + id);
        _logger.LogInformation("Call recieved, the converted oi is:" + oi.ToString);
        return dBService.DeleteById(id);
    }


    [HttpDelete("deletebyemail/{email}")]
    public Customer DeleteByEmail([FromRoute] string email)
    {
        _logger.LogInformation("Call recieved, the email is " + email);
        return dBService.DeleteByEmail(email);
    }


    [HttpPut("updatecustomer")]
    public Customer UdpdateCustomer([FromBody]Customer data)
    {
        _logger.LogInformation("Call revieved, the firstname om the customer is:" + data.FirstName);
        return dBService.UpdateCustomer(data);
    }


    [HttpPost("createcustomer")]
    public Customer CreateCustomer([FromBody] Customer data)
    {
        _logger.LogInformation("Call revieved, " + data.ToString());
        dBService.CreateCustomer(data);
        //dBService.CreateCustomer(data.FirstName, data.LastName, data.Gender, data.BirthDate, data.Address, data.PostalCode, data.City, data.Country, data.Telephone, data.Email, data.AccessCode);
        return data;
    }


    [HttpGet("version")]
    public IEnumerable<string> GetVersion()
    {
        var properties = new List<string>();
        var assembly = typeof(Program).Assembly;

        foreach (var attribute in assembly.GetCustomAttributesData())
        {
            properties.Add($"{attribute.AttributeType.Name} - {attribute.ToString()}");
        }

        return properties;
    }
}
