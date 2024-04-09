using BepInEx;
using BepInEx.Logging;

namespace Video_Converter
{
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    [BepInDependency("com.ramune.CustomVideoSaveLocation", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        private const string ModGuid = "breadsoup.Video_Converter";
        private const string ModName = "Video Converter";
        private const string ModVersion = "1.1.2";
        internal new static ManualLogSource? Logger { get; private set; }
        private const bool Devmode = true; // Set to false when releasing

        private void Awake()
        {
            Logger = base.Logger;
            
            Converter.Init();
            On.SurfaceNetworkHandler.RPCM_StartGame += RPCM_StartGame;
        }


        private void RPCM_StartGame(On.SurfaceNetworkHandler.orig_RPCM_StartGame orig, SurfaceNetworkHandler self)
        {
            if (Devmode)
            {
                SurfaceNetworkHandler.ReturnFromLostWorld();
                ExtractVideoMachine.FindObjectsOfType<ExtractVideoMachine>()[0].Awake();
                UploadVideoStation.FindObjectsOfType<UploadVideoStation>()[0].Awake();
            }
            orig(self);
        }
    }
}