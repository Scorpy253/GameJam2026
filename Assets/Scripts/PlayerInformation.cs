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
    private int startingHP;
    private float startingSpeed;
    private int startingDmg;
    public SpellsList.Element CurrentArenaElement = SpellsList.Element.Wind;
    public void SetArenaElement(SpellsList.Element newElement)
    {
        CurrentArenaElement = newElement;
    }
    void Awake()
    {
        startingHP = BaseHP;
        startingSpeed = MVSPD;
        startingDmg = BaseDMG;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadFromSave();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void LoadFromSave()
    {
        SaveData data = SaveSystem.Load();

        BaseHP = startingHP + (data.hpLevel * 5);
        MVSPD = startingSpeed + (data.speedLevel * 0.5f);
        BaseDMG = startingDmg + (data.dmgLevel * 2);

        totalGold = data.totalGold;
        Firecoin = data.Firecoin;
        Watercoin = data.Watercoin;
        Earthcoin = data.Earthcoin;
        Windcoin = data.Windcoin;
        Lightcoin = data.Lightcoin;

        currentSpells = new List<EquippedSpells>(data.equippedSpells);
    }
    public void AddSpellToEquips(SpellsList spelltoadd, int slot)
    {
        currentSpells.RemoveAll(s => s.SpellName == spelltoadd.spellName);
        EquippedSpells newEquip = new EquippedSpells(spelltoadd, slot);

        for (int i = 0; i < currentSpells.Count; i++)
        {
            if (currentSpells[i].SpellSlot == slot)
            {
                currentSpells[i] = newEquip;
                SaveCurrentData();
                return;
            }
        }

        currentSpells.Add(newEquip);
        SaveCurrentData();
    }
    private void SaveCurrentData()
    {
        SaveData data = SaveSystem.Load();
        data.totalGold = totalGold;
        data.Firecoin = Firecoin;
        data.Watercoin = Watercoin;
        data.Earthcoin = Earthcoin;
        data.Windcoin = Windcoin;
        data.Lightcoin = Lightcoin;
        data.equippedSpells = new List<EquippedSpells>(currentSpells);
        SaveSystem.Save(data);
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