using Microsoft.EntityFrameworkCore;
using DBAccess;
using Models;
using System.Net.Http.Headers;

string backupuser = "test";
string? user;
var filePath = "C://Test/yell1.mp3"; // Change this to the path of your MP3 file

Console.WriteLine("Testing Upload/Download");
Console.ReadLine();
Console.Clear();

Console.WriteLine("Skriv navn på bruger");
user = Console.ReadLine();
if (user == "" || user == null) { user = backupuser; }
Console.WriteLine("Skriv hvilken nummer fil der skal uploades 1-30");
try
{
    int x = Convert.ToInt32(Console.ReadLine());
    if (File.Exists($"C://Test/yell{x}.mp3"))
    {
        filePath = $"C://Test/yell{x}.mp3";
    }
}
catch (Exception)
{
    throw;
}
Console.Clear();

Console.WriteLine("Testing Upload");
var response = await UploadFile(user, filePath);
Console.ReadLine();
Console.Clear();

Console.WriteLine("Testing Download");
await DownloadFile(response);
Console.ReadLine();

static async Task<string> UploadFile(string userName, string filePath)
{
    var uploadUrl = $"http://localhost:5274/api/Audio/Upload?UserName={userName}"; // Change this to your upload endpoint URL

    if (!File.Exists(filePath))
    {
        Console.WriteLine("File not found.");
        return "error";
    }

    using var httpClient = new HttpClient();
    using var form = new MultipartFormDataContent();
    using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
    using var streamContent = new StreamContent(fileStream);
    streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");

    // Add the audio file with the field name 'audioFile'
    form.Add(streamContent, "audioFile", Path.GetFileName(filePath));


    try
    {
        var response = await httpClient.PostAsync(uploadUrl, form);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("File uploaded successfully.");
            Console.WriteLine("Response: " + responseContent);
            return responseContent;
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Error: " + responseContent);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception: " + ex.Message);
    }
    return "error";
}

static async Task DownloadFile(string placement)
{
    string[] strings = placement.Split('\\');
    var downloadUrl = $"http://localhost:5274/api/Audio/download/{strings[1]}?fileowner={strings[0]}"; // Change this to your download endpoint URL and filename
    var savePath = $"C://Test/{placement}"; // Change this to where you want to save the downloaded file

    if (!Directory.Exists($"C://Test/{strings[0]}"))
    {
        Directory.CreateDirectory($"C://Test/{strings[0]}");
    }

    using var httpClient = new HttpClient();



    try
    {
        var response = await httpClient.GetAsync(downloadUrl);

        if (response.IsSuccessStatusCode)
        {
            await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            await response.Content.CopyToAsync(fileStream);
            Console.WriteLine("File downloaded successfully and saved to " + savePath);
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Error: " + responseContent);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception: " + ex.Message);
    }
}
