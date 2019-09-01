﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossChecklist
{
	internal class BossTracker
	{
		public const float SlimeKing = 1f;
		public const float EyeOfCthulhu = 2f;
		public const float EaterOfWorlds = 3f;
		public const float QueenBee = 4f;
		public const float Skeletron = 5f;
		public const float WallOfFlesh = 6f;
		public const float TheTwins = 7f;
		public const float TheDestroyer = 8f;
		public const float SkeletronPrime = 9f;
		public const float Plantera = 10f;
		public const float Golem = 11f;
		public const float DukeFishron = 12f;
		public const float LunaticCultist = 13f;
		public const float Moonlord = 14f;

		/// <summary>
		/// All currently loaded bosses/minibosses/events sorted in progression order.
		/// </summary>
		internal List<BossInfo> SortedBosses;

		// TODO: OrphanBosses: Boss info added to other bosses when those bosses aren't loaded yet. Log remaining orphans maybe after load.

		public BossTracker() {
			BossChecklist.bossTracker = this;
			InitializeVanillaBosses();
		}

		private void InitializeVanillaBosses() {
			SortedBosses = new List<BossInfo> {
			// Bosses -- Vanilla
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, SlimeKing, "King Slime", new List<int>() { NPCID.KingSlime }, () => NPC.downedSlimeKing, new List<int>() { ItemID.SlimeCrown },
				$"Use [i:{ItemID.SlimeCrown}], randomly in outer 3rds of map, or kill 150 slimes during slime rain."
			),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, EyeOfCthulhu, "Eye of Cthulhu", new List<int>() { NPCID.EyeofCthulhu }, () => NPC.downedBoss1, new List<int>() { ItemID.SuspiciousLookingEye },
				$"Use [i:{ItemID.SuspiciousLookingEye}] at night, or 1/3 chance nightly if over 200 HP\nAchievement : [a:EYE_ON_YOU]"
			),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, EaterOfWorlds,"Eater of Worlds",new List<int>() { NPCID.EaterofWorldsHead, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail}, () => NPC.downedBoss2,new List<int>() { ItemID.WormFood },  $"Use [i:{ItemID.WormFood}] or [i:{ItemID.BloodySpine}] or break 3 Crimson Hearts or Shadow Orbs"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, EaterOfWorlds,"Brain of Cthulhu",new List<int>() { NPCID.BrainofCthulhu },
			 () => NPC.downedBoss2, new List<int>() { ItemID.BloodySpine }, $"Use [i:{ItemID.WormFood}] or [i:{ItemID.BloodySpine}] or break 3 Crimson Hearts or Shadow Orbs"), // FIX
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, QueenBee, "Queen Bee", new List<int>() { NPCID.QueenBee },
			 () => NPC.downedQueenBee, new List<int>() { ItemID.Abeemination }, $"Use [i:{ItemID.Abeemination}] or break Larva in Jungle"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, Skeletron,"Skeletron", new List<int>() { NPCID.SkeletronHead },
			 () => NPC.downedBoss3, new List<int>() { ItemID.ClothierVoodooDoll }, $"Visit dungeon or use [i:{ItemID.ClothierVoodooDoll}] at night"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, WallOfFlesh,"Wall of Flesh", new List<int>() { NPCID.WallofFlesh },
			 () => Main.hardMode  , new List<int>() { ItemID.GuideVoodooDoll }, $"Spawn by throwing [i:{ItemID.GuideVoodooDoll}] in lava in the Underworld. [c/FF0000:Starts Hardmode!]"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, TheTwins,"The Twins", new List<int>() { NPCID.Retinazer, NPCID.Spazmatism },
			 () => NPC.downedMechBoss2,new List<int>() { ItemID.MechanicalEye },  $"Use [i:{ItemID.MechanicalEye}] at night to spawn"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, TheDestroyer,"The Destroyer",new List<int>() { NPCID.TheDestroyer },
			 () => NPC.downedMechBoss1,  new List<int>() { ItemID.MechanicalWorm }, $"Use [i:{ItemID.MechanicalWorm}] at night to spawn"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, SkeletronPrime,"Skeletron Prime", new List<int>() { NPCID.SkeletronPrime },
			 () => NPC.downedMechBoss3,new List<int>() { ItemID.MechanicalSkull },   $"Use [i:{ItemID.MechanicalSkull}] at night to spawn"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, Plantera,"Plantera", new List<int>() { NPCID.Plantera },
			 () => NPC.downedPlantBoss, new List<int>() { }, $"Break a Plantera's Bulb in jungle after 3 Mechanical bosses have been defeated"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, Golem,"Golem", new List<int>() { NPCID.Golem, NPCID.GolemHead },
			 () => NPC.downedGolemBoss,  new List<int>() { ItemID.LihzahrdPowerCell }, $"Use [i:{ItemID.LihzahrdPowerCell}] on Lihzahrd Altar"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, Golem + 0.5f, "Betsy",  new List<int>() { NPCID.DD2Betsy }, () => WorldAssist.downedBetsy, new List<int>() { ItemID.DD2ElderCrystal }, "Fight during Old One's Army Tier 3"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, DukeFishron,"Duke Fishron",new List<int>() { NPCID.DukeFishron },
				 () => NPC.downedFishron, new List<int>() { ItemID.TruffleWorm }, $"Fish in ocean using the [i:{ItemID.TruffleWorm}] bait"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, LunaticCultist,"Lunatic Cultist",new List<int>() { NPCID.CultistBoss },                 () => NPC.downedAncientCultist, new List<int>() { }, $"Kill the cultists outside the dungeon post-Golem"),
			BossInfo.MakeVanillaBoss(BossChecklistType.Boss, Moonlord,"Moonlord",  new List<int>() { NPCID.MoonLordHead, NPCID.MoonLordCore, NPCID.MoonLordHand },
				 () => NPC.downedMoonlord, new List<int>() { ItemID.CelestialSigil }, $"Use [i:{ItemID.CelestialSigil}] or defeat all {(BossChecklist.tremorLoaded ? 5 : 4)} pillars. {(BossChecklist.tremorLoaded ? "[c/FF0000:Starts Tremode!]" : "")}"),
			// Event Bosses -- Vanilla
			BossInfo.MakeVanillaEvent(LunaticCultist + .1f, "Nebula Pillar", () => NPC.downedTowerNebula, new List<int>() { }, $"Kill the Lunatic Cultist outside the dungeon post-Golem"),
			BossInfo.MakeVanillaEvent(LunaticCultist + .2f, "Vortex Pillar",
				 () => NPC.downedTowerVortex,new List<int>() { },$"Kill the Lunatic Cultist outside the dungeon post-Golem"),
			BossInfo.MakeVanillaEvent( LunaticCultist +.3f,"Solar Pillar",
				 () => NPC.downedTowerSolar,new List<int>() { },  $"Kill the Lunatic Cultist outside the dungeon post-Golem"),
			BossInfo.MakeVanillaEvent(LunaticCultist + .4f, "Stardust Pillar",
				 () => NPC.downedTowerStardust, new List<int>() { }, $"Kill the Lunatic Cultist outside the dungeon post-Golem"),
			// TODO, all other event bosses...Maybe all pillars as 1?
			BossInfo.MakeVanillaBoss(BossChecklistType.MiniBoss,WallOfFlesh + 0.1f, "Clown", new List<int>() { NPCID.Clown},  () => NPC.downedClown, new List<int>() { }, $"Spawns during Hardmode Bloodmoon"),
			BossInfo.MakeVanillaEvent(EyeOfCthulhu + 0.5f,"Goblin Army",  () => NPC.downedGoblins,new List<int>() {ItemID.GoblinBattleStandard },  $"Occurs randomly at dawn once a Shadow Orb or Crimson Heart has been destroyed. Alternatively, spawn with [i:{ItemID.GoblinBattleStandard}]"),
			BossInfo.MakeVanillaBoss(BossChecklistType.MiniBoss, Plantera + 0.9f, "Ice Queen",  new List<int>() {NPCID.IceQueen },() => NPC.downedChristmasIceQueen, new List<int>() { }, $"Spawns during Wave 11 of Frost Moon. Start Frost Moon with [i:{ItemID.NaughtyPresent}]"),
			BossInfo.MakeVanillaBoss(BossChecklistType.MiniBoss, Plantera + 0.6f,"Santa-NK1", new List<int>() { NPCID.SantaNK1},  () => NPC.downedChristmasSantank, new List<int>() { }, $"Spawns during Wave 7 of Frost Moon. Start Frost Moon with [i:{ItemID.NaughtyPresent}]"),
			BossInfo.MakeVanillaBoss(BossChecklistType.MiniBoss,  Plantera + 0.3f,"Everscream", new List<int>() {NPCID.Everscream }, () => NPC.downedChristmasTree, new List<int>() { }, $"Spawns during Wave 4 of Frost Moon. Start Frost Moon with [i:{ItemID.NaughtyPresent}]"),
			BossInfo.MakeVanillaEvent( WallOfFlesh + 0.6f,"Frost Legion",   () => NPC.downedFrost, new List<int>() { ItemID.SnowGlobe}, $"Use [i:{ItemID.SnowGlobe}] to start. Find [i:{ItemID.SnowGlobe}] by opening [i:{ItemID.Present}] while in Hardmode during Christmas season."),
			BossInfo.MakeVanillaBoss(BossChecklistType.MiniBoss, Plantera + 0.3f,"Pumpking", new List<int>() {NPCID.Pumpking },  () => NPC.downedHalloweenKing, new List<int>() { }, $"Spawns during Wave 7 of Pumpkin Moon. Start Pumpkin Moon with [i:{ItemID.PumpkinMoonMedallion}]"),
			BossInfo.MakeVanillaBoss(BossChecklistType.MiniBoss, Plantera + 0.6f,"Mourning Wood",  new List<int>() {NPCID.MourningWood }, () => NPC.downedHalloweenTree, new List<int>() { }, $"Spawns during Wave 4 of Pumpkin Moon. Start Pumpkin Moon with [i:{ItemID.PumpkinMoonMedallion}]"),
			BossInfo.MakeVanillaEvent( Golem + 0.4f,"Martian Madness",   () => NPC.downedMartians, new List<int>() { }, $"After defeating Golem, find a Martian Probe above ground and let it escape."),
			BossInfo.MakeVanillaEvent( WallOfFlesh + 0.7f,"Pirate Invasion",   () => NPC.downedPirates,  new List<int>() { ItemID.PirateMap},$"Occurs randomly in Hardmode after an Altar has been destroyed. Alternatively, spawn with [i:{ItemID.PirateMap}]"),
			BossInfo.MakeVanillaEvent( EaterOfWorlds + 0.5f,"Old One's Army Any Tier",   () => Terraria.GameContent.Events.DD2Event.DownedInvasionAnyDifficulty,new List<int>() {ItemID.DD2ElderCrystal, ItemID.DD2ElderCrystalStand },  $"After finding the Tavernkeep, activate [i:{ItemID.DD2ElderCrystalStand}] with [i:{ItemID.DD2ElderCrystal}]"),
			// TODO: track bugged DownedInvasionT1 event separately from vanilla somehow.
			//new BossInfo(BossChecklistType.Event, "Old One's Army 1", EaterOfWorlds + 0.5f, () => true, () => Terraria.GameContent.Events.DD2Event.DownedInvasionT1,  $"After finding the Tavernkeep, activate [i:{ItemID.DD2ElderCrystalStand}] with [i:{ItemID.DD2ElderCrystal}]"),
			//new BossInfo(BossChecklistType.Event, "Old One's Army 2", TheTwins + 0.5f, () => true, () => Terraria.GameContent.Events.DD2Event.DownedInvasionT2,  $"After defeating a mechanical boss, activate [i:{ItemID.DD2ElderCrystalStand}] with [i:{ItemID.DD2ElderCrystal}]"),
			//new BossInfo(BossChecklistType.Event, "Old One's Army 3", Golem + 0.5f, () => true, () => Terraria.GameContent.Events.DD2Event.DownedInvasionT3,  $"After defeating Golem, activate [i:{ItemID.DD2ElderCrystalStand}] with [i:{ItemID.DD2ElderCrystal}]"),
			};
			SortedBosses.Sort((x, y) => x.progression.CompareTo(y.progression));
		}

		internal protected List<int> SetupLoot(int bossNum) {
			if (bossNum == NPCID.KingSlime) {
				return new List<int>()
				{
					ItemID.KingSlimeBossBag,
					ItemID.RoyalGel,
					ItemID.Solidifier,
					ItemID.SlimySaddle,
					ItemID.NinjaHood,
					ItemID.NinjaShirt,
					ItemID.NinjaPants,
					ItemID.SlimeHook,
					ItemID.SlimeGun,
					ItemID.LesserHealingPotion
				};
			}
			if (bossNum == NPCID.EyeofCthulhu) {
				return new List<int>()
				{
					ItemID.EyeOfCthulhuBossBag,
					ItemID.EoCShield,
					ItemID.DemoniteOre,
					ItemID.UnholyArrow,
					ItemID.CorruptSeeds,
					ItemID.Binoculars,
					ItemID.LesserHealingPotion
				};
			}
			if (bossNum == NPCID.EaterofWorldsHead) {
				return new List<int>()
				{
					ItemID.EaterOfWorldsBossBag,
					ItemID.WormScarf,
					ItemID.ShadowScale,
					ItemID.DemoniteOre,
					ItemID.EatersBone,
					ItemID.LesserHealingPotion
				};
			}
			if (bossNum == NPCID.BrainofCthulhu) {
				return new List<int>()
				{
					ItemID.BrainOfCthulhuBossBag,
					ItemID.BrainOfConfusion,
					ItemID.CrimtaneOre,
					ItemID.TissueSample,
					ItemID.BoneRattle,
					ItemID.LesserHealingPotion
				};
			}
			if (bossNum == NPCID.QueenBee) {
				return new List<int>()
				{
					ItemID.QueenBeeBossBag,
					ItemID.HiveBackpack,
					ItemID.BeeGun,
					ItemID.BeeKeeper,
					ItemID.BeesKnees,
					ItemID.HiveWand,
					ItemID.BeeHat,
					ItemID.BeeShirt,
					ItemID.BeePants,
					ItemID.HoneyComb,
					ItemID.Nectar,
					ItemID.HoneyedGoggles,
					ItemID.Beenade,
					ItemID.BottledHoney
				};
			}
			if (bossNum == NPCID.SkeletronHead) {
				return new List<int>()
				{
					ItemID.SkeletronBossBag,
					ItemID.BoneGlove,
					ItemID.SkeletronHand,
					ItemID.BookofSkulls,
					ItemID.LesserHealingPotion
				};
			}
			if (bossNum == NPCID.WallofFlesh) {
				return new List<int>()
				{
					ItemID.WallOfFleshBossBag,
					ItemID.DemonHeart,
					ItemID.Pwnhammer,
					ItemID.BreakerBlade,
					ItemID.ClockworkAssaultRifle,
					ItemID.LaserRifle,
					ItemID.WarriorEmblem,
					ItemID.SorcererEmblem,
					ItemID.RangerEmblem,
					ItemID.SummonerEmblem,
					ItemID.HealingPotion
				};
			}
			if (bossNum == NPCID.Retinazer) {
				return new List<int>()
				{
					ItemID.TwinsBossBag,
					ItemID.MechanicalWheelPiece,
					ItemID.SoulofSight,
					ItemID.HallowedBar,
					ItemID.GreaterHealingPotion
				};
			}
			if (bossNum == NPCID.TheDestroyer) {
				return new List<int>()
				{
					ItemID.DestroyerBossBag,
					ItemID.MechanicalWagonPiece,
					ItemID.SoulofMight,
					ItemID.HallowedBar,
					ItemID.GreaterHealingPotion
				};
			}
			if (bossNum == NPCID.SkeletronPrime) {
				return new List<int>()
				{
					ItemID.SkeletronPrimeBossBag,
					ItemID.MechanicalBatteryPiece,
					ItemID.SoulofFright,
					ItemID.HallowedBar,
					ItemID.GreaterHealingPotion
				};
			}
			if (bossNum == NPCID.Plantera) {
				return new List<int>()
				{
					ItemID.PlanteraBossBag,
					ItemID.SporeSac,
					ItemID.TempleKey,
					ItemID.Seedling,
					ItemID.TheAxe,
					ItemID.PygmyStaff,
					ItemID.GrenadeLauncher,
					ItemID.VenusMagnum,
					ItemID.NettleBurst,
					ItemID.LeafBlower,
					ItemID.FlowerPow,
					ItemID.WaspGun,
					ItemID.Seedler,
					ItemID.ThornHook,
					ItemID.GreaterHealingPotion
				};
			}
			if (bossNum == NPCID.Golem) {
				return new List<int>()
				{
					ItemID.GolemBossBag,
					ItemID.ShinyStone,
					ItemID.Stynger,
					ItemID.PossessedHatchet,
					ItemID.SunStone,
					ItemID.EyeoftheGolem,
					ItemID.Picksaw,
					ItemID.HeatRay,
					ItemID.StaffofEarth,
					ItemID.GolemFist,
					ItemID.BeetleHusk,
					ItemID.GreaterHealingPotion
				};
			}
			if (bossNum == NPCID.DD2Betsy) {
				return new List<int>()
				{
					ItemID.BossBagBetsy,
					ItemID.BetsyWings,
					ItemID.DD2BetsyBow, // Aerial Bane
                    ItemID.MonkStaffT3, // Sky Dragon's Fury
                    ItemID.ApprenticeStaffT3, // Betsy's Wrath
                    ItemID.DD2SquireBetsySword // Flying Dragon
                };
			}
			if (bossNum == NPCID.DukeFishron) {
				return new List<int>()
				{
					ItemID.FishronBossBag,
					ItemID.ShrimpyTruffle,
					ItemID.FishronWings,
					ItemID.BubbleGun,
					ItemID.Flairon,
					ItemID.RazorbladeTyphoon,
					ItemID.TempestStaff,
					ItemID.Tsunami,
					ItemID.GreaterHealingPotion
				};
			}
			if (bossNum == NPCID.CultistBoss) {
				return new List<int>()
				{
					ItemID.CultistBossBag,
					ItemID.LunarCraftingStation,
					ItemID.GreaterHealingPotion
				};
			}
			if (bossNum == NPCID.MoonLordHead) {
				return new List<int>()
				{
					ItemID.MoonLordBossBag,
					ItemID.GravityGlobe,
					ItemID.PortalGun,
					ItemID.LunarOre,
					ItemID.Meowmere,
					ItemID.Terrarian,
					ItemID.StarWrath,
					ItemID.SDMG,
					ItemID.FireworksLauncher, // The Celebration
                    ItemID.LastPrism,
					ItemID.LunarFlareBook,
					ItemID.RainbowCrystalStaff,
					ItemID.MoonlordTurretStaff, // Lunar Portal Staff
                    ItemID.SuspiciousLookingTentacle,
					ItemID.GreaterHealingPotion
				};
			}
			return new List<int>();
		}

		internal protected List<int> SetupCollect(int bossNum) {
			if (bossNum == NPCID.KingSlime) {
				return new List<int>()
				{
					ItemID.KingSlimeTrophy,
					ItemID.KingSlimeMask,
					ItemID.MusicBoxBoss1
				};
			}
			if (bossNum == NPCID.EyeofCthulhu) {
				return new List<int>()
				{
					ItemID.EyeofCthulhuTrophy,
					ItemID.EyeMask,
					ItemID.MusicBoxBoss1
				};
			}
			if (bossNum == NPCID.EaterofWorldsHead) {
				return new List<int>()
				{
					ItemID.EaterofWorldsTrophy,
					ItemID.EaterMask,
					ItemID.MusicBoxBoss1
				};
			}
			if (bossNum == NPCID.BrainofCthulhu) {
				return new List<int>()
				{
					ItemID.BrainofCthulhuTrophy,
					ItemID.BrainMask,
					ItemID.MusicBoxBoss3
				};
			}
			if (bossNum == NPCID.QueenBee) {
				return new List<int>()
				{
					ItemID.QueenBeeTrophy,
					ItemID.BeeMask,
					ItemID.MusicBoxBoss4
				};
			}
			if (bossNum == NPCID.SkeletronHead) {
				return new List<int>()
				{
					ItemID.SkeletronTrophy,
					ItemID.SkeletronMask,
					ItemID.MusicBoxBoss1
				};
			}
			if (bossNum == NPCID.WallofFlesh) {
				return new List<int>()
				{
					ItemID.WallofFleshTrophy,
					ItemID.FleshMask,
					ItemID.MusicBoxBoss2
				};
			}
			if (bossNum == NPCID.Retinazer) {
				return new List<int>()
				{
					ItemID.RetinazerTrophy,
					ItemID.SpazmatismTrophy,
					ItemID.TwinMask,
					ItemID.MusicBoxBoss2
				};
			}
			if (bossNum == NPCID.TheDestroyer) {
				return new List<int>()
				{
					ItemID.DestroyerTrophy,
					ItemID.DestroyerMask,
					ItemID.MusicBoxBoss3
				};
			}
			if (bossNum == NPCID.SkeletronPrime) {
				return new List<int>()
				{
					ItemID.SkeletronPrimeTrophy,
					ItemID.SkeletronPrimeMask,
					ItemID.MusicBoxBoss1
				};
			}
			if (bossNum == NPCID.Plantera) {
				return new List<int>()
				{
					ItemID.PlanteraTrophy,
					ItemID.PlanteraMask,
					ItemID.MusicBoxPlantera
				};
			}
			if (bossNum == NPCID.Golem) {
				return new List<int>()
				{
					ItemID.GolemTrophy,
					ItemID.GolemMask,
					ItemID.MusicBoxBoss5
				};
			}
			if (bossNum == NPCID.DD2Betsy) {
				return new List<int>()
				{
					ItemID.BossTrophyBetsy,
					ItemID.BossMaskBetsy,
					ItemID.MusicBoxDD2
				};
			}
			if (bossNum == NPCID.DukeFishron) {
				return new List<int>()
				{
					ItemID.DukeFishronTrophy,
					ItemID.DukeFishronMask,
					ItemID.MusicBoxBoss1
				};
			}
			if (bossNum == NPCID.CultistBoss) {
				return new List<int>()
				{
					ItemID.AncientCultistTrophy,
					ItemID.BossMaskCultist,
					ItemID.MusicBoxBoss5
				};
			}
			if (bossNum == NPCID.MoonLordHead) {
				return new List<int>()
				{
					ItemID.MoonLordTrophy,
					ItemID.BossMaskMoonlord,
					ItemID.MusicBoxLunarBoss
				};
			}
			return new List<int>();
		}

		internal void AddBoss(string bossname, float bossValue, Func<bool> bossDowned, string bossInfo = null, Func<bool> available = null) {
			SortedBosses.Add(new BossInfo(BossChecklistType.Boss, bossValue, "Unknown", bossname, new List<int>(), bossDowned, available, new List<int>(), new List<int>(), new List<int>(), null, bossInfo));
			SortedBosses.Sort((x, y) => x.progression.CompareTo(y.progression));
		}

		internal void AddMiniBoss(string bossname, float bossValue, Func<bool> bossDowned, string bossInfo = null, Func<bool> available = null) {
			SortedBosses.Add(new BossInfo(BossChecklistType.Boss, bossValue, "Unknown", bossname, new List<int>(), bossDowned, available, new List<int>(), new List<int>(), new List<int>(), null, bossInfo));
			SortedBosses.Sort((x, y) => x.progression.CompareTo(y.progression));
		}

		internal void AddEvent(string bossname, float bossValue, Func<bool> bossDowned, string bossInfo = null, Func<bool> available = null) {
			SortedBosses.Add(new BossInfo(BossChecklistType.Boss, bossValue, "Unknown", bossname, new List<int>(), bossDowned, available, new List<int>(), new List<int>(), new List<int>(), null, bossInfo));
			SortedBosses.Sort((x, y) => x.progression.CompareTo(y.progression));
		}

		// New system is better
		internal void AddBoss(float val, List<int> id, string source, string name, Func<bool> down, List<int> spawn, List<int> collect, List<int> loot, string texture) {
			if (!ModContent.TextureExists(texture)) texture = "BossChecklist/Resources/BossTextures/BossPlaceholder_byCorrina";
			SortedBosses.Add(new BossInfo(BossChecklistType.Boss, val, source, name, id, down, null, spawn, SortCollectibles(collect), loot, texture, "No info provided"));
			SortedBosses.Sort((x, y) => x.progression.CompareTo(y.progression));
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write("<<Boss Assist>> ");
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(source + " has added ");
			Console.ForegroundColor = ConsoleColor.DarkMagenta;
			Console.Write(name);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(" to the boss list!");
			Console.WriteLine();
			Console.ResetColor();
			if (BossChecklist.ServerCollectedRecords != null) {
				for (int i = 0; i < 255; i++) {
					BossChecklist.ServerCollectedRecords[i].Add(new BossStats());
				}
				// Adding a boss to each player
			}
		}
		/*
	Didnt work out as I had hoped, not completely sure why, but probably not even necessary

internal List<int> BagFirst(List<int> loot)
{
	int reserve = 0;
	foreach (int item in loot)
	{
		Item i = new Item();
		i.SetDefaults(item);
		if (i.expert || i.Name.Contains("Treasure Bag") || (i.type > ItemID.Count && i.modItem.Name.Contains("Treasure Bag")))
		{
			reserve = item;
			break;
		}
	}
	if (reserve != 0)
	{
		loot.Remove(reserve);
		loot.Insert(reserve, 0);
	}

	return loot;
}
*/

		internal List<int> SortCollectibles(List<int> collection) {
			// Sorts the Main 3 Collectibles
			List<int> SortedCollectibles = new List<int>();

			foreach (int item in collection) {
				Item i = new Item();
				i.SetDefaults(item);
				if (i.createTile > 0 && (i.Name.Contains("Trophy")) || (i.type > ItemID.Count && i.modItem.Name.Contains("Trophy"))) {
					SortedCollectibles.Add(item);
					break;
				}
			}
			if (SortedCollectibles.Count == 0) SortedCollectibles.Add(-1); // No Trophy
			foreach (int item in collection) {
				Item i = new Item();
				i.SetDefaults(item);
				if (i.vanity && (i.Name.Contains("Mask")) || (i.type > ItemID.Count && i.modItem.Name.Contains("Mask"))) {
					SortedCollectibles.Add(item);
					break;
				}
			}
			if (SortedCollectibles.Count == 1) SortedCollectibles.Add(-1); // No Mask
			foreach (int item in collection) {
				Item i = new Item();
				i.SetDefaults(item);
				if (i.createTile > 0 && (i.Name.Contains("Music Box")) || (i.type > ItemID.Count && i.modItem.Name.Contains("Music Box"))) {
					SortedCollectibles.Add(item);
					break;
				}
			}
			if (SortedCollectibles.Count == 2) SortedCollectibles.Add(-1); // No Music Box
			foreach (int item in collection) {
				if (!SortedCollectibles.Contains(item)) SortedCollectibles.Add(item);
			}

			return SortedCollectibles;
		}
		/*

		internal void AddSpawnItem(int bType, string bSource, List<int> bLoot)
		{
			int index = SortedBosses.FindIndex(x => x.id == bType && x.source == bSource);
			if (index != -1)
			{
				foreach (int item in bLoot)
				{
					SortedBosses[index].loot.Add(item);
				}
			}
		}

		internal void AddToLootTable(int bType, string bSource, List<int> bLoot)
        {
            int index = SortedBosses.FindIndex(x => x.id == bType && x.source == bSource);
            if (index != -1)
            {
                foreach (int item in bLoot)
                {
                    SortedBosses[index].loot.Add(item);
                }
            }
        }

        internal void AddToCollection(int bType, string bSource, List<int> bCollect)
        {
            int index = SortedBosses.FindIndex(x => x.id == bType && x.source == bSource);
            if (index != -1)
            {
                foreach (int item in bCollect)
                {
                    SortedBosses[index].collection.Add(item);
                }
            }
        }
		*/
	}
}