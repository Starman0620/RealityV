using System;
using System.Collections.Generic;
using System.Linq;

using GTA;
using GTA.UI;

using RealityV.Util;

namespace RealityV.Modules
{
    internal class Fuel : Module
    {
        List<FuelVeh> FuelVehicles = new List<FuelVeh>();
        FuelVeh CurrentVehicle;

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
