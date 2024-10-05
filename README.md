
# MusicApp

## Chapters

1. Installing the yt-dlp library
 - You can install the yt-dlp library from this github [link](https://github.com/yt-dlp/yt-dlp/releases/tag/2024.08.06).
 - And then make sure you add it to your system path variables.
 - Now you can use the yt=dlp commands from your command window.
 - You also need to make sure you have ffmpeg downloaded as well which is used for audio processing and conversion. For windows you can download the full release build from this [link](https://www.gyan.dev/ffmpeg/builds/).
 - Extract it to a suitable location and make sure you add this as well to your system path variables.
2. Installing System.Windows.Extensions
 - You need this extention to use windows extensions on VS to play audio through the console.
 - Click on tools in the top bar in VS.
 - Click on solution explorer.
 - Select manage NuGet packages.
 - Search for System.Windows.Extensions and install it.
3. Installing the Google API client library for .NET
 - In visual studio, under your Package Manager Console, enter the following command:
   Install-Package Google.Apis.YouTube.v3
 - Once it's done installing, you should be able to access Google APIs for the search results.

   You can now run the code and enter a search key as an input. The code then uses this search key and Google API to display 50 results. You then enter the desired result number and the code will play audio and display relevant information about the audio.

