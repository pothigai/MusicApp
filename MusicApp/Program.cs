using System;
using System.IO;
using System.Media;
using System.Text.Json;

namespace MusicApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jsonFilePath = @"..\..\..\metadata.json";
            string audioFilePath = @"..\..\..\Rick Astley - Never Gonna Give You Up (Official Music Video).wav";

            TrackInfo trackInfo = LoadTrackInfoFromJsonFile(jsonFilePath);

            if (trackInfo != null)
            {
                DisplayTrackInfo(trackInfo);
                Player player = new Player();
                player.Play(trackInfo);
                PlayAudio(audioFilePath, player);
            }
            else
            {
                Console.WriteLine("Failed to load track info.");
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

        public static void PlayAudio(string audioFilePath, Player player)
        {
            if (File.Exists(audioFilePath))
            {
                using (var soundPlayer = new SoundPlayer(audioFilePath))
                {
                    soundPlayer.Play();
                    Console.WriteLine("Playing... Press any key to force close.");
                    Console.ReadKey();
                    soundPlayer.Stop();
                }
            }

            else
            {
                Console.WriteLine("Audio file not found.");
            }
            player.Stop();
        }
    }
}