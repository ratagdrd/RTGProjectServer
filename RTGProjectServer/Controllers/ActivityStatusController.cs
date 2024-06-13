using RTGProjectServer.BL;
using Microsoft.AspNetCore.Mvc;
using ratagServerSide.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTGProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityStatusController : ControllerBase
    {
        // GET: api/<ActivityStatusController>
        [HttpGet]
        public ActivityStatus Get(int activitycode)   //להוסיף חריגות
        {
                ActivityStatus status = new ActivityStatus();
               return status.GetStat(activitycode);

        }

        [HttpPut]
        public IActionResult Put(int activityCode, bool isAccessible, bool isBlocked) //לוודא חריגות
        {
            try
            {
                ActivityStatus status = new ActivityStatus();
                int numEffected = status.Update(activityCode, isAccessible, isBlocked);

                if (numEffected > 0)
                {
                    return Ok(numEffected);
                }
                else
                {
                    return NotFound("status not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }
    }
}
