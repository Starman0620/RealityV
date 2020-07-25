using System;
using System.Drawing;

using GTA;
using GTA.UI;

using LemonUI;
using LemonUI.Menus;
using LemonUI.Items;
using RealityV.Util;
using RealityV.Modules;

namespace RealityV
{
    public class Main : Script
    {
        Configuration Config;



        // Debug stuff
#if DEBUG
        ContainerElement Container = new ContainerElement(new PointF(Screen.Width - 200, Screen.Height - 32), new SizeF(200, 32), Color.FromArgb(200, 200, 200, 200));
        TextElement Text = new TextElement("RealityV", new PointF(Screen.Width - 195, Screen.Height - 30), 0.30f, Color.Black, GTA.UI.Font.ChaletLondon);
        ObjectPool MainPool = new ObjectPool();
        NativeMenu Menu = new NativeMenu("RealityV Debug Menu", "Debug options");
        NativeItem DrainFuel = new NativeItem("Drain fuel", "Drains all of the fuel out of your vehicle");
        NativeItem FillFuel = new NativeItem("Fill fuel", "Maxes out your vehicles fuel");
        NativeItem Starve = new NativeItem("Starve", "Starves the player");
        NativeItem Eat = new NativeItem("Eat", "Maxes out the players hunger");
        NativeItem GoHomeless = new NativeItem("Go homeless", "Makes you homeless");
#endif


        public Main()
        {
            Config = Configuration.FromFile();

#if DEBUG
            int Build = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;
            Text.Caption = $"RealityV Debug Build {Build}\nDO NOT DISTRIBUTE";
            Menu.Add(DrainFuel);
            Menu.Add(FillFuel);
            Menu.Add(Starve);
            Menu.Add(Eat);
            Menu.Add(GoHomeless);
            Menu.UseMouse = false;
            MainPool.Add(Menu);
#endif

            // Initialize all of the modules
            if (Config.Modules.Fuel)
                Fuel.Initialize();
            if (Config.Modules.Bills)
                Bills.Initialize();
            if (Config.Modules.Homeless)
                Homeless.Initialize();
            if (Config.Modules.Hunger)
                Hunger.Initialize();
            if (Config.Modules.IncomeTax)
                IncomeTax.Initialize();
            if (Config.Modules.Jobs)
                Jobs.Initialize();

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
