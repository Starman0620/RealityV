using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GTA;
using GTA.Native;
using GTA.UI;

using LemonUI;
using LemonUI.Menus;
using LemonUI.Items;

namespace RealityV
{
    public class Main : Script
    {
#if DEBUG
        ContainerElement Container = new ContainerElement(new PointF(Screen.Width - 200, Screen.Height - 32), new SizeF(200, 32), Color.FromArgb(200, 200, 200, 200));
        TextElement Text = new TextElement("RealityV", new PointF(Screen.Width - 195, Screen.Height - 30), 0.30f, Color.Black, GTA.UI.Font.ChaletLondon);
        ObjectPool MainPool = new ObjectPool();
        NativeMenu Menu = new NativeMenu("Debug Menu", "Debug options");
        NativeItem GoHomeless = new NativeItem("Go homeless");
#endif


        public Main()
        {
#if DEBUG
            int Build = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;
            Text.Caption = $"RealityV Debug Build {Build}\nDO NOT DISTRIBUTE";
            Menu.Add(GoHomeless);
            Menu.UseMouse = false;
            MainPool.Add(Menu);
#endif
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
#if DEBUG
            Container.Draw();
            Text.Draw();
            MainPool.Process();
            if (Game.IsControlPressed(Control.Context) && Game.IsControlPressed(Control.Sprint) && !Menu.Visible)
                Menu.Visible = !Menu.Visible;
#endif
        }
    }
}
