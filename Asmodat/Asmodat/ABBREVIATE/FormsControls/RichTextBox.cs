using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using System.Drawing;

using System.Reflection;

using System.Data;


namespace Asmodat.Abbreviate
{
    public partial class FormsControls
    {
        public static void AppendText(RichTextBox RTBox, string text, Color color, int maxLines, bool scroll = false)
        {
            RTBox.Invoke((MethodInvoker)(() =>
            {
                RTBox.SelectionStart = RTBox.TextLength;
                RTBox.SelectionLength = 0;
                RTBox.SelectionColor = color;
                RTBox.AppendText(text);
                RTBox.SelectionColor = RTBox.ForeColor;

                int iSelectionSave = RTBox.SelectionStart;
                while (RTBox.Lines.Length > maxLines)
                {
                    RTBox.SelectionStart = 0;
                    RTBox.SelectionLength = RTBox.Text.IndexOf("\n", 0) + 1;
                    RTBox.SelectedText = "";
                }

                if (scroll)
                {
                    RTBox.SelectionStart = RTBox.Text.Length;
                    RTBox.ScrollToCaret();
                }
            }));
        }
    }
}
