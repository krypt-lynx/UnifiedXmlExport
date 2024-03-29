﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace UnifiedXmlExport
{
    public enum MessageType
    {
        Verbose,
        Message,
        Warning,
        Error,
    }

    public static class LogHelper
    {
        public static void Log(this string str, MessageType type = MessageType.Verbose)
        {
            str = $"[UXE] {str}";
            switch (type)
            {
                case MessageType.Verbose:
                    if (Mod.Debug)
                    {
                        Verse.Log.Message(str);
                    }
                    return;
                case MessageType.Message:
                    Verse.Log.Message(str);
                    return;
                case MessageType.Warning:
                    Verse.Log.Warning(str);
                    return;
                case MessageType.Error:
                    Verse.Log.Error(str);
                    return;
            }
        }
    }
}
