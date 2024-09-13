using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AudioOSSSwagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : Controller
    {
        private readonly BLogic bLogic;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        public AudioController()
        {
            bLogic = new BLogic();
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        /// <summary>
        /// Giver alt data på alle filer
        /// </summary>
        /// <returns>(.json) med alt gemt data</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Audio>>> GetAudio()
        {
            return Ok(await bLogic.GetAllAudio());
        }

        /// <summary>
        /// Giver alt data der matcher alle de category's som vi har sendt
        /// </summary>
        /// <param name="category">(.json) hvilke category's som er sendt</param>
        /// <returns>(.json) med data på alt data som matchede</returns>
        [HttpGet("Cat/{Category}")]
        public async Task<ActionResult<IEnumerable<Audio>>> GetAudio([FromBody] List<Category> category)
        {
            if (category.Any())
            {
                var categoryReturn = await bLogic.GetMultipleAudioCategory(category);
                if (categoryReturn.Any()) { return Ok(categoryReturn); } else { return BadRequest(new { error = "No audio with those categorys" }); }
            }
            return BadRequest(new { error = "Recieved no categorys" });
        }

        [HttpPut("{AudioID}")]
        public async Task<ActionResult<IEnumerable<Audio>>> UpdateAudio([FromBody] Audio audio, int AudioID)
        {
            bool works = await bLogic.EditAudio(audio, AudioID);
            if (works)
            {
                return Ok();
            }
            return BadRequest(new { error = "AudioID did not match" });
        }

        /// <summary>
        /// Giver alt data der matcher alle de genre som vi har sendt
        /// </summary>
        /// <param name="genre">(.json) hvilke genre som er sendt</param>
        /// <returns>(.json) med data på alt data som matchede</returns>
        [HttpGet("Gen/{Genre}")]
        public async Task<ActionResult<IEnumerable<Audio>>> GetAudio([FromBody] List<Genre> genre)
        {
            if (genre.Any())
            {
                var GenreReturn = await bLogic.GetMultipleAudioGenre(genre);
                if (GenreReturn.Any()) { return Ok(GenreReturn); } else { return BadRequest(new { error = "No audio with those genres" }); }
            }
            return BadRequest(new { error = "Recieved no genres" });
        }

        /// <summary>
        /// Giver alt data som er gemt om filen ud fra navnet på filen
        /// </summary>
        /// <param name="AudioName">(string) navn på filen</param>
        /// <returns>.json fil med dataen</returns>
        [HttpGet("Name/{AudioName}")]
        public async Task<IActionResult> GetOneAudioName(string AudioName = "")
        {
            var nameReturn = await bLogic.GetOneAudioName(AudioName);
            if (nameReturn.ID != 0) { return Ok(nameReturn); } else { return BadRequest(new { error = "Audio does not exist" }); }
        }

        /// <summary>
        /// Giver alt data som er gemt om filen ud fra id'et på filen
        /// </summary>
        /// <param name="AudioID">(int) ID på filen</param>
        /// <returns>.json fil med dataen</returns>
        [HttpGet("ID/{AudioID}")]
        public async Task<IActionResult> GetOneAudioID(int AudioID = 0)
        {
            var idReturn = await bLogic.GetOneAudioID(AudioID);
            if (idReturn.ID != 0) { return Ok(idReturn); } else { return BadRequest(new { error = "Audio does not exist" }); }
        }

        /// <summary>
        /// Giver alle variabler som skal gemmes om filen
        /// </summary>
        /// <param name="audio">.json fil</param>
        /// <returns>Success code</returns>
        [HttpPost]
        public async Task<IActionResult> AddNewAudio([FromBody] Audio audio)
        {
            var flag = await bLogic.AddNewAudio(audio);
            if (flag) { return Ok(); } else { return BadRequest(new { error = "Error while saving the audio" }); }
        }

        /// <summary>
        /// Her kan jeg gemme en fil på den lokale maskine hvor denne api kommer til at køre
        /// </summary>
        /// <param name="audioFile">(Form) Lydfilen som skal gemmes</param>
        /// <param name="username">(string) Navn på mappen som filen bliver gamt i</param>
        /// <returns>(string) Placering af filen</returns>
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadAudio([FromForm] IFormFile audioFile, string username)
        {
            if (audioFile == null || audioFile.Length == 0 || Path.GetExtension(audioFile.FileName).ToLower() != ".mp3")
            {
                return BadRequest(new { error = "Invalid file format" });
            }

            username = username.ToLower();
            var filePath = Path.Combine(_uploadPath, username, audioFile.FileName);

            if (!System.IO.File.Exists(Path.Combine(_uploadPath, username)))
            {
                Directory.CreateDirectory(Path.Combine(_uploadPath, username));
            }

            if (System.IO.File.Exists(filePath))
            {
                return BadRequest(new { error = "File exist already" });
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }

            return Ok(Path.Combine(username, audioFile.FileName));
        }

        /// <summary>
        /// Her laver jeg en function hvor jeg kan downloade mine filer når jeg modtager navnet af ejeren af filen (ejeren af filen fortæller mig hvilken mappe filen ligger i)
        /// Jeg skal også have navnet på filen med fileextension
        /// </summary>
        /// <param name="filename">(string) Navnet af på filen</param>
        /// <param name="fileowner">(string) Navn på mappen som filen bliver gamt i</param>
        /// <returns>(file) Lydfil</returns>
        [HttpGet("download/{filename}")]
        public IActionResult DownloadFile(string filename, string fileowner)
        {
            var filePath = Path.Combine(_uploadPath, fileowner, filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { error = "File not found" });
            }

            var mimeType = "audio/mpeg";
            return PhysicalFile(filePath, mimeType, Path.Combine(fileowner, filename));
        }

        /// <summary>
        /// Her gør jeg det muligt at slette mine filer
        /// </summary>
        /// <param name="fileplacement">placering af filen</param>
        /// <returns>Success/NotFound</returns>
        [HttpDelete("delete/{fileplacement}")]
        public IActionResult DeleteFile(string fileplacement)
        {
            var filePath = Path.Combine(_uploadPath, fileplacement);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { error = "File not found" });
            }

            System.IO.File.Delete(filePath);

            if (!System.IO.File.Exists(filePath))
            {
                return Ok();
            }

            return NotFound(new { error = "File not deleted but found" });
        }
    }
}
