using ratagServerSide.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ratagServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        //get all the groups in db
        // GET: api/<GroupController>
        [HttpGet]
        public IActionResult Get() 
        {
            try
            {
                Group g = new Group();
                
                return Ok(g.Read());
            }
            catch (Exception ex) {
                return StatusCode(500, "Internal Server Error"+ ex.Message);
            }
        }
        //get specific group by groupCode
        [Route("GetGroupByGroupCode")]
        [HttpGet]
        public IActionResult GetGroupByGroupCode(int groupCode) 
        {
            try
            {
                Group group = new Group();
                Group g = group.GetGroupByGroupCode(groupCode);

                if (g.GroupCode != 0)
                {
                    return Ok(g);
                }
                else
                {
                    return NotFound($"Group with groupCode {groupCode} not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    

        //// GET api/<GroupController>/5
        //[HttpGet("getPhoto/{groupCode}")]
        //public IActionResult GetPhoto(int groupCode)
        //{
        //    try
        //    {
        //        Group group = new Group();
        //        string photo = group.GetPhoto(groupCode);

        //        if (photo != null)
        //        {
        //            if (photo=="")
        //            {
        //                return NotFound("the group do not have image");

        //            }
        //            return Ok(photo);
        //        }
        //        else
        //        {
        //            return NotFound("group code not exist");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }
        //}


        //// GET api/<GroupController>/5
        //[HttpGet("GetEmoji/{groupCode}")]
        //public IActionResult GetEmoji(int groupCode)
        //{
        //    try
        //    {
        //        Group group = new Group();
        //        string emoji = group.GetEmoji(groupCode);

        //        if (emoji != null)
        //        {
        //            if (emoji == "")
        //            {
        //                return NotFound("the group do not have emoji");

        //            }
        //            return Ok(emoji);
        //        }
        //        else
        //        {
        //            return NotFound("groupCode not exist");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }
        //}

   
        // POST api/<GroupController>
       //insert group details
        [HttpPost]
        public IActionResult Post([FromBody] Group g)
        {
            if (g == null)
            {
                return BadRequest("Group data must not be null.");
            }
            try
            {
                return Ok(g.Insert());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error" + ex.Message);
            }
        }

        //insert group photo
        // Put api/<GroupController>
        [Route("Upload")]
        [HttpPut]
        public async Task<IActionResult> PutPhoto([FromForm] List<IFormFile> files, int groupCode)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return BadRequest("No files uploaded");
                }

                List<string> imageLinks = new List<string>();
                string fileName = "";
                string path = System.IO.Directory.GetCurrentDirectory();

                long size = files.Sum(f => f.Length);

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(path, "uploadedFiles/" + formFile.FileName);
                        fileName = formFile.FileName;
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        imageLinks.Add(formFile.FileName);
                    }
                }

                Group group = new Group();
                group.UpdatePhoto(groupCode, fileName);
                // Return status code  
                return Ok(imageLinks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }


        }

        //insert group emoji
        // PUT api/<GroupController>/5
        [Route("putEmoji")]
        [HttpPut]
        public IActionResult PutEmoji(int groupCode, string emoji)
        {
            try
            {
                Group group = new Group();
                int result = group.UpdatePhoto(groupCode, emoji);

                if (result > 0)
                {
                    return Ok(result); //numEffected
                }
                else
                {
                    return NotFound("Group not found"); //there is a default emoji so error happen only if groupCode not exist
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }



        //updates group total point
        // PUT api/<GroupController>/5
        [HttpPut("{groupCode}/{pointsToAdd}")]
        public IActionResult Put(int groupCode, int pointsToAdd)
        {
            try
            {
                Group group = new Group();
                int result = group.UpdateTotalPoints(groupCode, pointsToAdd);

                if (result >= 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Group not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        //// DELETE api/<GroupController>/5
        [HttpDelete("{groupCode}")]
        public IActionResult Delete(int groupCode)
        {
            try
            {
                DBServices dbs = new DBServices();
                int numEffected = dbs.deleteGroup(groupCode);

                if (numEffected > 0)
                {
                    return Ok(numEffected);
                }
                else
                {
                    return NotFound("Group not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
