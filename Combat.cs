using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ServerSync;

namespace Combat
{
    [BepInPlugin(PluginGUID, PluginGUID, Version)]
    public class Combat : BaseUnityPlugin
    {
        public const string PluginGUID = "Detalhes.Combat";
        public const string Name = "Combat";
        public const string Version = "1.0.0";

        ConfigSync configSync = new ConfigSync("Detalhes.Combat") { DisplayName = "Combat", CurrentVersion = Version, MinimumRequiredVersion = Version };

        Harmony _harmony = new Harmony(PluginGUID);

        public static ConfigEntry<int> BuffSeconds;
        public static ConfigEntry<bool> ActivateOnlyForPVP;
        public static ConfigEntry<bool> PreventTeleport;

        public void Awake()
        {
            BuffSeconds = config("Server config", "BuffSeconds", 30,
                   new ConfigDescription("BuffSeconds", null));

            ActivateOnlyForPVP = config("Server config", "ActivateOnlyForPVP", false,
                new ConfigDescription("ActivateOnlyForPVP", null));

            PreventTeleport = config("Server config", "PreventTeleport", true,
             new ConfigDescription("PreventTeleport", null));
            _harmony.PatchAll();
        }

        ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

    }
}
