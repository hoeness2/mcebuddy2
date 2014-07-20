using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MCEBuddy.Globals;
using MCEBuddy.Util;

namespace MCEBuddy.GUI
{
    public static class LocaliseForms
    {
        public static void LocaliseForm(Form sourceForm, ToolTip toolTip, ContextMenuStrip contextMenuStrip)
        {
            LocaliseForm(sourceForm, toolTip);
            foreach (ToolStripItem item in contextMenuStrip.Items)
            {
                item.Text = Localise.GetPhrase(item.Text);
                if (item.AutoToolTip)
                    item.ToolTipText = Localise.GetPhrase(item.ToolTipText);
            }
        }

        public static void LocaliseForm(Form sourceForm, ToolTip toolTip)
        {
            sourceForm.Text = Localise.GetPhrase(sourceForm.Text);
            LocaliseForms.LocaliseControl(sourceForm.Controls, toolTip);
        }

        private static void LocaliseControl(Control.ControlCollection ctl, ToolTip toolTip)
        {
            foreach (Control childCtl in ctl) // look for all child controls
            {
                //if (childCtl.GetType() != typeof(Button))
                //{
                //    float fontSize = 1;
                //    SizeF stringSize;
                //    do
                //    {
                //        fontSize += (float)0.25;
                //        stringSize = sourceForm.CreateGraphics().MeasureString(childCtl.Text,
                //                                                               new Font(childCtl.Font.Name, fontSize + (float)0.25));
                //    } while (stringSize.Width < childCtl.Width);
                //    childCtl.Font = ChangeFontSize(childCtl.Font, fontSize);
                //}
                toolTip.SetToolTip(childCtl, Localise.GetPhrase(toolTip.GetToolTip(childCtl)));
                if (((childCtl.GetType() == typeof(Label))) || ((childCtl.GetType() == typeof(GroupBox))) || ((childCtl.GetType() == typeof(CheckBox))) || ((childCtl.GetType() == typeof(Button))))
                {
                    childCtl.Text = Localise.GetPhrase(childCtl.Text);
                    LocaliseControl(childCtl.Controls, toolTip); // Localize recursively for nested controls
                }
            }
        }

        static public Font ChangeFontSize(Font font, float fontSize)
        {
            if (font != null)
            {
                float currentSize = font.Size;
                if (currentSize != fontSize)
                {
                    font = new Font(font.Name, fontSize,
                        font.Style, font.Unit,
                        font.GdiCharSet, font.GdiVerticalFont);
                }
            }
            return font;
        }
    }
}
