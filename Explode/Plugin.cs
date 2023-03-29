namespace Explode
{
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;
    using System;

    public class Plugin
    {
        public static Plugin Instance { get; private set; }
        public static Random Random = new Random();

        [PluginConfig("Configs/explode.yml")]
        public Config Config;

        [PluginPriority(LoadPriority.High)]
        [PluginEntryPoint("Explode","1.0.0","A Plugin that makes you explode while flipping coins", "holysilman")]
        void LoadPlugin()
        {
            Instance = this;
            Log.Info("Loaded plugin, register events...");
            EventManager.RegisterEvents<EventHandlers>(this);
            Log.Info($"Registered events");
            var handler = PluginHandler.Get(this);
            handler.SaveConfig(this, nameof(Config));
        }
    }
}
