using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Auth0
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            var claims = User.Claims;
            var Name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

            return new string[] { "value1", "value2" };
        }

    }
}
