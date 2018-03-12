using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.Abstractions;

namespace Simple.Controllers
{
    [Route("api/[controller]")]
    public class EchoController : Controller
    {
        private IEchoService _service;

        public EchoController(IEchoService service)
        {
            _service = service;
        }

        [HttpGet("{value}")]
        public IActionResult Get(string value)
        {
            return Ok(_service.Echo(value));
        }
    }
}
