using Cassowary;
using RimWorld;
using RWLayout.alpha2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace UnifiedXmlExport
{

    class SettingsView : CElement
    {
        public SettingsView() : base()
        {
            CElement inputPanel = AddElement(new CElement());
            CElement defaultBtn;
            inputPanel.StackLeft(inputPanel.AddElement(new CWidget
            {
                DoWidgetContent = (_, bounds) =>
                {
                    Mod.Settings.xmlExportPath = Widgets.TextField(bounds, Mod.Settings.xmlExportPath);
                }
            }),
            2,
            (inputPanel.AddElement(defaultBtn = new CButtonText
            {
                Title = "Use default path",
                Action = (_) =>
                {
                    Mod.Settings.xmlExportPath = Settings.xmlExportPathDefault;
                }
            }), defaultBtn.intrinsicWidth));


            this.StackTop(StackOptions.Create(constrainEnd: false, intrinsicIfNotSet: true),
                AddElement(new CLabel
                {
                    Title = "XML export path:"
                }), 
                2,
                (inputPanel, 28),
                10,
                AddElement(new CCheckboxLabeled
                {
                    Title = "Indent output xml",
                    Checked = Mod.Settings.indent,
                    Changed = (_, value) => Mod.Settings.indent = value,
                })
                );
        }

    }

}
