using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using CustomerService.Controllers;
using CustomerService.Services;
using Microsoft.AspNetCore.Mvc;
using CustomerService.Models;

namespace Service.Tests;

public class Tests
{
    private ILogger<CustomerController> _logger = null;
    private IConfiguration _configuration = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CustomerController>>().Object;

        var myConfiguration = new Dictionary<string, string?>
        {
            {"CustomerServiceBrokerHost", "http://testhost.local"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
    }

    [Test]
    public async Task CreateCustomerTest_Succes()
    {
        //Arrange
        var customer = CreateCustomer("mail@mail.dk");
        bool customerTrue = true;

        var stubService = new Mock<ICustomerDBService>();

        stubService.Setup(svc => svc.CreateCustomer(customer))
            .Returns(Task.FromResult<bool>(customerTrue));

        var controller = new CustomerController(_logger,_configuration, stubService.Object);

        //Act
        var result = await controller.CreateCustomer(customer);

        //Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>()); 
    }


/*
CreateCustomer i CustomerDBService returner aldrig en false
     [Test]
    public async Task CreateCustomerTest_NotFound()
    {
        //Arrange
        var customer = CreateCustomer("mail@mail.dk");
        bool customerFalse = false;

        var stubService = new Mock<ICustomerDBService>();

        stubService.Setup(svc => svc.CreateCustomer(customer))
            .Returns(Task.FromResult<bool>(customerFalse));

        var controller = new CustomerController(_logger,_configuration, stubService.Object);

        //Act
        var result = await controller.CreateCustomer(customer);

        //Assert
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>()); 
    }

*/
     /// <summary>
    /// Helper method for creating Customer instance.
    /// </summary>
    /// <returns></returns>
    private Customer CreateCustomer(string email)
    {
        var customer = new Customer("Test Firstname", "Test LastName", "Test Gender", 
                                    new DateTime(1999, 09, 20), "Test Address", "Test PostalCode", 
                                    "Test City", "Test Country", "Test Telephone", email, "Test AccessCode");
        
        return customer; 
     }

}
 
    