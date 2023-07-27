namespace Explode
{
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Core;
    using PluginAPI.Core.Items;
    using PluginAPI.Core.Zones;
    using UnityEngine;
    using Footprinting;
    using InventorySystem;
    using InventorySystem.Items;
    using InventorySystem.Items.ThrowableProjectiles;
    using PlayerStatsSystem;
    using CustomPlayerEffects;
    using System;

    public class EventHandlers
    {
        [PluginEvent(ServerEventType.PlayerPreCoinFlip)]
        public void OnPlayerPreCoinFlip(Player player)
        {
            float randNext = (float)Plugin.Random.NextDouble();
            Log.Info(randNext.ToString());
            if (randNext <= Plugin.Instance.Config.ExplodeChance && Plugin.Instance.Config.IsExplosive)
            {
                InventoryItemLoader.AvailableItems.TryGetValue(ItemType.GrenadeHE, out ItemBase itemBase);
                ThrowableItem throwableItem = (ThrowableItem)itemBase;
                ExplosionGrenade.Explode(new Footprint(player.ReferenceHub), player.Position, (ExplosionGrenade)throwableItem.Projectile);
            }
        }
        [PluginEvent(ServerEventType.PlayerCoinFlip)]
        public void OnPlayerCoinFlip(Player player, bool isTails)
        {
            Log.Info(isTails.ToString());
            // On Heads, give a positive bonus, on Tails, give a negative effect
            if (!isTails && Plugin.Instance.Config.HasEffects)
            {
                // Gets a random positive effect
                int maxGood = (int)Math.Ceiling(Plugin.Instance.Config.NumGoodEffects * 10* Plugin.Instance.Config.PercentExtremes);
                maxGood += Plugin.Instance.Config.NumGoodEffects * 10;
                ApplyEffect(player, Plugin.Random.Next(0, maxGood));
            }
            else if(Plugin.Instance.Config.HasEffects)
            {
                // Gets a random negative effect
                int maxBad = (int)Math.Ceiling(Plugin.Instance.Config.NumBadEffects * 10 * Plugin.Instance.Config.PercentExtremes);
                maxBad += Plugin.Instance.Config.NumBadEffects * 10 + Plugin.Instance.Config.NumGoodEffects+ 10;
                ApplyEffect(player, Plugin.Random.Next(Plugin.Instance.Config.NumGoodEffects * 10+10, maxBad));
            }
            // Otherwise, do nothing

            // Removes the coin if loseCoin is set to true in config
            if(Plugin.Instance.Config.LoseCoin)
            {
                player.RemoveItem(new Item(player.CurrentItem));
            }
        }
        private void ApplyEffect(Player player, int effect)
        {
            Log.Info(effect.ToString());
            // Rounds the effect back down to the enum value
            int value = (int)Math.Floor(effect / 10.0f);
            Log.Info(value.ToString());

            if (value == (int)Config.CoinEffects.Corroding)
            {
                player.ReceiveHint("<b>You have had <color=yellow>Corrosion</color> applied</b>", 2.0f);
            }
            else
            {
                player.ReceiveHint(
                    Plugin.Instance.Config.FlipMessage.Replace("{effect}",((Config.CoinEffects)value).ToString()), 2.0f);
            }
            // Applies the effect to the player
            switch (value)
            {
                case (int)Config.CoinEffects.BodyshotReduction:
                    player.EffectsManager.EnableEffect<BodyshotReduction>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.DamageReduction:
                    player.EffectsManager.EnableEffect<DamageReduction>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Invigorated:
                    player.EffectsManager.EnableEffect<Invigorated>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Invisibility:
                    player.EffectsManager.EnableEffect<Invisible>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.MovementBoost:
                    player.EffectsManager.EnableEffect<MovementBoost>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.SCP1853:
                    player.EffectsManager.EnableEffect<Scp1853>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.SpawnProtection:
                    player.EffectsManager.EnableEffect<SpawnProtected>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Rainbow:
                    player.EffectsManager.EnableEffect<RainbowTaste>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Vitality:
                    player.EffectsManager.EnableEffect<Vitality>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.SCP207:
                    player.EffectsManager.EnableEffect<Scp207>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Shield:
                    player.GetStatModule<AhpStat>().ServerAddProcess(30);
                    break;
                case (int)Config.CoinEffects.Overheal:
                    player.Health = 150;
                    break;
                case (int)Config.CoinEffects.Freedom:
                    TeleportPlayer(player);
                    break;
                case (int)Config.CoinEffects.HotDamn:
                    player.GetStatModule<AhpStat>().ServerAddProcess(30);
                    player.Health = 150;
                    TeleportPlayer(player);
                    player.EffectsManager.EnableEffect<BodyshotReduction>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<DamageReduction>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Invigorated>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Invisible>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<MovementBoost>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Scp1853>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<SpawnProtected>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<RainbowTaste>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Vitality>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Scp207>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.DisableEffect<Poisoned>();
                    break;
                // Negative effects begin here
                case (int)Config.CoinEffects.Amnesia:
                    player.EffectsManager.EnableEffect<AmnesiaVision>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<AmnesiaItems>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Asphyxiated:
                    player.EffectsManager.EnableEffect<Asphyxiated>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Bleeding:
                    player.EffectsManager.EnableEffect<Bleeding>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Blinded:
                    player.EffectsManager.EnableEffect<Blinded>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Burned:
                    player.EffectsManager.EnableEffect<Burned>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Cardiac:
                    player.EffectsManager.EnableEffect<CardiacArrest>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Concussed:
                    player.EffectsManager.EnableEffect<Concussed>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Corroding:
                    player.EffectsManager.EnableEffect<Corroding>(10000);
                    break;
                case (int)Config.CoinEffects.Deafened:
                    player.EffectsManager.EnableEffect<Deafened>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Decontaminated:
                    player.EffectsManager.EnableEffect<Decontaminating>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Disabled:
                    player.EffectsManager.EnableEffect<Disabled>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Ensnared:
                    player.EffectsManager.EnableEffect<Ensnared>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Exhausted:
                    player.EffectsManager.EnableEffect<Exhausted>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Flashed:
                    player.EffectsManager.EnableEffect<Flashed>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Hemorrhage:
                    player.EffectsManager.EnableEffect<Hemorrhage>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Poisoned:
                    player.EffectsManager.EnableEffect<Poisoned>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.SeveredHands:
                    player.EffectsManager.EnableEffect<SeveredHands>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Sinkhole:
                    player.EffectsManager.EnableEffect<Sinkhole>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Stained:
                    player.EffectsManager.EnableEffect<Stained>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Traumatized:
                    player.EffectsManager.EnableEffect<Traumatized>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.SCP939Vision:
                    player.EffectsManager.EnableEffect<InsufficientLighting>(Plugin.Instance.Config.Duration);
                    break;
                case (int)Config.CoinEffects.Suffer:
                    player.EffectsManager.EnableEffect<InsufficientLighting>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<AmnesiaVision>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<AmnesiaItems>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Asphyxiated>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Bleeding>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Blinded>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Burned>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<CardiacArrest>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Concussed>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Corroding>(10000);
                    player.EffectsManager.EnableEffect<Deafened>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Decontaminating>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Disabled>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Ensnared>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Exhausted>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Flashed>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Hemorrhage>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Poisoned>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<SeveredHands>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Sinkhole>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Stained>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Traumatized>(Plugin.Instance.Config.Duration);
                    player.EffectsManager.EnableEffect<Decontaminating>(Plugin.Instance.Config.Duration);
                    break;
            }
        }

        private void TeleportPlayer(Player player)
        {
            // Random using valid rooms undder the Room Name enum
            int room = Plugin.Random.Next(1, 38);
            while(room == 35 || room == 26 || room == 37 || room == 27 || room == 28 || room == 7 || room == 30)
            {
                room = Plugin.Random.Next(1, 38);
            }
            foreach(var facRoom in Facility.Rooms)
            {
                if((int)facRoom.Identifier.Name == room)
                {
                    Log.Info(room.ToString());
                    player.GameObject.transform.position = facRoom.Transform.position + Vector3.up;
                }
            }
        }
    }
}
