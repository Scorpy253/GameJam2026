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
        element = Element.Fire;
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
        element = Element.Water;
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
        element = Element.Lightning ;
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
    }
}