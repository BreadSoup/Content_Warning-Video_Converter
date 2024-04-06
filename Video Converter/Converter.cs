﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Configuration;

namespace Video_Converter
{
    public static class Converter
    {
        public static void Init()
        {
            Plugin.Logger?.LogInfo("Converter initialized");
            On.CameraRecording.SaveToDesktop += SaveToDesktop;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool SaveToDesktop(On.CameraRecording.orig_SaveToDesktop orig, CameraRecording self, out string videoFileName)
        {
            Plugin.Logger?.LogInfo("Beginning conversion process");
            bool originalResult = orig(self, out videoFileName);
            string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content Warning_Data\\StreamingAssets\\FFmpegOut\\Windows\\ffmpeg.exe");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string inputFilePath;
                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.ramune.CustomVideoSaveLocation"))
                {
                    string configPath = Path.Combine(Paths.ConfigPath, "com.ramune.CustomVideoSaveLocation.cfg");
                    var otherModConfig = new ConfigFile(configPath, true);
                    var configDef = new ConfigDefinition("VideoSaveFolderPath", "Path to the folder to save your videos to");
                    var videoSaveFolderPath = otherModConfig.Bind<string>(configDef, "Desktop").Value;
                    
                    Plugin.Logger?.LogInfo(configPath);
                    Plugin.Logger?.LogInfo("CustomVideoSaveLocation: " + videoSaveFolderPath);
                    
                    if (videoSaveFolderPath != "Desktop" && videoSaveFolderPath != null)
                    {
                        inputFilePath = Path.Combine(videoSaveFolderPath, $"{videoFileName}");
                        Plugin.Logger?.LogInfo("Custom path detected");
                    }
                    else
                    {
                        inputFilePath = Path.Combine(desktopPath, videoFileName);
                        Plugin.Logger?.LogInfo("Custom path not detected, using desktop");
                    }
                }
                else
                {
                    inputFilePath = Path.Combine(desktopPath, videoFileName);
                }
            

            string outputFilePath = Path.ChangeExtension(inputFilePath, ".mp4"); // changes the extension to .mp4
            string arguments = $"-i \"{inputFilePath}\" \"{outputFilePath}\"";

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
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), videoFileName)))
                {
                    Plugin.Logger?.LogInfo("Deleting WEBM that wasn't cleaned up by CustomVideoSaveLocation");
                    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), videoFileName));
                }
            }
            else
            {
                Plugin.Logger?.LogError("Conversion failed :(");
            }

            return originalResult;
        }
    }
}