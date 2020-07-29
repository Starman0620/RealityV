using System;
using System.Drawing;
using System.Collections.Generic;

using GTA;
using GTA.UI;

using LemonUI;
using LemonUI.Menus;

using RealityV.Util;
using RealityV.Modules;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace RealityV
{
    public class Main : Script
    {
        Configuration Config;
        List<Module> Modules = new List<Module>();


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
            if (!Directory.Exists(Globals.ModPath))
                Directory.CreateDirectory(Globals.ModPath);
            if (!Directory.Exists(Globals.ModulePath))
                Directory.CreateDirectory(Globals.ModulePath);
            if (!Directory.Exists(Globals.FuelPath))
                Directory.CreateDirectory(Globals.FuelPath);

            Config = Configuration.FromFile();
            // Initialize all of the necessary modules
            if (Config.Modules.Fuel)
            {
                Fuel FuelMod = new Fuel();
                DrainFuel.Activated += (object sender, EventArgs e) => {
                    FuelMod.DrainFuel(); };
                FillFuel.Activated += (object sender, EventArgs e) => {
                    FuelMod.FillFuel(); };
                Modules.Add(FuelMod);
            }
            if (Config.Modules.Bills)
                Modules.Add(new Bills());
            if (Config.Modules.Homeless)
                Modules.Add(new Homeless());
            if (Config.Modules.Hunger)
                Modules.Add(new Hunger());
            if (Config.Modules.IncomeTax)
                Modules.Add(new Tax());
            if (Config.Modules.Jobs)
                Modules.Add(new Jobs());
            foreach (Module Mod in Modules)
                Mod.Initialize();

#if DEBUG
            int Build = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;
            Text.Caption = $"RealityV Build {Build}\nDO NOT DISTRIBUTE";
            Menu.Add(DrainFuel);
            Menu.Add(FillFuel);
            //Menu.Add(Starve);
            //Menu.Add(Eat);
            //Menu.Add(GoHomeless);
            Menu.UseMouse = false;
            MainPool.Add(Menu);
#endif

            Tick += OnTick;
            Aborted += (object sender, EventArgs e) =>
            {
                foreach (Module Mod in Modules)
                    Mod.Abort();
            };
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

            foreach (Module Mod in Modules)
                Mod.Tick();
        }
    }
}
