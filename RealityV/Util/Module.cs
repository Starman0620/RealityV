﻿namespace RealityV.Util
{
    internal abstract class Module
    {
        /// <summary>
        /// Initializes the module
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Called in the OnTick event
        /// </summary>
        public abstract void Tick();
    }
}
