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
            for (int i = 0; i < AssetList.Length; i++)
            {
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
                    if (weaponAsset.barricadeDamage > 0 || weaponAsset.structureDamage > 0 || weaponAsset.vehicleDamage > 0)
                    {
                        if (weaponAsset is ItemGunAsset)
                            gunsModified++;
                        if (weaponAsset is ItemMeleeAsset)
                            meleesModified++;
                        if (weaponAsset is ItemThrowableAsset)
                            throwablesModified++;
                        weaponAsset.barricadeDamage = 0;
                        weaponAsset.structureDamage = 0;
                        weaponAsset.vehicleDamage = 0;
                    }
                }
                else if (asset is ItemTrapAsset)
                {
                    ItemTrapAsset trapAsset = asset as ItemTrapAsset;
                    if (trapAsset.barricadeDamage > 0 || trapAsset.structureDamage > 0 || trapAsset.vehicleDamage > 0)
                    {
                        trapsModified++;
                        trapAsset.GetType().GetField("_barricadeDamage", bindingFlags).SetValue(trapAsset, 0);
                        trapAsset.GetType().GetField("_structureDamage", bindingFlags).SetValue(trapAsset, 0);
                        trapAsset.GetType().GetField("_vehicleDamage", bindingFlags).SetValue(trapAsset, 0);
                    }
                }
                else if (asset is ItemChargeAsset)
                {
                    ItemChargeAsset chargeAsset = asset as ItemChargeAsset;
                    if (chargeAsset.barricadeDamage > 0 || chargeAsset.structureDamage > 0 || chargeAsset.vehicleDamage > 0)
                    {
                        chargesModified++;
                        chargeAsset.GetType().GetField("_barricadeDamage", bindingFlags).SetValue(chargeAsset, 0);
                        chargeAsset.GetType().GetField("_structureDamage", bindingFlags).SetValue(chargeAsset, 0);
                        chargeAsset.GetType().GetField("_vehicleDamage", bindingFlags).SetValue(chargeAsset, 0);
                    }
                }
                else if (asset is ItemMagazineAsset)
                {
                    ItemMagazineAsset magAsset = asset as ItemMagazineAsset;
                    if (magAsset.barricadeDamage > 0 || magAsset.structureDamage > 0 || magAsset.vehicleDamage > 0)
                    {
                        magsModified++;
                        magAsset.GetType().GetField("_barricadeDamage", bindingFlags).SetValue(magAsset, 0);
                        magAsset.GetType().GetField("_structureDamage", bindingFlags).SetValue(magAsset, 0);
                        magAsset.GetType().GetField("_vehicleDamage", bindingFlags).SetValue(magAsset, 0);
                    }
                }
            }


            Asset[] vehicleList = Assets.find(EAssetType.VEHICLE);
            for (int v = 0; v < vehicleList.Length; v++)
            {
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
                if (!vAsset.isVulnerable || !vAsset.canTiresBeDamaged)
                {
                    vehiclesModified++;
                    vAsset.GetType().GetField("_isVulnerable", bindingFlags).SetValue(vAsset, false);
                    vAsset.GetType().GetProperty("canTiresBeDamaged", bindingFlags | BindingFlags.Public).SetValue(vAsset, false, null);
                }
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
