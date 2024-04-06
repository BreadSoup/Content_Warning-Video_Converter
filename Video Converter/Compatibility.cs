namespace Video_Converter
{
    public static class CustomLocationCompatibility
    {
        private static bool? _enabled;

        public static bool Enabled
        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.ramune.CustomVideoSaveLocation");
                }

                return (bool)_enabled;
            }
        }
    }
}