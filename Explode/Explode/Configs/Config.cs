using System.ComponentModel;

namespace Explode
{
    public class Config
    {
        [Description("Chance to explode while sprinting")]
        public float explodeChance { get; set; } = 0.1f;
    }
}