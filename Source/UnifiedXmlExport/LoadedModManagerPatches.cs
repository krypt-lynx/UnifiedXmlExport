using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Verse;

namespace UnifiedXmlExport
{
    static class LoadedModManagerPatches
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(AccessTools.Method(typeof(LoadedModManager), "ParseAndProcessXML"),
                prefix: new HarmonyMethod(typeof(LoadedModManagerPatches), "ParseAndProcessXML_prefix"),
                postfix: new HarmonyMethod(typeof(LoadedModManagerPatches), "ParseAndProcessXML_postfix"));

        }

        static void ParseAndProcessXML_prefix(XmlDocument xmlDoc, ref XmlDocument __state)
        {
            __state = xmlDoc;
        }

        static void ParseAndProcessXML_postfix(XmlDocument __state)
        {
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.Unicode,
                Indent = Mod.Settings.indent
            };

            try
            {
                using (var writer = XmlWriter.Create(Mod.Settings.xmlExportPath, settings))
                {
                    __state.WriteContentTo(writer);
                    $"Unified xml files saved at {Path.GetFullPath(Mod.Settings.xmlExportPath)}".Log(MessageType.Message);
                }
            } 
            catch (Exception e)
            {
                LogException("Exception thrown during xml export", e).Log(MessageType.Error);
            }
        }

        static string LogException(string intro, Exception e)
        {
            var result = $"{intro}: {e.GetType().Name}: {e.Message}\n";
            if (e.InnerException != null)
            {
                result += LogException("Inner Exception", e.InnerException);
            }
            return result;
        }
    }
}
