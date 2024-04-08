using System;
using System.Collections;
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

        public IEnumerator WaitForFile(string outputFilePath, int timeoutInSeconds)
        {
            int elapsed = 0;

            while (!File.Exists(outputFilePath) && elapsed < timeoutInSeconds)
            {
                yield return new WaitForSeconds(1); // Wait for 1 second
                elapsed++; // Increment the elapsed time
            }
        }
    }
}