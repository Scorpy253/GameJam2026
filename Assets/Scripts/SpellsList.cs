using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpellsList
{
    public enum Arrow { Up, Down, Left, Right }
    public string spellName;
    public int baseDamage;
    public float scaling;
    public List<Arrow> sequence = new List<Arrow>();
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
    }
}