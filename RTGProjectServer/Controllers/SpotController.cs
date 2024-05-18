using ratagServerSide.BL;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Data.Common;


namespace ratagServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotController : ControllerBase
    {
       

        // GET api/<spotController>/5
        [HttpGet("{siteCode}")]
        public IActionResult Get(int siteCode)
        {
            try
            {
                Spot spot = new Spot();
                List<Spot> spots = spot.ReadSpotsInSite(siteCode);

                if (spots != null && spots.Any())
                {
                    return Ok(spots); // Return 200 OK with the spots data if there are spots
                }
                else
                {
                    return NotFound($"No spots found for site with code {siteCode}"); // Return 404 Not Found if there are no spots
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}"); // Return 500 Internal Server Error for other exceptions
            }
        }

        // POST api/<spotController>
        [HttpPost]
        public IActionResult Post([FromBody] Spot spot)
        {
            try
            {
                if (spot == null)
                {
                    return BadRequest("Invalid spot object"); 
                }

                int numEffected = spot.Insert();

                if (numEffected > 0)
                {
                    return Ok(numEffected); 
                }
                else
                {
                    return StatusCode(500, "Failed to insert spot"); 
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT api/<spotController>/5
        [HttpPut]
        public IActionResult Put([FromBody] Spot spot)
        {
            try
            {
                if (spot == null)
                {
                    return BadRequest("Invalid spot object");
                }

                int numEffected = spot.Update();

                if (numEffected > 0)
                {
                    return Ok(numEffected);
                }
                else
                {
                    return NotFound("Spot not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        // DELETE api/<spotController>/5
        [HttpDelete("{spotId}")]
        public IActionResult Delete(int spotId)
        {
            try
            {
                DBServices dbs = new DBServices();
                int numEffected = dbs.deleteSpot(spotId);

                if (numEffected > 0)
                {
                    return Ok(numEffected);
                }
                else
                {
                    return NotFound("Spot not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
