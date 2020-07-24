using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GTA;
using GTA.Native;
using GTA.UI;

namespace RealityV
{
    public class Main : Script
    {
#if DEBUG
        ContainerElement Container = new ContainerElement(new PointF(Screen.Width - 200, Screen.Height - 32), new SizeF(200, 32), Color.FromArgb(200, 200, 200, 200));
        TextElement Text = new TextElement("RealityV", new PointF(Screen.Width - 195, Screen.Height - 30), 0.30f, Color.Black, GTA.UI.Font.ChaletLondon);
#endif

        public Main()
        {
#if DEBUG
            int Build = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;
            Text.Caption = $"RealityV Debug Build {Build}\nDO NOT DISTRIBUTE";
#endif
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
#if DEBUG
            Container.Draw();
            Text.Draw();
#endif
        }
    }
}
