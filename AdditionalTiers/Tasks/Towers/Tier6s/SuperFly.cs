using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Models;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Simulation.SMath;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.UI_New.InGame.AbilitiesMenu;
using Assets.Scripts.Utils;
using Harmony;
using Il2CppSystem;
using MelonLoader;
using AdditionalTiers.Utils;
using Assets.Scripts.Models.Towers.TowerFilters;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using v = Assets.Scripts.Simulation.SMath.Vector3;

namespace AdditionalTiers.Tasks.Towers.Tier6s {
    public class SuperFly : TowerTask {
        public static TowerModel superFly;
        private static int time = -1;
        public SuperFly() {
            identifier = "Super Fly";
            getTower = superFly;
            requirements += tts => tts.tower.towerModel.baseId.Equals("MonkeyVillage") && tts.tower.towerModel.tiers[0] == 5 && tts.damageDealt > ((int)AddedTierEnum.SUPERFLY) * Globals.SixthTierPopCountMulti;
            onComplete += tts => {
                if (time < 50) {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.worth = 0;
                tts.tower.UpdateRootModel(superFly);
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "Upgraded!", false);
                Globals.SixthTierPopCountMulti /= 10;
                AbilityMenu.instance.TowerChanged(tts);
                AbilityMenu.instance.RebuildAbilities();
            };
            gameLoad += gm => {
                superFly = gm.towers.First(a => a.name.Contains("MonkeyVillage-520")).Clone()
                    .Cast<TowerModel>();

                superFly.range = 500;
                superFly.cost = 0;
                superFly.name = "Super Fly";
                superFly.baseId = "SuperFly";
                superFly.display = "SuperFly";
                superFly.dontDisplayUpgrades = true;
                superFly.portrait = new("SuperFlyIcon");
                superFly.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>()).Cast<DisplayModel>().display = "SuperFly";
                var beh = superFly.behaviors;
                for (var i = 0; i < beh.Length; i++) {
                    var behavior = beh[i];
                    if (behavior.GetIl2CppType() == Il2CppType.Of<PierceSupportModel>()) {
                        PierceSupportModel a = behavior.Cast<PierceSupportModel>();
                        a.pierce = 5;
                        a.filters = new Il2CppReferenceArray<TowerFilterModel>(0);
                        behavior = a;
                    }
                    if (behavior.GetIl2CppType() == Il2CppType.Of<ProjectileSpeedSupportModel>()) {
                        ProjectileSpeedSupportModel a = behavior.Cast<ProjectileSpeedSupportModel>();
                        a.multiplier = 0.5f;
                        a.filters = new Il2CppReferenceArray<TowerFilterModel>(0);
                        behavior = a;
                    }
                    if (behavior.GetIl2CppType() == Il2CppType.Of<RangeSupportModel>()) {
                        RangeSupportModel a = behavior.Cast<RangeSupportModel>();
                        // There's 2 RangeSupportModels
                        if (a.multiplier == .1f)
                            a.multiplier = .25f;
                        if (a.additive == 5)
                            a.additive = 10;
                        a.filters = new Il2CppReferenceArray<TowerFilterModel>(0);
                        behavior = a;
                    }
                    if (behavior.GetIl2CppType() == Il2CppType.Of<FreeUpgradeSupportModel>()) {
                        FreeUpgradeSupportModel a = behavior.Cast<FreeUpgradeSupportModel>();
                        a.upgrade = 3;
                        a.filters = new Il2CppReferenceArray<TowerFilterModel>(0);
                        behavior = a;
                    }
                    if (behavior.GetIl2CppType() == Il2CppType.Of<AbilityCooldownScaleSupportModel>()) {
                        AbilityCooldownScaleSupportModel a = behavior.Cast<AbilityCooldownScaleSupportModel>();
                        a.filters = new Il2CppReferenceArray<TowerFilterModel>(0);
                        behavior = a;
                    }
                    
                    beh[i] = behavior;
                }

                superFly.behaviors = beh.Remove(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>());
            };
            recurring += tts => {};
            onLeave += () => { time = -1; Globals.Load(); };
            CacheBuilder.toBuild.PushAll("SuperFly", "SuperFlyIcon");
            assetsToRead.Add(new ("SuperFly", "06b880ab7e2941b4f9de3e132ba1e11e", RendererType.SKINNEDMESHRENDERER));
        }
    }
}