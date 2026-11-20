using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int dmgLevel = 0;
    public int hpLevel = 0;
    public int speedLevel = 0;
    public int cdrLevel = 0;
    public int totalGold = 0;
    public int Firecoin = 0;
    public int Watercoin = 0;
    public int Earthcoin = 0;
    public int Windcoin = 0;
    public int Lightcoin = 0;
    public List<EquippedSpells> equippedSpells = new List<EquippedSpells>();
    public List<string> unlockedSpellNames = new List<string>();
}