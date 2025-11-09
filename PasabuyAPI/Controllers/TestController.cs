using Mapster;
using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class TestController
    {
        [HttpGet]
        public string Hello()
        {
            return "Hello";
        }

        [HttpGet("test")]
        public string Test()
        {
            return "Testing";
        }
    }
}