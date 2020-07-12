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

namespace course_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly DataContext _dataContext;
         
        public ValuesController(DataContext context) 
        {
            _dataContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetValues() 
        {
            var values = await _dataContext.Values.ToListAsync();
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