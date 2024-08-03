using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp
{
    public class DataStructure
    {
        public class UserSearch
        {
            public string Query { get; set; }

            public UserSearch(string query)
            {
                Query = query;
            }
        }
        public class SearchResult
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string Thumbnail { get; set; }
            public int Duration { get; set; }
            public string UniqueId { get; set; }
            public string Date { get; set; }

            public SearchResult(string title, string url, string thumbnail, int duration, string uniqueId, string date)
            {
                Title = title;
                Url = url;
                Thumbnail = thumbnail;
                Duration = duration;
                UniqueId = uniqueId;
                Date = date;
            }

        }
        public class DownloadedAudio
        {
            public string AudioFormat { get; set; }
            public string AudioUrl { get; set; }

            public DownloadedAudio(string audioFormat, string audioUrl)
            {
                AudioFormat = audioFormat;
                AudioUrl = audioUrl;
            }
        }

        public class PlayerStatus
        {
            public SearchResult Track { get; set; }
            public int PlaybackTime { get; set; }
            public string AudioStatus { get; set; }

            public PlayerStatus(SearchResult track, int playbackTime = 0, string audioStatus = "stopped")
            {
                Track = track;
                PlaybackTime = playbackTime;
                AudioStatus = audioStatus;
            }

        }
        public class Player
        {
            public PlayerStatus CurrentStatus { get; set; }

            public void Play(SearchResult track)
            {
                CurrentStatus = new PlayerStatus(track, audioStatus: "playing");
            }

            public void Pause()
            {
                if (CurrentStatus != null && CurrentStatus.AudioStatus == "playing")
                {
                    CurrentStatus.AudioStatus = "paused";
                }
            }

            public void Resume()
            {
                if (CurrentStatus != null && CurrentStatus.AudioStatus == "paused")
                {
                    CurrentStatus.AudioStatus = "playing";
                }
            }

            public void Stop()
            {
                if (CurrentStatus != null)
                {
                    CurrentStatus.AudioStatus = "stopped";
                }
            }

            public PlayerStatus GetCurrentPlayback()
            {
                return CurrentStatus;
            }
        }
    }
}
