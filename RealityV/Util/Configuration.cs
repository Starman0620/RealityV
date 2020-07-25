using System.IO;
using System.Xml.Serialization;

namespace RealityV.Util
{
    public class Configuration
    {
        public Modules Modules { get; set; }

        /// <summary>
        /// Loads the configuration from XML
        /// </summary>
        /// <returns></returns>
        public static Configuration FromFile()
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(Configuration));
            if (File.Exists("scripts\\RealityV.xml"))
            {
                FileStream Stream = new FileStream("scripts\\RealityV.xml", FileMode.Open);
                Configuration Config = (Configuration)Serializer.Deserialize(Stream);
                Stream.Close();
                return Config;
            }
            else
            {
                FileStream Stream = new FileStream("scripts\\RealityV.xml", FileMode.CreateNew);
                Configuration Config = new Configuration()
                {
                    Modules = new Modules()
                    {
                        Homeless = true,
                        Bills = true,
                        Hunger = true,
                        Fuel = true,
                        IncomeTax = true,
                        Jobs = true
                    }
                };
                Serializer.Serialize(Stream, Config);
                Stream.Close();
                return Config;
            }
        }
    }
}
