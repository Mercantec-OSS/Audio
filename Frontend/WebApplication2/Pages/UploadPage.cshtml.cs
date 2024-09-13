using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Models;

namespace AudioMercantec.Pages
{
    public class UploadPage : PageModel
    {
        public UploadPage()
        {

        }

        [BindProperty]
        public IFormFileCollection Files { get; set; }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Tags { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public bool AcceptsLicense { get; set; }

        /// <summary>
        /// This first posts the audiofile and then posts the info on the audiofile
        /// </summary>
        /// <returns>redirect</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            // Step 1: Upload the files
            var fileLocations = await UploadFilesAsync(Files, "Jeppe");

            if (fileLocations == null || !fileLocations.Any())
            {
                ModelState.AddModelError("", "File upload failed.");
                return Page();
            }

            // Step 2: Submit the form data
            var isSuccess = await SubmitFormDataAsync(fileLocations);

            if (isSuccess)
            {
                // Redirect to success page or display success message
                return RedirectToPage("/Success");
            }

            ModelState.AddModelError("", "Failed to submit form data.");
            return Page();
        }

        /// <summary>
        /// Post the files to the backend
        /// </summary>
        /// <param name="files">the file that is being uploaded</param>
        /// <param name="userName">the person that uploads the file</param>
        /// <returns>(string) the fileplacement at the backend</returns>
        private async Task<List<string>> UploadFilesAsync(IFormFileCollection files, string userName)
        {
            var fileLocations = new List<string>();
            using var client = new HttpClient();

            using (var form = new MultipartFormDataContent())
            {
                var streamContent = new StreamContent(files[0].OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
                form.Add(streamContent, "audioFile", files[0].FileName);


                var response = await client.PostAsync($"http://localhost:5274/api/Audio/Upload?UserName={userName}", form);


                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);
                    fileLocations.Add(responseString);
                }
            }

            return fileLocations;
        }

        /// <summary>
        /// Posts all the info about the audio file
        /// </summary>
        /// <param name="fileLocations">where the files are placed at the backend</param>
        /// <returns>(bool) if everything worked</returns>
        private async Task<bool> SubmitFormDataAsync(List<string> fileLocations)
        {
            using var client = new HttpClient();

            // The original formdata that gets the info from the user
            //var formData = new
            //{
            //    Title,
            //    Tags,
            //    Description,
            //    AcceptsLicense,
            //    FileLocations = fileLocations
            //};

            // Some random info made by chatgpt that just fills the json
            var formData = new Audio
            {
                Name = "Sample Track",
                Hidden = false,
                Description = "This is a sample description.",
                Duration = 180, // Duration in seconds
                SampleRate = 44100, // Sample rate in Hz
                BitDepth = 24, // Bit depth
                FileSize = "10MB",
                UploadDate = DateTime.UtcNow,
                Downloads = 100,
                Format = "MP3",
                FilePlacement = fileLocations[0],
                UserID = 12345,
                Category = new List<Category>
            {
                new Category { Name = "Music" }
            },
                Genre = new List<Genre>
            {
                new Genre { Name = "Pop" }
            },
                Type = new Models.Type
                {
                    Name = "Song",
                    Channels = "Stereo",
                    AverageBPM = 120,
                    Key = "C Major"
                },
                Loudness = new Loudness
                {
                    PeakAmplitude = -1,
                    RmsLevel = -14.0
                },
                Instrument = new Instrument
                {
                    Name = "Guitar"
                },
                Mood = new Mood
                {
                    Name = "Happy"
                },
                Other = new Other
                {
                    Language = "English",
                    Copyright = "© 2024 Sample Company",
                    IsrcCode = "US-S1Z-99-00001",
                    UpceanCode = 123456789012
                },
                UsedIn = new List<UsedIn>
                {
                    new UsedIn { AudioID = 2 }
                },
                MadeOf = new List<MadeOf> 
                { 
                    new MadeOf {  AudioID = 2 } 
                }
            };

            // Converts the formdata into .json format and the sends it to the api
            var jsonContent = new StringContent(JsonSerializer.Serialize(formData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5274/api/Audio", jsonContent);

            return response.IsSuccessStatusCode;
        }
    }
}
