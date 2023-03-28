using System.ComponentModel;
using System.Collections.Generic;

namespace Explode
{
    public class Config
    {
        [Description("What message will be displayed when using the coin")]
        public string FlipMessage { get; set; } = "<b>You have had <color=yellow>{effect}</color> applied for 10 seconds</b>";
                                                        
        [Description("Chance to explode while sprinting")]
        public float ExplodeChance { get; set; } = 0.1f;

        [Description("Toggle whether explosions can activate on coin flip")]
        public bool IsExplosive { get; set; } = true;

        [Description("Toggle to enable or disable giving effects/items for flipping coin")]
        public bool HasEffects { get; set; } = true;
        [Description("Toggle to remove coin after being flipped")]
        public bool LoseCoin { get; set; } = true;
        [Description("Duration of time-based effects")]
        public int Duration { get; set; } = 10;

        [Description("Number of good effects. Only modify to be less than or equal to 10")]
        public int NumGoodEffects { get; set; } = 10;

        [Description("Number of badd effects. Only modify to be less than or equal to 20")]
        public int NumBadEffects { get; set; } = 20;

        [Description("Decimal representing percent chance to have the extreme effects occur. Will not completely make it 100% if set to 1")]
        public float PercentExtremes { get; set; } = 0.01f;

        [Description("Integer equivalent to SCP:SL effects, Do not modify")]
        public enum CoinEffects
        {
            BodyshotReduction = 0,
            DamageReduction,
            Invigorated,
            Invisibility,
            MovementBoost,
            SCP1853,
            SpawnProtection,
            Rainbow,
            Vitality,
            SCP207,
            HotDamn,
            Amnesia,
            Asphyxiated,
            Bleeding,
            Blinded,
            Burned,
            Cardiac,
            Concussed,
            Corroding,
            Deafened,
            Decontaminated,
            Disabled,
            Ensnared,
            Exhausted,
            Flashed,
            Hemorrhage,
            Poisoned,
            SeveredHands,
            Sinkhole,
            Stained,
            Traumatized,
            Suffer
        };
    }
}