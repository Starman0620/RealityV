using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using GTA;
using GTA.Math;
using GTA.UI;

using RealityV.Util;

namespace RealityV.Modules
{
    internal class Fuel : Module
    {
        List<FuelVeh> FuelVehicles = new List<FuelVeh>();
        List<Blip> Blips = new List<Blip>();
        FuelVeh CurrentVehicle;
        ContainerElement FuelBarBackground = new ContainerElement(new PointF(225, Screen.Height - 139), new SizeF(15, 128), Color.FromArgb(190, 30, 30, 30));
        ContainerElement FuelBar = new ContainerElement(new PointF(226, Screen.Height - 137.5f), new SizeF(13, 125), Color.FromArgb(200, 128, 128, 0), false);
        List<Vector3> GasStations = new List<Vector3>()
        {
            new Vector3(1211.756f, -1407.93f, 34.67278f),
            new Vector3(-1434.403f, -282.7147f, 45.72723f),
            new Vector3(-74.86875f, -1755.578f, 29.11153f),
            new Vector3(1183.869f, -327.3409f, 68.69455f),
            new Vector3(614.7054f, 273.5178f, 102.6092f),
            new Vector3(2590.615f, 358.7582f, 107.9775f),
            new Vector3(1686.468f, 4930.328f, 41.59781f),
            new Vector3(-2557.833f, 2331.427f, 32.5805f),
            new Vector3(-1806.12f, 801.3314f, 138.0324f),
            new Vector3(2007.822f, 3778.583f, 31.70061f),
            new Vector3(1704.573f, 6412.813f, 32.277f),
            new Vector3(-95.43013f, 6415.329f, 31.00079f),
            new Vector3(174.8102f, 6603.814f, 31.36695f),
            new Vector3(153.1613f, 6628.985f, 31.23245f),
            new Vector3(263.78f, 2604.741f, 44.39055f),
            new Vector3(47.44701f, 2777.335f, 57.40466f),
            new Vector3(2676.54f, 3263.63f, 54.76064f),
            new Vector3(-2094.415f, -320.5872f, 12.54607f),
            new Vector3(266.9666f, -1253.497f, 28.51884f),
            new Vector3(1788.137f, 3330.988f, 40.96835f),
            new Vector3(1210.248f, 2660.764f, 37.50904f),
            new Vector3(1043.307f, 2672.588f, 39.24903f),
            new Vector3(-721.3615f, -932.5006f, 18.71538f),
            new Vector3(821.1265f, -1030.986f, 25.87164f),
            new Vector3(2536.900f, 2594.227f, 37.52017f),
            new Vector3(167.4295f, -1560.498f, 28.98856f)
        };

        /// <summary>
        /// Called in the OnTick event
        /// </summary>
        public override void Tick()
        {            // Create a new FuelVeh for the current vehicle if there isn't already one
            if(Game.Player.Character.IsInVehicle() && !Game.Player.Character.CurrentVehicle.Model.IsBicycle && FuelVehicles.FirstOrDefault(x => x.Vehicle == Game.Player.Character.CurrentVehicle) == null)
            {
                CurrentVehicle = new FuelVeh()
                {
                    FType = FuelType.Gas,
                    Fuel = new Random().Next(10, 125),
                    Vehicle = Game.Player.Character.CurrentVehicle
                };
                FuelVehicles.Add(CurrentVehicle);
            }

            // Set the current vehicle appropriately and runs everything needed when in a vehicle
            if (CurrentVehicle != null)
            {
                FuelBar.Position = new PointF(FuelBar.Position.X, Screen.Height - 137.5f + 125 - CurrentVehicle.Fuel);
                FuelBar.Size = new SizeF(FuelBar.Size.Width, CurrentVehicle.Fuel);
                FuelBarBackground.Draw();
                FuelBar.Draw();

                if (!Game.Player.Character.IsInVehicle())
                {
                    CurrentVehicle = null;
                    return ;
                }

                if (CurrentVehicle.Vehicle.IsEngineRunning)
                {
                    // Fuel depletion
                    switch (CurrentVehicle.Vehicle.Acceleration)
                    {
                        case -1: // Backwards
                            CurrentVehicle.Fuel -= 0.0015f;
                            break;
                        case 0: // Idle
                            CurrentVehicle.Fuel -= .00025f;
                            break;
                        case 1: // Forwards
                            CurrentVehicle.Fuel -= 0.0050f;
                            break;
                    }
                }

                // Engine killing
                if (CurrentVehicle.Fuel <= 0.0f && CurrentVehicle.Vehicle.IsEngineRunning)
                    CurrentVehicle.Vehicle.FuelLevel = 0;
                else if (CurrentVehicle.Fuel > 0.0f)
                    CurrentVehicle.Vehicle.FuelLevel = 60;
            }
            else if (CurrentVehicle == null && Game.Player.Character.IsInVehicle())
                CurrentVehicle = FuelVehicles.FirstOrDefault(x => x.Vehicle == Game.Player.Character.CurrentVehicle);

            if (Game.Player.Character.IsInVehicle())
            {
                // Gas station stuff
                foreach (Vector3 Station in GasStations)
                {
                    World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(Station.X, Station.Y, Station.Z - 1), Vector3.Zero, Vector3.Zero, new Vector3(1, 1, 1), System.Drawing.Color.Yellow);
                    if (World.GetDistance(Station, Game.Player.Character.Position) <= 2.5f)
                    {
                        int Cost = (int)Math.Round((125.0f - CurrentVehicle.Fuel) * .50f);
                        if (Cost == 0) Cost = 1;
                        if (Game.Player.Money >= Cost)
                        {
                            Screen.ShowHelpTextThisFrame($"Press ~INPUT_CONTEXT~ to refuel (${Cost})");
                            if (Game.IsControlJustPressed(Control.Context))
                            {
                                Screen.FadeOut(1500);
                                Script.Wait(1500);
                                CurrentVehicle.Fuel = 125;
                                Game.Player.Money -= Cost;
                                Script.Wait(1500);
                                Screen.FadeIn(1500);
                            }
                        }
                        else
                            Screen.ShowHelpTextThisFrame($"You cannot afford to refuel this vehicle. The cost is ${Cost}");
                    }
                }
            }
        }

        /// <summary>
        /// Called in the Aborted event
        /// </summary>
        public override void Abort()
        {
            // Delete all of the gas station blips
            foreach (Blip GasBlip in Blips)
                GasBlip.Delete();
        }

        /// <summary>
        /// Initializes the module
        /// </summary>
        public override void Initialize() 
        {  
            // Create all of the gas station blips
            foreach(Vector3 Station in GasStations)
            {
                Blip NewBlip = World.CreateBlip(Station);
                NewBlip.Sprite = BlipSprite.JerryCan;
                NewBlip.Name = "Gas Station";
                NewBlip.Color = BlipColor.Red;
                NewBlip.IsShortRange = true;
                Blips.Add(NewBlip);
            }
        }

        /// <summary>
        /// Drains all of the fuel from the current vehicle, for debug menu
        /// </summary>
        public void DrainFuel()
        {
            if(CurrentVehicle != null)
                CurrentVehicle.Fuel = 0;
        }
        /// <summary>
        /// Fills the current vehicle with fuel, for debug menu
        /// </summary>
        public void FillFuel()
        {
            if (CurrentVehicle != null)
                CurrentVehicle.Fuel = 125;
        }
    }

    internal class FuelVeh
    {
        public Vehicle Vehicle { get; set; }
        public float Fuel { get; set; }
        public FuelType FType { get; set; }
    }

    enum FuelType
    {
        Gas, Diesel
    }
}
