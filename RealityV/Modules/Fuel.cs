using System;
using System.Collections.Generic;
using System.Linq;

using GTA;
using GTA.Math;
using GTA.UI;

using RealityV.Util;

namespace RealityV.Modules
{
    internal class Fuel : Module
    {
        List<FuelVeh> FuelVehicles = new List<FuelVeh>();
        FuelVeh CurrentVehicle;
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
            new Vector3(-2094.415f, -320.5872f, 12.54607f)
        };

        /// <summary>
        /// Called in the OnTick event
        /// </summary>
        public override void Tick()
        {
            // Create a new FuelVeh for the current vehicle if there isn't already one
            if(Game.Player.Character.IsInVehicle() && !Game.Player.Character.CurrentVehicle.Model.IsBicycle && FuelVehicles.FirstOrDefault(x => x.Vehicle == Game.Player.Character.CurrentVehicle) == null)
            {
                CurrentVehicle = new FuelVeh()
                {
                    FType = FuelType.Gas,
                    Fuel = new Random().Next(10, 100),
                    Vehicle = Game.Player.Character.CurrentVehicle
                };
                FuelVehicles.Add(CurrentVehicle);
            }

            // Set the current vehicle appropriately and runs everything needed when in a vehicle
            if (CurrentVehicle != null)
            {
                if (!Game.Player.Character.IsInVehicle())
                {
                    CurrentVehicle = null;
                    return ;
                }

                Screen.ShowSubtitle($"~y~Fuel: ~w~{CurrentVehicle.Fuel}\n~y~Accel: ~w~{CurrentVehicle.Vehicle.Acceleration}\n~y~Game Fuel: ~w~{CurrentVehicle.Vehicle.FuelLevel}", 1);
                // Fuel depletion
                switch (CurrentVehicle.Vehicle.Acceleration)
                {
                    case -1: // Backwards
                        CurrentVehicle.Fuel -= 0.0010f;
                        break;
                    case 0: // Idle
                        CurrentVehicle.Fuel -= 0.0001f;
                        break;
                    case 1: // Forwards
                        CurrentVehicle.Fuel -= 0.0020f;
                        break;
                }

                // Engine killing
                if (CurrentVehicle.Fuel <= 0.0f && CurrentVehicle.Vehicle.IsEngineRunning)
                    CurrentVehicle.Vehicle.FuelLevel = 0;
                else if (CurrentVehicle.Fuel > 0.0f)
                    CurrentVehicle.Vehicle.FuelLevel = 60;
            }
            else if (CurrentVehicle == null && Game.Player.Character.IsInVehicle())
                CurrentVehicle = FuelVehicles.FirstOrDefault(x => x.Vehicle == Game.Player.Character.CurrentVehicle);
        }

        /// <summary>
        /// Initializes the module
        /// </summary>
        public override void Initialize() {    }

        public void DrainFuel()
        {
            if(CurrentVehicle != null)
                CurrentVehicle.Fuel = 0;
        }

        public void FillFuel()
        {
            if (CurrentVehicle != null)
                CurrentVehicle.Fuel = 100;
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
        Gas, Diesel, Kerosene
    }
}
