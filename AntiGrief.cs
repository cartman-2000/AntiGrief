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

            Asset[] AssetList = Assets.find(EAssetType.ITEM);

            ushort gunsModified = 0;
            ushort meleesModified = 0;
            ushort throwablesModified = 0;
            ushort trapsModified = 0;
            ushort chargesModified = 0;
            ushort vehiclesModified = 0;
            ushort magsModified = 0;

            Logger.LogWarning("Starting anti grief modification run.");
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            bool shouldUpdateCount;
            for (int i = 0; i < AssetList.Length; i++)
            {
                shouldUpdateCount = false;
                Asset asset = AssetList[i];
                bool shouldSkip = false;
                for (int s = 0; s < Configuration.Instance.SkipItemIDs.Count; s++)
                {
                    if (asset.id == Configuration.Instance.SkipItemIDs[s])
                    {
                        shouldSkip = true;
                        break;
                    }
                }
                if (shouldSkip)
                    continue;
                if (asset is ItemWeaponAsset)
                {
                    ItemWeaponAsset weaponAsset = asset as ItemWeaponAsset;
                    if (weaponAsset.barricadeDamage > 0 && Configuration.Instance.NegateBarricadeDamage)
                    {
                        weaponAsset.barricadeDamage = 0;
                        shouldUpdateCount = true;
                    }
                    if (weaponAsset.structureDamage > 0 && Configuration.Instance.NegateStructureDamage)
                    {
                        weaponAsset.structureDamage = 0;
                        shouldUpdateCount = true;
                    }
                    if (weaponAsset.vehicleDamage > 0 && Configuration.Instance.NegateVehicleDamage)
                    {
                        weaponAsset.vehicleDamage = 0;
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
                    if (shouldUpdateCount)
                        magsModified++;
                }
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
                if (!vAsset.canTiresBeDamaged && Configuration.Instance.MakeTiresInvulnerable)
                {
                    vAsset.GetType().GetProperty("canTiresBeDamaged", bindingFlags | BindingFlags.Public).SetValue(vAsset, false, null);
                    shouldUpdateCount = true;
                }
                if (Configuration.Instance.MinVehicleSpawnHealth >= vAsset.healthMax && Configuration.Instance.ModifyMinVehicleSpawnHealth)
                {
                    vAsset.GetType().GetField("_healthMax", bindingFlags).SetValue(vAsset, Configuration.Instance.MinVehicleSpawnHealth);
                    shouldUpdateCount = true;
                }
                if (shouldUpdateCount)
                    vehiclesModified++;
            }
            Logger.LogWarning(string.Format("Finished modification run, counts of bundles modified: Guns: {0}, Mags: {6}, Melee: {1}, Throwables: {2}, Traps: {3}, Charges: {4}, Vehicles: {5}.", gunsModified, meleesModified, throwablesModified, trapsModified, chargesModified, vehiclesModified, magsModified));
        }

        private PropertyInfo GetPropertieInfo(VehicleAsset asset, string name)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            Type type = asset.GetType();
            PropertyInfo propInfo = type.GetProperty(name, bindingFlags);
            return propInfo;
        }
    }
}
