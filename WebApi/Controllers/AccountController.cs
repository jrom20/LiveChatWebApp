using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return Ok("OK");
        }
    }
}