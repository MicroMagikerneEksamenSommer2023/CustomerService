using CustomerService.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using CustomerService.Services;
using MongoDB.Bson;
using System.Runtime.CompilerServices;

namespace CustomerService.Controllers;

[ApiController]
[Route("customerservice/public/v1")]
public class PublicController : ControllerBase
{


    private readonly ILogger<CustomerController> _logger;

    private readonly CustomerDBService dBService;

    public PublicController(ILogger<CustomerController> logger, CustomerDBService service)
    {
        _logger = logger;
        dBService = service;
    }

    [HttpPost("createcustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer data)
    {
           try
        {
            var response = await dBService.CreateCustomer(data);
            return Ok("Customer created succesfully");
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
}
