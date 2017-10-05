using System;
using Rocket.API;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace AntiGrief
{
    public class AntiGriefConfig : IRocketPluginConfiguration
    {
        public bool NegateBarricadeDamage = true;
        public bool NegateStructureDamage = true;
        public bool NegateVehicleDamage = true;
        public bool NegateObjectDamage = false;
        public bool NegateResourceDamage = false;

        public bool MakeVehiclesInvulnerable = true;
        public bool MakeTiresInvulnerable = true;

        public bool ModifyMinVehicleSpawnHealth = false;
        public ushort MinVehicleSpawnHealth = 4000;

        public bool ModifyMinBarricadeStructureHealth = false;
        public ushort MinBarricadeStructureHealth = 1000;

        [XmlArray("SkipItemIDs"), XmlArrayItem(ElementName = "ID")]
        public List<ushort> SkipItemIDs = new List<ushort>()
        {
            { 76 },
        };

        [XmlArray("SkipVehicleIDs"), XmlArrayItem(ElementName = "ID")]
        public List<ushort> SkipVehicleIDs = new List<ushort>()
        {
            { 91 },
        };
        public void LoadDefaults()
        {
        }
    }
}