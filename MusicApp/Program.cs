using System;
using System.IO;
using System.Media;
using System.Text.Json;
using System.Diagnostics;
using System.IO.Enumeration;
using MusicApp;

namespace MusicApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Console.Write("Enter a search term: ");
            string searchTerm = Console.ReadLine();

            UserSearch searcher = new UserSearch();
            string InputURL = await searcher.DisplayResults(searchTerm);

            if (!string.IsNullOrEmpty(InputURL))
            {
                Console.Write($"This is your selected URL: {InputURL}\n");
            }

            string downloadPath = DownloadAudio(InputURL);
            string jsonMetadata = GetYouTubeMetadata(InputURL);
            TrackInfo trackInfo = LoadTrackInfoFromJsonString(jsonMetadata);

            if (trackInfo != null)
            {
                DisplayTrackInfo(trackInfo);
                PlayAudio(downloadPath);
            }
            else
            {
                Console.WriteLine("Failed to load track info.");
            }

        }

        static string DownloadAudio(string url)
        {
            string outputTemplate = "%(title)s.%(ext)s";
            string command = $"-x --audio-format wav -o \"{outputTemplate}\" {url}";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "yt-dlp",
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine("yt-dlp error: ");
                        Console.WriteLine(error);
                    }

                    string downloadedFile = GetDownloadedFilePath(url);
                    return downloadedFile;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading audio: " + ex.Message);
                return null;
            }

        }

        static string GetDownloadedFilePath(string url)
        {
            string command = $"-e {url}";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "yt-dlp",
                Arguments = command,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(psi))
                {
                    string title = process.StandardOutput.ReadToEnd().Trim();
                    process.WaitForExit();
                    string fileName = $"{title}.wav";
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:" + ex.Message);
                return null;
            }
        }
        static void PlayAudio(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    using (SoundPlayer player = new SoundPlayer(filePath))
                    {
                        Console.WriteLine("Playing audio....Press any key to stop");
                        player.Play();
                        Console.ReadKey();
                        player.Stop();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error playing audioL: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("File not found: " + filePath);
            }
        }

        static string GetYouTubeMetadata(string url)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "yt-dlp",
                Arguments = $"--dump-json {url}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output;
            }
        }

        public static TrackInfo LoadTrackInfoFromJsonString(string jsonString)
        {
            try
            {
                using (JsonDocument document = JsonDocument.Parse(jsonString))
                {
                    string title = document.RootElement.GetProperty("title").GetString();
                    string artist = document.RootElement.GetProperty("uploader").GetString();
                    string url = document.RootElement.GetProperty("uploader_url").GetString();
                    double durationInSeconds = document.RootElement.GetProperty("duration").GetDouble();
                    TimeSpan duration = TimeSpan.FromSeconds(durationInSeconds);
                    string dateString = document.RootElement.GetProperty("upload_date").GetString();
                    DateOnly uploadDate = DateOnly.ParseExact(dateString, "yyyyMMdd", null);
                    string uniqueId = document.RootElement.GetProperty("id").GetString();
                    string thumbnail = document.RootElement.GetProperty("thumbnail").GetString();

                    return new TrackInfo
                    {
                        Title = title,
                        Artist = artist,
                        Duration = duration,
                        Url = url,
                        Date = uploadDate,
                        UniqueId = uniqueId,
                        Thumbnail = thumbnail
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JSON metadata: {ex.Message}");
                return null;
            }
        }

        public static void DisplayTrackInfo(TrackInfo track)
        {
            Console.WriteLine("Track Information:");
            Console.WriteLine($"Title: {track.Title}");
            Console.WriteLine($"Artist: {track.Artist}");
            Console.WriteLine($"Duration: {track.Duration}");
            Console.WriteLine($"URL: {track.Url}");
            Console.WriteLine($"Unique ID: {track.UniqueId}");
            Console.WriteLine($"Thumbnail: {track.Thumbnail}");
            Console.WriteLine($"Date: {track.Date}");
        }
    }
}