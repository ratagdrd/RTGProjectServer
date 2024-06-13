using RTGProjectServer.BL;
using Microsoft.AspNetCore.Mvc;
using ratagServerSide.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTGProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostLogIn([FromBody]  Employee e)
        {
            if (e == null)
            {
                return BadRequest(new { success = false, message = "Invalid request payload" });
            }

            int result = e.LogIn();
            if (result == 1)
            {
                return Ok(new { success = true });
            }
            else
            {
                return Unauthorized(new { success = false, message = "Invalid credentials" });
            }

        }

    }
}
