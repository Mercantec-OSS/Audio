using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AudioOSSSwagger.Controllers
{
    [Route("api/Audio/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private BLogic bLogic;
        public GenreController() { bLogic = new BLogic(); }

        /// <summary>
        /// This gets all the genre's that have been created
        /// </summary>
        /// <returns>(.json) with all the genre's</returns>
        [HttpGet]
        public async Task<IActionResult> GetGenre()
        {
            return Ok(await bLogic.GetGenre());
        }

        /// <summary>
        /// Adds a new genre to the list
        /// </summary>
        /// <param name="genre">(Genre) custom object</param>
        /// <returns>Success code</returns>
        [HttpPost]
        public async Task<IActionResult> AddNewGenre([FromBody] Genre genre)
        {
            var flag = await bLogic.AddNewGenre(genre);
            if (flag) { return Ok(); } else { return BadRequest(new { error = "error = Category exist already" }); }
        }
    }
}
