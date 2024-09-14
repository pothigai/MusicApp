using System;
using System.IO;
using System.Media;
using System.Text.Json;
using System.Diagnostics;

namespace MusicApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jsonFilePath = @"..\..\..\metadata.json";

            Console.WriteLine("Enter the YouTube URL: ");

            string InputURL = Console.ReadLine();
            string outputFile = "downloaded_audio.wav";
            TrackInfo trackInfo = LoadTrackInfoFromJsonFile(jsonFilePath);

            if (trackInfo != null)
            {
                DisplayTrackInfo(trackInfo);
            }
            else
            {
                Console.WriteLine("Failed to load track info.");
            }

            Audio(InputURL, outputFile);
            PlayAudio(outputFile);
        }

        static void Audio(string url, string outputFile)
        {
            string command = $"-x --audio-format wav -o \"{outputFile}\" {url}";
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading audio: " + ex.Message);
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
        public static TrackInfo LoadTrackInfoFromJsonFile(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);

                using (JsonDocument document = JsonDocument.Parse(jsonString))
                {
                    string title = document.RootElement.GetProperty("title").GetString();
                    string artist = document.RootElement.GetProperty("uploader").GetString();
                    string url = document.RootElement.GetProperty("uploader_url").GetString();
                    double durationInSeconds = document.RootElement.GetProperty("duration").GetDouble();
                    TimeSpan duration = TimeSpan.FromSeconds(durationInSeconds);
                    string dateString = document.RootElement.GetProperty("upload_date").GetString();
                    DateOnly date = DateOnly.ParseExact(dateString, "yyyyMMdd");
                    string uniqueId = document.RootElement.GetProperty("id").GetString();
                    string thumbnail = document.RootElement.GetProperty("thumbnail").GetString();

                    return new TrackInfo
                    {
                        Title = title,
                        Artist = artist,
                        Duration = duration,
                        Url = url,
                        Date = date,
                        UniqueId = uniqueId,
                        Thumbnail = thumbnail
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
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