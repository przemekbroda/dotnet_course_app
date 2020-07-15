using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using course_app.Data;
using course_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace course_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    { 

        private readonly DataContext _dataContext;
        private readonly ILogger<ValuesController> _logger;


        public ValuesController(DataContext context, ILogger<ValuesController> logger) 
        {
            _dataContext = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetValues() 
        {
            var values = await _dataContext.Values.ToListAsync();
            _logger.LogInformation("Jest w pyte");
            return Ok(values);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var value = await _dataContext.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }





    }
}