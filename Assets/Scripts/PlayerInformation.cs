using UnityEngine;
using System.Collections.Generic;

public class PlayerInformation : MonoBehaviour
{
    public int BaseHP = 10;
    public float MVSPD = 10;
    public int BaseDMG = 10;
    public int cdrLevel = 0;
    public int totalGold = 0;
    public int Firecoin = 0;
    public int Watercoin = 0;
    public int Earthcoin = 0;
    public int Windcoin = 0;
    public int Lightcoin = 0;
    public List<EquippedSpells> currentSpells = new List<EquippedSpells>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpells.Add(new EquippedSpells(new Fireball(), 1)); //TESTING
        currentSpells.Add(new EquippedSpells(new Waterball(), 2)); //TESTING
        currentSpells.Add(new EquippedSpells(new Earthball(), 3)); //TESTING
        currentSpells.Add(new EquippedSpells(new Lightball(), 4)); //TESTING
        currentSpells.Add(new EquippedSpells(new Windball(), 5)); //TESTING
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddSpellToEquips(SpellsList spelltoadd,int slot)
    {
        if (spelltoadd == null) return;
        currentSpells.RemoveAll(s => s.SpellName == spelltoadd.spellName);
        EquippedSpells newEquip = new EquippedSpells(spelltoadd, slot);
        for (int i = 0; i < currentSpells.Count; i++)
        {
            if (currentSpells[i].SpellSlot == slot)
            {
                currentSpells[i] = newEquip;
                return;
            }
        }
        currentSpells.Add(newEquip);
    }
    public void TakeDamage(int totalDmg)
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
    public float SpellCooldown;
    public bool ignored;
    public EquippedSpells(SpellsList data, int slot)
    {
        SpellName = data.spellName;
        SpellDmg = data.baseDamage;
        SpellScaling = data.scaling;
        SpellSlot = slot;
        SpellArrow = data.sequence.ToArray();
        SpellCooldown = data.spellCooldown;
        ignored = data.Ignore;
    }
}
