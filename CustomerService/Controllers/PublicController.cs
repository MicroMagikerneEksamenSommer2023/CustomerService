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
    // Attributter
    private readonly ILogger<PublicController> _logger;
    private readonly ICustomerDBService dBService;

    // Constructor
    public PublicController(ILogger<PublicController> logger, ICustomerDBService service)
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
