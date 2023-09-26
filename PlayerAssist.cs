﻿using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;

namespace BossChecklist
{
	public class PlayerAssist : ModPlayer {
		// For the 'never opened' button glow for players who haven't noticed the new feature yet.
		public bool hasOpenedTheBossLog;
		// When players jon a different world, the boss log PageNum should reset back to its original state
		public bool enteredWorldReset;

		// Records are bound to characters, but records are independent between worlds as well.
		// AllStored records contains every player record from every world
		// RecordsForWorld is a reference to the specfic player records of the current world
		// We split up AllStoredRecords with 'Main.ActiveWorldFileData.UniqueId.ToString()' as keys
		public Dictionary<string, List<BossRecord>> AllStoredRecords;
		public List<BossRecord> RecordsForWorld;
		public List<ItemDefinition> BossItemsCollected;

		public bool[] hasNewRecord;

		public override void Initialize() {
			hasOpenedTheBossLog = false;
			enteredWorldReset = false;

			AllStoredRecords = new Dictionary<string, List<BossRecord>>();
			RecordsForWorld = new List<BossRecord>();
			BossItemsCollected = new List<ItemDefinition>();

			hasNewRecord = Array.Empty<bool>();
		}

		public override void SaveData(TagCompound tag) {
			// Every time player data is saved, the RecordsForWorld list should be resubmitted into AllStoredRecords to properly update them
			string WorldID = Main.ActiveWorldFileData.UniqueId.ToString();
			if (AllStoredRecords.ContainsKey(WorldID)) {
				AllStoredRecords[WorldID] = RecordsForWorld;
			}

			// We cannot save dictionaries, so we'll convert it to a TagCompound instead
			TagCompound TempRecords = new TagCompound();
			foreach (KeyValuePair<string, List<BossRecord>> bossRecord in AllStoredRecords) {
				TempRecords.Add(bossRecord.Key, bossRecord.Value);
			}

			tag["BossLogPrompt"] = hasOpenedTheBossLog;
			tag["StoredRecords"] = TempRecords;
			tag["BossLootObtained"] = BossItemsCollected;
		}

		public override void LoadData(TagCompound tag) {
			hasOpenedTheBossLog = tag.GetBool("BossLogPrompt");

			// Grab the player's record data so we can grab what we need in OnEnterWorld().
			TagCompound SavedStoredRecords = tag.Get<TagCompound>("StoredRecords");
			AllStoredRecords.Clear(); // Clear the list so we can convert our TagCompund back to a Dictionary
			foreach (KeyValuePair<string, object> bossRecords in SavedStoredRecords) {
				AllStoredRecords.Add(bossRecords.Key, SavedStoredRecords.GetList<BossRecord>(bossRecords.Key).ToList());
			}

			// Prepare the collectibles for the player. Putting unloaded bosses in the back and new/existing ones up front
			BossItemsCollected = tag.GetList<ItemDefinition>("BossLootObtained").ToList();
		}

		public override void OnEnterWorld() {
			// PageNum starts out with an invalid number so jumping between worlds will always reset the BossLog when toggled
			enteredWorldReset = true;

			// Upon entering a world, determine if records already exist for a player and copy them into 'RecordsForWorld'
			RecordsForWorld.Clear(); // The list must be cleared first, otherwise the list will contain items from previously entered worlds
			string WorldID = Main.ActiveWorldFileData.UniqueId.ToString();
			if (AllStoredRecords.TryGetValue(WorldID, out List<BossRecord> tempRecords)) {
				for (int i = 0; i < BossChecklist.bossTracker.BossRecordKeys.Count; i++) {
					// If the index was found, copy it to our list
					// Otherwise the index must be invalid so a new entry must be created
					int grabIndex = tempRecords.FindIndex(x => x.bossKey == BossChecklist.bossTracker.BossRecordKeys[i]);
					if (grabIndex != -1) {
						RecordsForWorld.Add(tempRecords[grabIndex]);
					}
					else {
						RecordsForWorld.Add(new BossRecord(BossChecklist.bossTracker.BossRecordKeys[i]));
					}
				}
			}
			else {
				// If personal records do not exist for this world, create a new entry for the player to use
				foreach (string key in BossChecklist.bossTracker.BossRecordKeys) {
					RecordsForWorld.Add(new BossRecord(key));
				}
				// A new entry will be added to AllStoredRecords so that it can be saved when needed
				AllStoredRecords.Add(WorldID, RecordsForWorld);
			}

			hasNewRecord = new bool[BossChecklist.bossTracker.BossRecordKeys.Count];

			if (BossChecklist.DebugConfig.DISABLERECORDTRACKINGCODE) {
				return;
			}

			// When a player joins a world, their Records will need to be sent to the server
			// The server doesn't need player records from every world, just the current one
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)PacketMessageType.SendAllRecordsFromPlayerToServer);
				packet.Write(RecordsForWorld.Count);
				for (int i = 0; i < RecordsForWorld.Count; i++) {
					// The only records that we need to compare between other players are previous and best records
					// The statisctics and first records are strictly for the assigned client
					PersonalStats stat = RecordsForWorld[i].stats;
					packet.Write(RecordsForWorld[i].bossKey);
					packet.Write(stat.durationBest);
					packet.Write(stat.durationPrev);
					packet.Write(stat.hitsTakenBest);
					packet.Write(stat.hitsTakenPrev);
				}
				packet.Send(); // Multiplayer client --> Server
			}
		}

		// Track each tick that passes during boss fights.
		public override void PreUpdate() {
			/* Debug tool for opening the Progression Mode prompt
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
				hasOpenedTheBossLog = false;
			*/

			if (BossChecklist.DebugConfig.DISABLERECORDTRACKINGCODE || Player.whoAmI == 255)
				return;

			if (Main.netMode == NetmodeID.SinglePlayer && !BossChecklist.ClientConfig.RecordTrackingEnabled)
				return;

			List<BossRecord> EntryRecords = Main.netMode == NetmodeID.Server ? BossChecklist.ServerCollectedRecords[Player.whoAmI] : RecordsForWorld;
			foreach (BossRecord record in EntryRecords) {
				if (record.stats.IsCurrentlyBeingTracked)
					record.stats.Tracker_Duration++;
			}
		}

		// Track amount of times damage was taken during a boss fight. Source of damage does not matter.
		public override void OnHurt(Player.HurtInfo info) {
			if (BossChecklist.DebugConfig.DISABLERECORDTRACKINGCODE || Player.whoAmI == 255)
				return;

			if (Main.netMode == NetmodeID.SinglePlayer && !BossChecklist.ClientConfig.RecordTrackingEnabled)
				return;

			List<BossRecord> EntryRecords = Main.netMode == NetmodeID.Server ? BossChecklist.ServerCollectedRecords[Player.whoAmI] : RecordsForWorld;
			foreach (BossRecord record in EntryRecords) {
				if (record.stats.IsCurrentlyBeingTracked)
					record.stats.Tracker_HitsTaken++;
			}
		}

		// Track player deaths during boss fights.
		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
			if (BossChecklist.DebugConfig.DISABLERECORDTRACKINGCODE || Player.whoAmI == 255)
				return;

			if (Main.netMode == NetmodeID.SinglePlayer && !BossChecklist.ClientConfig.RecordTrackingEnabled)
				return;

			List<BossRecord> EntryRecords = Main.netMode == NetmodeID.Server ? BossChecklist.ServerCollectedRecords[Player.whoAmI] : RecordsForWorld;
			foreach (BossRecord record in EntryRecords) {
				if (record.stats.IsCurrentlyBeingTracked)
					record.stats.Tracker_Deaths++;
			}
		}

		// Record tracking should stop if the player disconnects from the world.
		public override void PlayerDisconnect() {
			if (BossChecklist.DebugConfig.DISABLERECORDTRACKINGCODE || Player.whoAmI == 255)
				return;

			if (Main.netMode == NetmodeID.SinglePlayer && !BossChecklist.ClientConfig.RecordTrackingEnabled)
				return;

			if (Main.netMode == NetmodeID.Server) {
				foreach (BossRecord record in BossChecklist.ServerCollectedRecords[Player.whoAmI]) {
					BossChecklist.bossTracker.SortedEntries.Find(x => x.Key == record.bossKey).IsRecordIndexed(out int recordIndex);
					record.stats.StopTracking_Server(Player.whoAmI, recordIndex, false, false);
				}
			}
			else {
				foreach (BossRecord record in RecordsForWorld) {
					record.stats.StopTracking(false, false); // Note: Disconnecting still tracks attempts and deaths. Does not save last attempt data.
				}
			}
		}
		
		// Respawn timer feature
		public override void UpdateDead() {
			if (Main.netMode == NetmodeID.Server || Player.whoAmI == 255)
				return;

			// Timer sounds when a player is about to respawn
			if (BossChecklist.ClientConfig.TimerSounds && Player.respawnTimer > 0 && Player.respawnTimer <= 180 && Player.respawnTimer % 60 == 0)
				SoundEngine.PlaySound(SoundID.MaxMana);
		}
	}
}
