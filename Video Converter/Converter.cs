using System;
using System.Diagnostics;
using System.IO;
using BepInEx;

namespace Video_Converter
{
    public class Converter : BaseUnityPlugin
    {
        public static void Init()
        {
            On.CameraRecording.SaveToDesktop += SaveToDesktop;
        }
        
        private static bool SaveToDesktop(On.CameraRecording.orig_SaveToDesktop orig, CameraRecording self, out string videoFileName)
        {
            bool originalResult = orig(self, out videoFileName);
            string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content Warning_Data\\StreamingAssets\\FFmpegOut\\Windows\\ffmpeg.exe");
            string inputFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), videoFileName);
            string outputFilePath = Path.ChangeExtension(inputFilePath, ".mp4"); // changes the extension to .mp4
            string arguments = $"-i {inputFilePath} {outputFilePath}";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }

            if (File.Exists(outputFilePath))
            {
                Plugin.Logger?.LogInfo("Conversion successful deleting WEBM");
                File.Delete(inputFilePath);
            }
            else
            {
                Plugin.Logger?.LogError("Conversion failed :(");
            }

            return originalResult;
        }
    }
}