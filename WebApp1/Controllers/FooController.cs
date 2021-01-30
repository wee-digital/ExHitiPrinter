using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FooController : ControllerBase
    {
   
        private readonly ILogger<FooController> _logger;

        public FooController(ILogger<FooController> logger)
        {
            _logger = logger;
        }

        public Response SuccessResponse() {
            return new Response
            {
                code = 0,
                message = "successfully"
            };
        }
        
        [HttpGet("{id}")]
        public Response Get(int id)
        {
            return SuccessResponse();
        }

        [HttpPost]
        public async Task<ActionResult<Response>> Post(Foo foo)
        {
            return SuccessResponse();
        }




    }
}
