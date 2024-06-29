using ratagServerSide.BL;
using Microsoft.AspNetCore.Mvc;
using RTGProjectServer.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ratagServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        // GET: api/<ActivityController>
        //get all activities in db
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                Activity activity = new Activity();
                List<Activity> activities = activity.Read();
                return Ok(activities); // 200 OK status with the list of activities
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        // GET api/<ActivityController>/5
        //get specific activity by activity code
        [HttpGet("{code}")]
        public IActionResult Get(int code)
        {
            try
            {
                Activity activity = new Activity();
                Activity requestedActivity = activity.getActivityByCode(code);
                if (requestedActivity.Activitycode != 0)
                {
                    return Ok(requestedActivity); // 200 OK status with the requested activity
                }
                else
                {
                    return NotFound("Activity not found"); // 404 Not Found
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/<ActivityController>
        //for future admin website
        [HttpPost]
        public IActionResult Post([FromBody] Activity activity)
        {
            if (activity == null)
            {
                return BadRequest("Invalid activity object");
            }

            int result = activity.Insert();
            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(500, "Failed to insert activity");
            }
        }
       
        //update activity rate
        // PUT api/<ActivityController>/5
        [HttpPut("{activitycode}/{rateToAdd}")]
        public IActionResult Put(int activitycode, int rateToAdd)
        {
            try
            {
                Activity activity = new Activity();
                int result = activity.UpdateRate(activitycode, rateToAdd);

                if (result > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Activity not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update rate: {ex.Message}");
            }
        }



        [HttpPut]
        public IActionResult PutActivity(int activityCode, string activityname, string instruction)
        {
            try
            {
                Activity a = new Activity();
                int numEffected = a.Update(activityCode, activityname, instruction);

                if (numEffected > 0)
                {
                    return Ok(numEffected);
                }
                else
                {
                    return NotFound("activity not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        //// DELETE api/<ActivityController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
