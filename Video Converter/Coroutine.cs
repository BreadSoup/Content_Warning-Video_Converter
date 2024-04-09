using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Video_Converter
{
    public class CoroutineStarter : MonoBehaviour
    {
        public static CoroutineStarter? Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public IEnumerator WaitForFile(string outputFilePath, int timeoutInSeconds, string ffmpegPath, string arguments, string inputFilePath, string videoFileName)
        {
            Plugin.Logger?.LogInfo("Beginning coroutine");
            int elapsed = 0;
            
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
            else if (elapsed >= timeoutInSeconds)
            {
                Plugin.Logger?.LogError("Conversion timed out");
            }
            else
            {
                Plugin.Logger?.LogError("Conversion Failed");
            }
            yield return null;
        }
    }
}