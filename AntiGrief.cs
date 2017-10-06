using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AntiGrief
{
    public class AntiGrief : RocketPlugin<AntiGriefConfig>
    {
        public AntiGrief Instance;

        protected override void Load()
        {
            Instance = this;
            Configuration.Save();

            Level.onPostLevelLoaded += OnPostLevelLoaded;
        }

        protected override void Unload()
        {
            Level.onPostLevelLoaded -= OnPostLevelLoaded;
        }

        private void OnPostLevelLoaded(int level)
        {
            Asset[] AssetList = Assets.find(EAssetType.ITEM);

            ushort gunsModified = 0;
            ushort meleesModified = 0;
            ushort throwablesModified = 0;
            ushort trapsModified = 0;
            ushort chargesModified = 0;
            ushort vehiclesModified = 0;
            ushort magsModified = 0;
            ushort elementsModified = 0;

            Logger.LogWarning("Starting anti grief modification run.");
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            bool shouldUpdateCount;
            for (int i = 0; i < AssetList.Length; i++)
            {
                shouldUpdateCount = false;
                Asset asset = AssetList[i];
                bool shouldSkip = false;
                // Look for and skip id's in the skil lists.
                for (int si = 0; si < Configuration.Instance.SkipItemIDs.Count; si++)
                {
                    if (asset.id == Configuration.Instance.SkipItemIDs[si])
                    {
                        shouldSkip = true;
                        break;
                    }
                }
                for (int se = 0; se < Configuration.Instance.SkilElementIDs.Count; se++)
                {
                    if (asset.id == Configuration.Instance.SkilElementIDs[se])
                    {
                        shouldSkip = true;
                        break;
                    }
                }
                if (shouldSkip)
                    continue;

                // Run though updating the items/elements/vehicles on the server.
                if (asset is ItemWeaponAsset)
                {
                    ItemWeaponAsset weaponAsset = asset as ItemWeaponAsset;
                    // Start modifying weapon type bundles, but skip the blowtorch(76) as that heals structures.
                    if (weaponAsset.barricadeDamage > 0 && Configuration.Instance.NegateBarricadeDamage && weaponAsset.id != 76)
                    {
                        weaponAsset.barricadeDamage = 0;
                        shouldUpdateCount = true;
                    }
                    if (weaponAsset.structureDamage > 0 && Configuration.Instance.NegateStructureDamage && weaponAsset.id != 76)
                    {
                        weaponAsset.structureDamage = 0;
                        shouldUpdateCount = true;
                    }
                    if (weaponAsset.vehicleDamage > 0 && Configuration.Instance.NegateVehicleDamage && weaponAsset.id != 76)
                    {
                        weaponAsset.vehicleDamage = 0;
                        shouldUpdateCount = true;
                    }

                    if (weaponAsset.objectDamage > 0 && Configuration.Instance.NegateObjectDamage)
                    {
                        weaponAsset.objectDamage = 0;
                        shouldUpdateCount = true;
                    }
                    // Don't change resource damage for resource gathering weapons: Camp Axe(16), Fire Axe(104), Chain Saw(490), Pickaxe(1198), Jackhammer(1475).
                    if (weaponAsset.resourceDamage > 0 && Configuration.Instance.NegateResourceDamage && weaponAsset.id != 16 && weaponAsset.id != 104 && weaponAsset.id != 490 && weaponAsset.id != 1198 && weaponAsset.id != 1475)
                    {
                        weaponAsset.resourceDamage = 0;
                        shouldUpdateCount = true;
                    }
                    if (shouldUpdateCount)
                    {
                        if (weaponAsset is ItemGunAsset)
                            gunsModified++;
                        if (weaponAsset is ItemMeleeAsset)
                            meleesModified++;
                        if (weaponAsset is ItemThrowableAsset)
                            throwablesModified++;
                    }
                }
                else if (asset is ItemTrapAsset)
                {
                    ItemTrapAsset trapAsset = asset as ItemTrapAsset;
                    if (trapAsset.barricadeDamage > 0 && Configuration.Instance.NegateBarricadeDamage)
                    {
                        trapAsset.GetType().GetField("_barricadeDamage", bindingFlags).SetValue(trapAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (trapAsset.structureDamage > 0 && Configuration.Instance.NegateStructureDamage)
                    {
                        trapAsset.GetType().GetField("_structureDamage", bindingFlags).SetValue(trapAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (trapAsset.structureDamage > 0 && Configuration.Instance.NegateVehicleDamage)
                    {
                        trapAsset.GetType().GetField("_vehicleDamage", bindingFlags).SetValue(trapAsset, 0);
                        shouldUpdateCount = true;
                    }

                    if (trapAsset.objectDamage > 0 && Configuration.Instance.NegateObjectDamage)
                    {
                        trapAsset.GetType().GetField("_objectDamage", bindingFlags).SetValue(trapAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (trapAsset.resourceDamage > 0 && Configuration.Instance.NegateResourceDamage)
                    {
                        trapAsset.GetType().GetField("_resourceDamage", bindingFlags).SetValue(trapAsset, 0);
                        shouldUpdateCount = true;
                    }

                    if (shouldUpdateCount)
                        trapsModified++;
                }
                else if (asset is ItemChargeAsset)
                {
                    ItemChargeAsset chargeAsset = asset as ItemChargeAsset;
                    if (chargeAsset.barricadeDamage > 0 && Configuration.Instance.NegateBarricadeDamage)
                    {
                        chargeAsset.GetType().GetField("_barricadeDamage", bindingFlags).SetValue(chargeAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (chargeAsset.structureDamage > 0 && Configuration.Instance.NegateStructureDamage)
                    {
                        chargeAsset.GetType().GetField("_structureDamage", bindingFlags).SetValue(chargeAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (chargeAsset.vehicleDamage > 0 && Configuration.Instance.NegateVehicleDamage)
                    {
                        chargeAsset.GetType().GetField("_vehicleDamage", bindingFlags).SetValue(chargeAsset, 0);
                        shouldUpdateCount = true;
                    }

                    if (chargeAsset.objectDamage > 0 && Configuration.Instance.NegateObjectDamage)
                    {
                        chargeAsset.GetType().GetField("_objectDamage", bindingFlags).SetValue(chargeAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (chargeAsset.resourceDamage > 0 && Configuration.Instance.NegateResourceDamage)
                    {
                        chargeAsset.GetType().GetField("_resourceDamage", bindingFlags).SetValue(chargeAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (shouldUpdateCount)
                        chargesModified++;
                }
                else if (asset is ItemMagazineAsset)
                {
                    ItemMagazineAsset magAsset = asset as ItemMagazineAsset;
                    if (magAsset.barricadeDamage > 0 && Configuration.Instance.NegateBarricadeDamage)
                    {
                        magAsset.GetType().GetField("_barricadeDamage", bindingFlags).SetValue(magAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (magAsset.structureDamage > 0 && Configuration.Instance.NegateStructureDamage)
                    {
                        magAsset.GetType().GetField("_structureDamage", bindingFlags).SetValue(magAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (magAsset.vehicleDamage > 0 && Configuration.Instance.NegateVehicleDamage)
                    {
                        magAsset.GetType().GetField("_vehicleDamage", bindingFlags).SetValue(magAsset, 0);
                        shouldUpdateCount = true;
                    }

                    if (magAsset.objectDamage > 0 && Configuration.Instance.NegateObjectDamage)
                    {
                        magAsset.GetType().GetField("_objectDamage", bindingFlags).SetValue(magAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (magAsset.resourceDamage > 0 && Configuration.Instance.NegateResourceDamage)
                    {
                        magAsset.GetType().GetField("_resourceDamage", bindingFlags).SetValue(magAsset, 0);
                        shouldUpdateCount = true;
                    }
                    if (shouldUpdateCount)
                        magsModified++;
                }
                shouldUpdateCount = false;
                if (asset is ItemBarricadeAsset)
                {
                    ItemBarricadeAsset basset = asset as ItemBarricadeAsset;
                    if (basset.health < Configuration.Instance.MinElementSpawnHealth && Configuration.Instance.ModifyMinElementSpawnHealth)
                    {
                        basset.GetType().GetField("_health", bindingFlags).SetValue(basset, Configuration.Instance.MinElementSpawnHealth);
                        shouldUpdateCount = true;
                    }
                    if (!basset.proofExplosion && Configuration.Instance.MakeElementsExplosionProof)
                    {
                        basset.GetType().GetField("_proofExplosion", bindingFlags).SetValue(basset, true);
                        shouldUpdateCount = true;
                    }
                    if (basset.isVulnerable && Configuration.Instance.MakeElementsInvulnerable)
                    {
                        basset.GetType().GetField("_isVulnerable", bindingFlags).SetValue(basset, false);
                        shouldUpdateCount = true;
                    }
                }
                if (asset is ItemStructureAsset)
                {
                    ItemStructureAsset sasset = asset as ItemStructureAsset;
                    if (sasset.health < Configuration.Instance.MinElementSpawnHealth && Configuration.Instance.ModifyMinElementSpawnHealth)
                    {
                        sasset.GetType().GetField("_health", bindingFlags).SetValue(sasset, Configuration.Instance.MinElementSpawnHealth);
                        shouldUpdateCount = true;
                    }
                    if (!sasset.proofExplosion && Configuration.Instance.MakeElementsExplosionProof)
                    {
                        sasset.GetType().GetField("_proofExplosion", bindingFlags).SetValue(sasset, true);
                        shouldUpdateCount = true;
                    }
                    if (sasset.isVulnerable && Configuration.Instance.MakeElementsInvulnerable)
                    {
                        sasset .GetType().GetField("_isVulnerable", bindingFlags).SetValue(sasset , false);
                        shouldUpdateCount = true;
                    }
                }
                if (shouldUpdateCount)
                    elementsModified++;
            }

            Asset[] vehicleList = Assets.find(EAssetType.VEHICLE);
            for (int v = 0; v < vehicleList.Length; v++)
            {
                shouldUpdateCount = false;
                Asset asset = vehicleList[v];
                bool shouldSkip = false;
                for (int i = 0; i < Configuration.Instance.SkipVehicleIDs.Count; i++)
                {
                    if (asset.id == Configuration.Instance.SkipVehicleIDs[i])
                    {
                        shouldSkip = true;
                        break;
                    }
                }
                if (shouldSkip == true)
                    continue;

                VehicleAsset vAsset = asset as VehicleAsset;
                if (!vAsset.isVulnerable && Configuration.Instance.MakeVehiclesInvulnerable)
                {
                    vAsset.GetType().GetField("_isVulnerable", bindingFlags).SetValue(vAsset, false);
                    shouldUpdateCount = true;
                }
                if (vAsset.canTiresBeDamaged && Configuration.Instance.MakeTiresInvulnerable)
                {
                    vAsset.GetType().GetProperty("canTiresBeDamaged", bindingFlags | BindingFlags.Public).SetValue(vAsset, false, null);
                    shouldUpdateCount = true;
                }
                if (vAsset.healthMax < Configuration.Instance.MinVehicleSpawnHealth && Configuration.Instance.ModifyMinVehicleSpawnHealth)
                {
                    vAsset.GetType().GetField("_healthMax", bindingFlags).SetValue(vAsset, Configuration.Instance.MinVehicleSpawnHealth);
                    shouldUpdateCount = true;
                }
                if (shouldUpdateCount)
                    vehiclesModified++;
            }
            Logger.LogWarning(string.Format("Finished modification run, counts of bundles modified: Guns: {0}, Mags: {6}, Melee: {1}, Throwables: {2}, Traps: {3}, Charges: {4}, Vehicles: {5}, Elements: {7}.", gunsModified, meleesModified, throwablesModified, trapsModified, chargesModified, vehiclesModified, magsModified, elementsModified));
        }
    }
}
