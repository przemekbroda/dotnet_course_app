using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using course_app.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace course_app.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ISeed _seed;
        public SeedController(ISeed seed)
        {
            _seed = seed;
        }

        [HttpGet]
        public IActionResult SeedDb()
        {
            _seed.SeedUsers();
            return Ok();
        }

    }
}
