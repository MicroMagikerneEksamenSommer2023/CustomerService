using CustomerService.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using CustomerService.Services;
using MongoDB.Bson;
using System.Runtime.CompilerServices;

namespace CustomerService.Controllers;

[ApiController]
[Route("customerservice/v1")]
public class CustomerController : ControllerBase
{
    // Attributter
    private readonly ILogger<CustomerController> _logger;
    private readonly ICustomerDBService dBService;

    // Constructor
    public CustomerController(ILogger<CustomerController> logger, IConfiguration configuration, ICustomerDBService service)
    {
        _logger = logger;
        dBService = service;
    }

    // Henter alle kunder fra databasen
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var response = await dBService.GetAllCustomers();
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {

            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
    }

    // Henter en kunde fra databasen baseret på id
    [HttpGet("getbyid/{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        try
        {
            var response = await dBService.GetCustomerById(id);
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
    }

    // Henter en kunde fra databasen baseret på e-mail
    [HttpGet("getbyemail/{email}")]
    public async Task<IActionResult> GetByEmail([FromRoute] string email)
    {
        try
        {
            var response = await dBService.GetCustomerByEmail(email);
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
    }

    // Sletter en kunde fra databasen baseret på id
    [HttpDelete("deletebyid/{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] string id)
    {
        try
        {
            var response = await dBService.DeleteById(id);
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
    }

    // Sletter en kunde fra databasen baseret på e-mail
    [HttpDelete("deletebyemail/{email}")]
    public async Task<IActionResult> DeleteByEmail([FromRoute] string email)
    {
        try
        {
            var response = await dBService.DeleteByEmail(email);
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
    }

    // Opdaterer en kunde i databasen
    [HttpPut("updatecustomer")]
    public async Task<IActionResult> UpdateCustomer([FromBody] Customer data)
    {
        try
        {
            var response = await dBService.UpdateCustomer(data);
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
    }

    // Opretter en kunde i databsen
    [HttpPost("createcustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer data)
    {
        try
        {
            var response = await dBService.CreateCustomer(data);
            return Ok("You have succesfully created a customer!");
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
        //dBService.CreateCustomer(data.FirstName, data.LastName, data.Gender, data.BirthDate, data.Address, data.PostalCode, data.City, data.Country, data.Telephone, data.Email, data.AccessCode);
    }

    // Tjekker kundeoplysningerne mod databasen
    [HttpPost("checkcredentials")]
    public bool CheckCredentials([FromBody] Credentials data)
    {
        try
        {
            bool result = dBService.CheckCredentials(data.Email, data.AccessCode);
            return result;
        }
        catch (ItemsNotFoundException ex)
        {
            _logger.LogInformation("no match in the database");
            return false;
        }
    }

    // Henter version
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
