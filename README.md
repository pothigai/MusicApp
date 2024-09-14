
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

   You can now run the code and it should take a YouTube URL as an input and play audio and display relevant information about the audio.
   For now you will still need to download the metadata.JSON file into the project folder.
