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

            if (!isSuccess.Contains("Error"))
            {
                // Redirect to success page or display success message
                return RedirectToPage("/Success");
            }

            ModelState.AddModelError("", "Failed to submit form data.");
            return Page();
        }

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

        private async Task<string> SubmitFormDataAsync(List<string> fileLocations)
        {
            using var client = new HttpClient();

            var formData = new Audio
            {
                ////Title,
                ////Tags,
                ////Description,
                ////AcceptsLicense,
                ////FileLocations = fileLocations

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
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(formData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5274/api/Audio", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                // Log the response details for debugging
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response Status: " + response.StatusCode);
                Console.WriteLine("Response Content: " + responseContent);
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
