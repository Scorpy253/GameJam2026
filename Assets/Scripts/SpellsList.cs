using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class SpellsList
{
    public enum Arrow { Up, Down, Left, Right }
    public string spellName;
    public int baseDamage;
    public float scaling;
    public List<Arrow> sequence = new List<Arrow>();
    public float spellCooldown;
    public bool Ignore = false;
}
class Fireball : SpellsList
{
    public Fireball()
    {
        spellName = "Fireball";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> {Arrow.Up,Arrow.Down,Arrow.Up};
        spellCooldown = 5f;
    }
}
class Waterball : SpellsList
{
    public Waterball()
    {
        spellName = "Waterball";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Down, Arrow.Down, Arrow.Up };
        spellCooldown = 5f;
    }
}
class Lightball : SpellsList
{
    public Lightball()
    {
        spellName = "Lightball";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Left, Arrow.Down, Arrow.Up };
        spellCooldown = 5f;
    }
}
class Earthball : SpellsList
{
    public Earthball()
    {
        spellName = "Windball";
        baseDamage = 10;
        scaling = 1;
        sequence.Clear();
        sequence = new List<Arrow> { Arrow.Right, Arrow.Down, Arrow.Up };
        spellCooldown = 5f;
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
    }
}