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
        public IActionResult Get(int activitycode)
        {
            try
            {
                ActivityStatus status = new ActivityStatus();
                ActivityStatus result = status.GetStat(activitycode);

                if (result != null)
                {
                    return Ok(result); // Return 200 OK with the fetched ActivityStatus object
                }
                else
                {
                    return NotFound($"Activity status not found for code {activitycode}"); // Return 404 Not Found if activity status is not found
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}"); // Return 500 Internal Server Error for any other exceptions
            }
        }



        [HttpPut]
        public IActionResult Put(int activityCode, bool isAccessible, bool isBlocked)
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
