using BepInEx;
using BepInEx.Logging;
using JetBrains.Annotations;

namespace Video_Converter
{
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    [BepInDependency("com.bepinex.plugin.important")]
    public class Plugin : BaseUnityPlugin
    {
        private const string ModGuid = "breadsoup.Video_Converter";
        private const string ModName = "Video Converter";
        private const string ModVersion = "1.1.0";
        [UsedImplicitly] public new static ManualLogSource? Logger;
        //private const bool Devmode = true; // Set to false when releasing
        

        private void Awake()
        {
            Converter.Init();
            //On.ExtractVideoMachine.CheckState += CheckState; kinda useless and broken for now
        }
        
        /*private void CheckState(On.ExtractVideoMachine.orig_CheckState orig, ExtractVideoMachine self)
        {
            if (Devmode)
            {
                SurfaceNetworkHandler.ReturnFromLostWorld();
                //need to figure out a way to get the video extractor to open and not break
                UploadVideoStation.FindObjectsOfType<UploadVideoStation>()[0].Unlock();
            }
            orig(self);
        }*/
    }
}