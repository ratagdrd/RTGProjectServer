using ratagServerSide.BL;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ratagServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionForActivityController : ControllerBase
    {
        //get questions for activity by activityCode
        // GET api/<QuestionForActivity>/5
        [HttpGet("{activitycode}")]
        public IActionResult Get(int activitycode)
        {
            try
            {
                DBServices dbs = new DBServices();
                List<QuestionForActivity> questions = dbs.GetQuestion(activitycode);

                if (questions != null && questions.Any())
                {
                    return Ok(questions);
                }
                else
                {
                    return NotFound("Activity code not found or No questions found for the activity");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        //insert ques, for future admin website
        // POST api/<QuestionForActivity>
        [HttpPost]
        public IActionResult Post([FromBody] QuestionForActivity ques)
        {
            try
            {
                if (ques == null)
                {
                    return BadRequest("Invalid question object");
                }

                int result = ques.Insert();

                if (result > 0)
                {
                    return Ok(result); //return numEffected
                }
                else
                {
                    return StatusCode(500, "Failed to insert question");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException innerException && innerException.Number == 547)
                {
                    // Foreign key constraint violation occurred (activityCode inserted doesnt exists)
                    return BadRequest("The provided activity code does not exist.");
                }
                else
                {
                    // Other exceptions
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
        }
        // PUT api/<QuestionForActivity>/5
        [HttpPut]
        public IActionResult Put([FromBody] QuestionForActivity q)
        {
            try
            {
                if (q == null)
                {
                    return BadRequest("Invalid question for activity object");
                }

                int numEffected = q.Update();

                if (numEffected > 0)
                {
                    return Ok(numEffected);
                }
                else
                {
                    return NotFound("qusetion for activity not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        //    // DELETE api/<QuestionForActivity>/5
        //    [HttpDelete("{id}")]
        //    public void Delete(int id)
        //    {
        //    }
    }
}
