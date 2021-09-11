global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Drawing;
global using System.Linq;
global using System.Reflection;
global using System.Reflection.Emit;
global using System.Runtime.InteropServices;

global using AdditionalTiers.Resources;
global using AdditionalTiers.Tasks;
global using AdditionalTiers.Utils;
global using AdditionalTiers.Utils.Assets;
global using AdditionalTiers.Utils.Attack;
global using AdditionalTiers.Utils.Components;
global using AdditionalTiers.Utils.Math;
global using AdditionalTiers.Utils.Towers;

global using Assets.Scripts.Models;
global using Assets.Scripts.Models.GenericBehaviors;
global using Assets.Scripts.Models.Towers;
global using Assets.Scripts.Models.Towers.Behaviors;
global using Assets.Scripts.Models.Towers.Behaviors.Abilities;
global using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
global using Assets.Scripts.Models.Towers.Behaviors.Attack;
global using Assets.Scripts.Models.Towers.Behaviors.Emissions;
global using Assets.Scripts.Models.Towers.Mods;
global using Assets.Scripts.Models.Towers.Projectiles;
global using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
global using Assets.Scripts.Models.Towers.Upgrades;
global using Assets.Scripts.Simulation.Towers;
global using Assets.Scripts.Simulation.Towers.Weapons;
global using Assets.Scripts.Unity.Bridge;
global using Assets.Scripts.Unity.Display;
global using Assets.Scripts.Unity.UI_New.InGame;
global using Assets.Scripts.Unity.UI_New.InGame.AbilitiesMenu;
global using Assets.Scripts.Unity.UI_New.InGame.Stats;
global using Assets.Scripts.Utils;

global using HarmonyLib;

global using MelonLoader;

global using UnhollowerBaseLib;
global using UnhollowerBaseLib.Attributes;

global using UnhollowerRuntimeLib;

global using UnityEngine;

global using static HarmonyLib.AccessTools;

global using ADisplay = AdditionalTiers.Utils.Display;
global using Type = Il2CppSystem.Type;
global using Color = UnityEngine.Color;
global using Object = UnityEngine.Object;
global using Image = UnityEngine.UI.Image;
