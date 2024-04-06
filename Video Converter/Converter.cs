using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using BepInEx;
using Ramune.CustomVideoSaveLocation;

namespace Video_Converter
{
    public class Converter : BaseUnityPlugin
    {
        
        public static void Init()
        {
            On.CameraRecording.SaveToDesktop += SaveToDesktop;
        }
        
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool SaveToDesktop(On.CameraRecording.orig_SaveToDesktop orig, CameraRecording self, out string videoFileName)
        {
            Plugin.Logger?.LogInfo("Beginning conversion process");
            bool originalResult = orig(self, out videoFileName);
            string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content Warning_Data\\StreamingAssets\\FFmpegOut\\Windows\\ffmpeg.exe");
            string inputFilePath;
            if (CustomVideoSaveLocation.Path.Value != "Desktop" && CustomVideoSaveLocation.Path.Value != null)
            {
                inputFilePath = CustomVideoSaveLocation.Path.Value;
                Plugin.Logger?.LogInfo("Custom path detected");
            }
            else
            {
                inputFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), videoFileName);
                Plugin.Logger?.LogInfo("Custom path not detected, using desktop");
            }
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