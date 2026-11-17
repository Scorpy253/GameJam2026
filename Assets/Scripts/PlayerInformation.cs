using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerInformation : MonoBehaviour
{
    public int BaseHP = 10;
    public float MVSPD = 2;
    public int BaseDMG = 10;
    public List<EquippedSpells> currentSpells = new List<EquippedSpells>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpells.Add(new EquippedSpells(new Fireball(), 1)); //TESTING
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class EquippedSpells
{
    public string SpellName;
    public int SpellDmg;
    public float SpellScaling;
    public int SpellSlot;
    public SpellsList.Arrow[] SpellArrow;
    public EquippedSpells(SpellsList data, int slot)
    {
        SpellName = data.spellName;
        SpellDmg = data.baseDamage;
        SpellScaling = data.scaling;
        SpellSlot = slot;
        SpellArrow = data.sequence.ToArray();
    }
}
