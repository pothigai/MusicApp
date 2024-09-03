using System;
using System.IO;
using System.Media;

namespace MusicApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string textFilePath = @"..\..\..\condensed_data.txt";
            string audioFilePath = @"..\..\..\Rick Astley - Never Gonna Give You Up (Official Music Video).wav";

            TrackInfo trackInfo = LoadTrackInfoFromTextFile(textFilePath);

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

        public static TrackInfo LoadTrackInfoFromTextFile(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length < 7) 
                {
                    Console.WriteLine("The text file does not contain enough information.");
                    return null;
                }

                return new TrackInfo
                {
                    Title = lines[0],
                    Artist = lines[1], 
                    Duration = TimeSpan.FromSeconds(double.Parse(lines[2])),
                    Url = lines[3],
                    Date = DateOnly.ParseExact(lines[4], "yyyyMMdd"),
                    UniqueId = lines[5],
                    Thumbnail = lines[6]
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading text file: {ex.Message}");
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