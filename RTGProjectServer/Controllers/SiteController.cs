using ratagServerSide.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ratagServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        //get all sites
        // GET: api/<SiteController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                Site s = new Site();
                List<Site> sitesList = s.Read();

                if (sitesList != null && sitesList.Any())
                {
                    return Ok(sitesList); // Return 200 OK with the sites data if there are sites
                }
                else
                {
                    return NotFound("No sites found"); // Return 404 Not Found if there are no sites
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}"); // Return 500 Internal Server Error for other exceptions
            }
        }


        //get site by siteCode
        // GET api/<SiteController>/5
        [HttpGet("{siteCode}")]
        public IActionResult GetSiteByCode(int siteCode)
        {
            try
            {
                Site site = new Site();
                Site s = site.ReadSiteByCode(siteCode);

                if (s.SiteCode != 0)
                {
                    return Ok(s); // Return 200 OK with the site data
                }
                else
                {
                    return NotFound($"Site with siteCode {siteCode} not found"); // Return 404 Not Found if site is not found
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}"); // Return 500 Internal Server Error for other exceptions
            }
        }

        //insert site, for future admin website
        // POST api/<SiteController>
        [HttpPost]
        public IActionResult Post([FromBody] Site site)
        {
            try
            {
                if (site == null)
                {
                    return BadRequest("Invalid site object"); // Return 400 Bad Request if the site object is null
                }

                int numEffected = site.Insert();

                if (numEffected > 0)
                {
                    return Ok(numEffected); // Return 200 OK with the number of affected rows if insertion is successful
                }
                else
                {
                    return StatusCode(500, "Failed to insert site"); // Return 500 Internal Server Error if insertion fails
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}"); // Return 500 Internal Server Error for other exceptions
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Site s)
        {
            try
            {
                if (s == null)
                {
                    return BadRequest("Invalid spot object");
                }

                int numEffected = s.Update();

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
