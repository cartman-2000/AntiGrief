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

        public bool MakeVehiclesInvulnerable = false;
        public bool MakeVehiclesInvulnerableExplosions = true;
        public bool MakeTiresInvulnerable = true;
        public bool VehicleCarjackOwnerGroupOnly = true;
        public bool ModifyMinVehicleSpawnHealth = false;
        public ushort MinVehicleSpawnHealth = 4000;
        public bool VehicleSetMobileBuildables = false;

        public bool ModifyMinElementSpawnHealth = false;
        public ushort MinElementSpawnHealth = 1000;
        public bool MakeElementsExplosionProof = true;
        public bool MakeElementsExplosionProofIncludeTraps = false;
        public bool MakeElementsInvulnerable = false;

        public bool MakeContainersLocked = false;
        public bool MakeDisplaysLocked = false;
        public bool ModDisplayGrid = false;
        public byte DisplayGridX = 5;
        public byte DisplayGridY = 5;
        public bool MakeSignsLocked = false;


        public bool DisableZombieElementDamage = true;
        public bool DisableZombieTrapDamage = false;
        public bool DisableMiscElementDamage = true;

        public bool RestrictHarvesting = false;
        public bool ShowHarvestBlockMessage = true;

        public bool EnableItemDropRestriction = false;
        public bool EnableInvRestrictedItemCheck = false;
        public float CheckFrequency = 1; 

        [XmlArray("ItemDropDeniedList"), XmlArrayItem(ElementName = "ID")]
        public List<ushort> ItemDropDeniedList = new List<ushort>();

        [XmlArray("ItemInvRestrictedList"), XmlArrayItem(ElementName = "ID")]
        public List<ushort> ItemInvRestrictedList = new List<ushort>();

        [XmlArray("SkipItemIDs"), XmlArrayItem(ElementName = "ID")]
        public List<ushort> SkipItemIDs = new List<ushort>();

        [XmlArray("SkipElementIDs"), XmlArrayItem(ElementName = "ID")]
        public List<ushort> SkipElementIDs = new List<ushort>();

        [XmlArray("SkipVehicleIDs"), XmlArrayItem(ElementName = "ID")]
        public List<ushort> SkipVehicleIDs = new List<ushort>();

        public void LoadDefaults()
        {
            ItemDropDeniedList = new List<ushort>()
            {
                { 1353 },
            };

            ItemInvRestrictedList = new List<ushort>()
            {
                { 1353 },
            };

            SkipItemIDs = new List<ushort>()
            {
                { 76 },
            };

            SkipElementIDs = new List<ushort>()
            {
                { 383 },
                { 384 },
                { 385 },
            };

            SkipVehicleIDs = new List<ushort>()
            {
                { 186 },
            };
        }
    }
}