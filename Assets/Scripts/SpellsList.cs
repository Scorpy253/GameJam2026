using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SpellsList
{
    public enum Arrow { Up, Down, Left, Right }
    public enum Element { Fire, Water, Earth, Wind, Lightning }
    public string spellName;
    public int baseDamage;
    public float scaling;
    public List<Arrow> sequence = new List<Arrow>();
    public float spellCooldown;
    public bool Ignore = false;
    public Element element;
    public string Desc;
}

class Fireball : SpellsList //DONE
{
    public Fireball()
    {
        spellName = "Fireball";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Down, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Fire;
        Desc = "A basic Fireball, does damage in an small AOE";
    }
}

class Iceball : SpellsList
{
    public Iceball()
    {
        spellName = "Iceball";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Down, Arrow.Down, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Water;
        Desc = "A basic Iceball, slows the first enemy hit and does damage";
    }
}

class Earthball : SpellsList
{
    public Earthball()
    {
        spellName = "Earthball";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Right, Arrow.Down, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Earth;
        Desc = "A basic Earthball, a rock thrown that knocks back enemies hit";
    }
}

class Windball : SpellsList
{
    public Windball()
    {
        spellName = "Windball";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Up, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Wind;
        Desc = "A basic Windball, gives the player move speed when it damages an enemy";
    }
}

class Lightningball : SpellsList
{
    public Lightningball()
    {
        spellName = "Lightningball";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Left, Arrow.Down, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Lightning;
        Desc = "A basic Lightningball, slow moving orb that shocks enemies near it";
    }
}

class FireBlast : SpellsList
{
    public FireBlast()
    {
        spellName = "Fire Blast";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Up, Arrow.Down, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Fire;
        Desc = "A bigger fireball, explodes in a much larger AOE";
    }
}

class IceSpear : SpellsList
{
    public IceSpear()
    {
        spellName = "Ice Spear";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Down, Arrow.Up, Arrow.Up, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Water;
        Desc = "A sharp spear of ice that pierces and freezes a single target";
    }
}

class Earthquake : SpellsList
{
    public Earthquake()
    {
        spellName = "Earthquake";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Right, Arrow.Down, Arrow.Down, Arrow.Left };
        spellCooldown = 5f;
        element = Element.Earth;
        Desc = "A violent tremor around the player that stuns nearby enemies at intervals";
    }
}

class GaleForce : SpellsList
{
    public GaleForce()
    {
        spellName = "Gale Force";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Left, Arrow.Left, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Wind;
        Desc = "A powerful gust of wind in a cone that pushes back the closest enemies";
    }
}

class LightningStrike : SpellsList
{
    public LightningStrike()
    {
        spellName = "Lightning Strike";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Left, Arrow.Up, Arrow.Down, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Lightning;
        Desc = "A bolt of lightning from the sky that automatically smites a nearby enemy";
    }
}

class FlamePath : SpellsList
{
    public FlamePath()
    {
        spellName = "Flame Path";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Left, Arrow.Right, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Fire;
        Desc = "A fiery trail left behind the player that damages enemies for a duration";
    }
}

class IceAge : SpellsList
{
    public IceAge()
    {
        spellName = "Ice Age";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Down, Arrow.Left, Arrow.Right, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Water;
        Desc = "A massive ice storm that sweeps across the arena and freezes all enemies";
    }
}

class MeteorShower : SpellsList
{
    public MeteorShower()
    {
        spellName = "Meteor Shower";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Right, Arrow.Up, Arrow.Down, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Earth;
        Desc = "A barrage of falling meteors that crash down and deal heavy AOE damage";
    }
}

class RazorWind : SpellsList
{
    public RazorWind()
    {
        spellName = "Razor Wind";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Right, Arrow.Left, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Wind;
        Desc = "Three sharp wind blades that home in on the weakest nearby enemy";
    }
}

class LightningSpear : SpellsList
{
    public LightningSpear()
    {
        spellName = "Lightning Spear";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Left, Arrow.Right, Arrow.Right, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Lightning;
        Desc = "An aimed spear of lightning that pierces an enemy and bounces between targets";
    }
}

class MeteorStrike : SpellsList
{
    public MeteorStrike()
    {
        spellName = "Meteor Strike";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Up, Arrow.Down, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Fire;
        Desc = "A massive leap into the air, slamming down to deal heavy AOE damage";
    }
}

class CrystalGuard : SpellsList
{
    public CrystalGuard()
    {
        spellName = "Crystal Guard";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Down, Arrow.Left, Arrow.Left, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Water;
        Desc = "A protective ice shield that blocks up to 3 hits and damages attackers";
    }
}

class EarthPillar : SpellsList
{
    public EarthPillar()
    {
        spellName = "Earth Pillar";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Right, Arrow.Up, Arrow.Up, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Earth;
        Desc = "A jagged pillar of stone that erupts from the ground, dealing AOE damage";
    }
}

class Cyclones : SpellsList
{
    public Cyclones()
    {
        spellName = "Cyclones";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Left, Arrow.Right, Arrow.Left };
        spellCooldown = 5f;
        element = Element.Wind;
        Desc = "Swirling winds around the player granting dodge chance scaled by move speed";
    }
}

class LightningBarrage : SpellsList
{
    public LightningBarrage()
    {
        spellName = "Lightning Barrage";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Left, Arrow.Down, Arrow.Left, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Lightning;
        Desc = "A self-targeted lightning strike that sends circular shockwaves across the ground";
    }
}

class FireStorm : SpellsList
{
    public FireStorm()
    {
        spellName = "Fire Storm";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Down, Arrow.Left, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Fire;
        Desc = "Pillars of fire erupting from the ground at intervals to burn enemies";
    }
}

class SnowMan : SpellsList
{
    public SnowMan()
    {
        spellName = "Snow Man";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Down, Arrow.Down, Arrow.Left, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Water;
        Desc = "A decoy snowman that distracts enemies and explodes when destroyed or attacked";
    }
}

class EarthWall : SpellsList
{
    public EarthWall()
    {
        spellName = "Earth Wall";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Right, Arrow.Left, Arrow.Left, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Earth;
        Desc = "A solid wall of rock that blocks enemies and projectiles from passing through";
    }
}

class WindDash : SpellsList
{
    public WindDash()
    {
        spellName = "Wind Dash";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Up, Arrow.Right, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Wind;
        Desc = "A rapid burst of wind that dashes the player quickly across the arena";
    }
}

class LightningWave : SpellsList
{
    public LightningWave()
    {
        spellName = "Lightning Wave";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Left, Arrow.Left, Arrow.Right, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Lightning;
        Desc = "A surging wave of electricity that travels forward and shocks enemies in its path";
    }
}

class ChargeUp : SpellsList
{
    public ChargeUp()
    {
        spellName = "Charge Up";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Right, Arrow.Up, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Fire;
        Desc = "A temporary buff that increases spell damage for each unique spell cast";
    }
}

class IceConstructs : SpellsList
{
    public IceConstructs()
    {
        spellName = "Ice Constructs";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Down, Arrow.Up, Arrow.Left, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Water;
        Desc = "Summoned ice wands that automatically fire Iceballs at nearby enemies";
    }
}

class BouncingBoulder : SpellsList
{
    public BouncingBoulder()
    {
        spellName = "Bouncing Boulder";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Right, Arrow.Up, Arrow.Left, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Earth;
        Desc = "A heavy boulder that ricochets between enemies, damaging and knocking them back";
    }
}

class VacuumControl : SpellsList
{
    public VacuumControl()
    {
        spellName = "Vacuum Control";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Down, Arrow.Up, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Wind;
        Desc = "A large vortex of air that pulls and groups enemies together in a wide AOE";
    }
}

class TormentPulse : SpellsList
{
    public TormentPulse()
    {
        spellName = "Torment Pulse";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Left, Arrow.Up, Arrow.Left, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Lightning;
        Desc = "A lingering electric field that periodically shocks any nearby enemy";
    }
}

class LavaPool : SpellsList
{
    public LavaPool()
    {
        spellName = "Lava Pool";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Down, Arrow.Down, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Fire;
        Desc = "A lingering pool of lava on the ground that damages enemies standing in it";
    }
}

class FreezeDry : SpellsList
{
    public FreezeDry()
    {
        spellName = "Freeze Dry";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Down, Arrow.Right, Arrow.Right, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Water;
        Desc = "A chilling AOE blast that damages enemies and reduces the damage they deal";
    }
}

class BoulderTransmute : SpellsList
{
    public BoulderTransmute()
    {
        spellName = "Boulder Transmute";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Right, Arrow.Right, Arrow.Down, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Earth;
        Desc = "Transforms the player into a rolling boulder that zooms through and crushes enemies";
    }
}

class WindCushion : SpellsList
{
    public WindCushion()
    {
        spellName = "Wind Cushion";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Down, Arrow.Right, Arrow.Left };
        spellCooldown = 5f;
        element = Element.Wind;
        Desc = "A protective air barrier that triggers on enemy hits to knock attackers back";
    }
}

class ShockWave : SpellsList
{
    public ShockWave()
    {
        spellName = "Shock Wave";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Left, Arrow.Down, Arrow.Down, Arrow.Up };
        spellCooldown = 5f;
        element = Element.Lightning;
        Desc = "An electric AOE burst around the player, dealing low damage with high knockback";
    }
}

class FlameBlast : SpellsList
{
    public FlameBlast()
    {
        spellName = "Flame Blast";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Down, Arrow.Left, Arrow.Left };
        spellCooldown = 5f;
        element = Element.Fire;
        Desc = "A short range blast of fire that deals heavy damage and pushes the player back";
    }
}

class SlowingAura : SpellsList
{
    public SlowingAura()
    {
        spellName = "Slowing Aura";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Down, Arrow.Left, Arrow.Up, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Water;
        Desc = "A freezing field around the player that slows down incoming enemy projectiles";
    }
}

class EarthGolem : SpellsList
{
    public EarthGolem()
    {
        spellName = "Earth Golem";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Right, Arrow.Down, Arrow.Up, Arrow.Left };
        spellCooldown = 5f;
        element = Element.Earth;
        Desc = "A summoned stone golem that launches enemies into the air for heavy fall damage";
    }
}

class Rejuvenation : SpellsList
{
    public Rejuvenation()
    {
        spellName = "Rejuvenation";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Up, Arrow.Left, Arrow.Down, Arrow.Right };
        spellCooldown = 5f;
        element = Element.Wind;
        Desc = "A soothing breeze that restores a portion of the player's health";
    }
}

class ThunderStorm : SpellsList
{
    public ThunderStorm()
    {
        spellName = "Thunder Storm";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Left, Arrow.Up, Arrow.Right, Arrow.Down };
        spellCooldown = 5f;
        element = Element.Lightning;
        Desc = "A raging storm that calls down repeated lightning bolts across a wide area";
    }
}