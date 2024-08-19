using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AudioOSSSwagger.Controllers
{
    [Route("api/Audio/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private BLogic bLogic;
        public CategoryController() { bLogic = new BLogic(); }

        /// <summary>
        /// This gets all the categorys that have been created
        /// </summary>
        /// <returns>(.json) with all the category's</returns>
        [HttpGet]
        public async Task<IActionResult> GetCategory()
        {
            return Ok(await bLogic.GetCategory());
        }

        /// <summary>
        /// Adds a new category to the list
        /// </summary>
        /// <param name="category">(Category) custom object</param>
        /// <returns>Success code</returns>
        [HttpPost]
        public async Task<IActionResult> AddNewCategory([FromBody] Category category)
        {
            var flag = await bLogic.AddNewCategory(category);
            if (flag) { return Ok(); } else { return BadRequest(new { error = "error = Category exist already" }); }
        }
    }
}
