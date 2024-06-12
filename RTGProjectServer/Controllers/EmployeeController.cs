using ratagServerSide.BL;
using Microsoft.AspNetCore.Mvc;
using RTGProjectServer.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTGProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpPost]
        [Route("LogIn")]
        public int PostLogIn(string username, string password)
        {
            Employee employee = new Employee();
            return employee.LogIn(username, password);
        }

    }
}
