using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Models;
using Newtonsoft.Json;


namespace BusinessLogic
{
    public class AudioMetaDataLink
    {
        public async Task<Audio> AudioExtract(Audio audio)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the API URL
                string url = "http://localhost:5000/audio-info";

                // Create the JSON object with the required data
                var data = new
                {
                    file_path = audio.FilePlacement
                };

                // Serialize the object to a JSON string
                string json = JsonConvert.SerializeObject(data);

                // Create the content to be sent in the request
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    // Send the POST request
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Ensure the request was successful
                    response.EnsureSuccessStatusCode();

                    // Read the response content
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var audioData = JsonConvert.DeserializeObject<MetaDataLink>(responseContent);

                    if (audioData.samplerate != 0 && audio.SampleRate == 0)
                    {
                        audio.SampleRate = audioData.samplerate;
                    }

                    if (audioData.duration != 0 && audio.Duration == 0)
                    {
                        audio.Duration = audioData.duration;
                    }

                    if (audioData.duration != 0 && audio.Duration == 0)
                    {
                        audio.Duration = audioData.duration;
                    }

                    if (audioData.filesize != 0 && audio.FileSize == "")
                    {
                        audio.FileSize = audioData.filesize.ToString();
                    }

                    return audio;
                }
                catch (HttpRequestException e)
                {
                    return audio;
                }
            }
        }
    }
}
