namespace Explode
{
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Core;
    using PluginAPI.Core.Items;
    using UnityEngine;
    using Footprinting;
    using InventorySystem;
    using InventorySystem.Items;
    using InventorySystem.Items.ThrowableProjectiles;

    public class EventHandlers
    {
        [PluginEvent(ServerEventType.PlayerPreCoinFlip)]
        public bool OnPlayerPreCoinFlip(Player player)
        {
            float randNext = (float)Plugin.Random.NextDouble();
            Log.Info(randNext.ToString());
            if (randNext <= Plugin.Instance.Config.explodeChance)
            {
                InventoryItemLoader.AvailableItems.TryGetValue(ItemType.GrenadeHE, out ItemBase itemBase);
                ThrowableItem throwableItem = (ThrowableItem)itemBase;
                ExplosionGrenade.Explode(new Footprint(player.ReferenceHub), player.Position, (ExplosionGrenade)throwableItem.Projectile);
                return false;
            }
            return true;
        }
    }
}
