using HarmonyLib;
using RimWorld;
using RWLayout.alpha2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace UnifiedXmlExport
{
    public class Settings : ModSettings
    {
        public bool debug = false;
        public const string xmlExportPathDefault = @"./Mods/Unified.xml";
        public string xmlExportPath = xmlExportPathDefault;
        public bool indent = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref debug, "debug", false);
            Scribe_Values.Look(ref xmlExportPath, "xmlExportPath", xmlExportPathDefault);
            Scribe_Values.Look(ref indent, "indent", true);

            base.ExposeData();
        }
    }

    public class Mod : CMod
    {
        public static string PackageIdOfMine = null;
        public static Settings Settings { get; private set; }

        private static bool debug = false;
        public static bool Debug
        {
            get
            {
                return Settings.debug;
            }
        }

        public static string CommitInfo = null;

        public static Verse.Mod Instance = null;

        public static Action ActiveTablesChanged = null;

        public Mod(ModContentPack content) : base(content)
        {
            ReadModInfo(content);
            Settings = GetSettings<Settings>();
            Instance = this;

            Harmony harmony = new Harmony(PackageIdOfMine);

            ApplyPatches(harmony);
        }

        private static void ApplyPatches(Harmony harmony)
        {
            LoadedModManagerPatches.Patch(harmony);
        }

        private static void ReadModInfo(ModContentPack content)
        {
            PackageIdOfMine = content.PackageId;

            var name = Assembly.GetExecutingAssembly().GetName().Name;

            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream(name + ".git.txt"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    CommitInfo = reader.ReadToEnd()?.TrimEndNewlines();
                }
            }
            catch
            {
                CommitInfo = null;
            }

            debug = PackageIdOfMine.EndsWith(".dev");
        }

        public override string SettingsCategory()
        {
            return "Unified Xml Export";
        }

        public override string Version()
        {
            return CommitInfo;
        }

        public override CElement CreateSettingsView()
        {
            return new SettingsView();
        }
    }


}
