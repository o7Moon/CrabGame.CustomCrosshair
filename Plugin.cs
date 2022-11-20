using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Crosshair = MonoBehaviourPublicRedoleReritoboReBoenUnique;
using GameUI = MonoBehaviourPublicGaroloGaObInCacachGaUnique;

namespace CustomCrosshair
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        static Plugin instance;
        ConfigEntry<bool> enableDot;
        ConfigEntry<int> dotSize;
        ConfigEntry<int> gapSize;
        ConfigEntry<int> length;
        ConfigEntry<int> width;
        ConfigEntry<string> color;
        public override void Load()
        {
            instance = this;
            enableDot = Config.Bind<bool>(
                "Crosshair",
                "enableDot",
                false,
                "if enabled, displays a dot in the center of the crosshair");
            dotSize = Config.Bind<int>(
                "Crosshair",
                "dotSize",
                2,
                "the size of the dot, if enabled");
            gapSize = Config.Bind<int>(
                "Crosshair",
                "gapSize",
                6,
                "the distance between the bars and the dot");
            length = Config.Bind<int>(
                "Crosshair",
                "length",
                10,
                "the length of the bars");
            width = Config.Bind<int>(
                "Crosshair",
                "width",
                4,
                "the width of the bars. set to 0 to disable bars");
            color = Config.Bind<string>(
                "Crosshair",
                "color",
                "#FFFFFF",
                "the color of the crosshair"
            );
            Harmony.CreateAndPatchAll(typeof(Plugin));
            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
        [HarmonyPatch(typeof(GameUI),nameof(GameUI.Start))]
        [HarmonyPostfix]
        public static void AfterGameUIStart(GameUI __instance){
            Crosshair cross = __instance.transform.FindChild("Crosshair").GetComponent<Crosshair>();
            instance.Config.Reload();
            
            cross.enableDot = instance.enableDot.Value;
            
            cross.dotSize = instance.dotSize.Value;
            
            cross.gap = instance.gapSize.Value;
            
            cross.length = instance.length.Value;
            
            cross.width = instance.width.Value;

            Color color = new Color(1,1,1,1);
            ColorUtility.TryParseHtmlString(instance.color.Value, out color);
            cross.transform.GetChild(0).GetComponent<RawImage>().color = color;
            // the bars are children 1-4, and they have another nested gameobject with the rawimage
            for (int i = 1; i < 5; i++){
                cross.transform.GetChild(i).GetChild(0).GetComponent<RawImage>().color = color;
            }
        }
    }
}
