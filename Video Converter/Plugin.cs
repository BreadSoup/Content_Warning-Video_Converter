using BepInEx;

namespace Infinite_Camera
{
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string ModGuid = "breadsoup.Infinite_Camera";
        private const string ModName = "Infinite Camera";
        private const string ModVersion = "1.0.0";

        private void Awake()
        {
            On.VideoCamera.Update += VideoCamera_Update;
        }

        private void VideoCamera_Update(On.VideoCamera.orig_Update orig, VideoCamera self)
        {
            orig(self);
            self.m_recorderInfoEntry.timeLeft = self.m_recorderInfoEntry.maxTime;
        }
    }
}