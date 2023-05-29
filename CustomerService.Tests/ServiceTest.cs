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
    // Attributter til ILogger og IConfuguration
    private ILogger<CustomerController> _logger = null;
    private IConfiguration _configuration = null!;

    // Opsætter testmiljøet ved at initialisere _logger og _configuration
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

    // Tester oprettelse af en customer med succes
    [Test]
    public async Task CreateCustomerTest_Succes()
    {
        //Arrange
        var customer = CreateCustomer("mail@mail.dk", new DateTime(1999, 02, 02));
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

    // Tester oprettelse af en customer med fejl - Notfound
    [Test]
    public async Task CreateCustomerTest_Failure_NotFound()
    {
        //Arrange
        var customer = CreateCustomer("mail@mail.dk", new DateTime(2020, 02, 02));

        var stubService = new Mock<ICustomerDBService>();

        stubService.Setup(svc => svc.CreateCustomer(customer))
            .ThrowsAsync(new ItemsNotFoundException());

        var controller = new CustomerController(_logger,_configuration, stubService.Object);

        //Act
        var result = await controller.CreateCustomer(customer);

        //Assert
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>()); 
    }

    // Tester oprettelse af en customer med fejl - Expection
    [Test]
    public async Task CreateCustomerTest_Failure_Expection()
    {
        //Arrange
        var customer = CreateCustomer("mail@mail.dk", new DateTime(2020, 02, 02));

        var stubService = new Mock<ICustomerDBService>();

        stubService.Setup(svc => svc.CreateCustomer(customer))
            .ThrowsAsync(new Exception());

        var controller = new CustomerController(_logger,_configuration, stubService.Object);

        //Act
        var result = await controller.CreateCustomer(customer);

        //Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = (ObjectResult)result;
        Assert.AreEqual(500, objectResult.StatusCode);
    }

    // Tester opdaterer af en customer med succes
    [Test]
    public async Task UpdateCustomerTest_Succes()
    {
        //Arrange
        var customer = CreateCustomer("mail@mail.dk", new DateTime(1999, 02, 02));

        var stubService = new Mock<ICustomerDBService>();

        stubService.Setup(svc => svc.UpdateCustomer(customer))
            .Returns(Task.FromResult<Customer>(customer));

        var controller = new CustomerController(_logger,_configuration, stubService.Object);

        //Act
        var result = await controller.UpdateCustomer(customer);

        //Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>()); 
    }

    // Tester opdaterer af en customer med fejl - NotFound
    [Test]
    public async Task UpdateCustomerTest_Failure_NotFound()
    {
        //Arrange
        var customer = CreateCustomer("mail@mail.dk", new DateTime(2020, 02, 02));

        var stubService = new Mock<ICustomerDBService>();

        stubService.Setup(svc => svc.UpdateCustomer(customer))
            .ThrowsAsync(new ItemsNotFoundException());

        var controller = new CustomerController(_logger,_configuration, stubService.Object);

        //Act
        var result = await controller.UpdateCustomer(customer);

        //Assert
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>()); 
    }

    // Tester opdaterer af en customer med fejl - Expection
    [Test]
    public async Task UpdateCustomerTest_Failure_Expection()
    {
        //Arrange
        var customer = CreateCustomer("mail@mail.dk", new DateTime(2020, 02, 02));

        var stubService = new Mock<ICustomerDBService>();

        stubService.Setup(svc => svc.UpdateCustomer(customer))
            .ThrowsAsync(new Exception());

        var controller = new CustomerController(_logger,_configuration, stubService.Object);

        //Act
        var result = await controller.UpdateCustomer(customer);

        //Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = (ObjectResult)result;
        Assert.AreEqual(500, objectResult.StatusCode);
    }

    /// <summary>
    /// Helper method for creating Customer instance.
    /// </summary>
    /// <returns></returns>
    private Customer CreateCustomer(string email, DateTime dateTime)
    {
        var customer = new Customer("Test Firstname", "Test LastName", "Test Gender", 
                                    dateTime, "Test Address", "Test PostalCode", 
                                    "Test City", "Test Country", "Test Telephone", email, "Test AccessCode");
        
        return customer; 
     }

}
 
    