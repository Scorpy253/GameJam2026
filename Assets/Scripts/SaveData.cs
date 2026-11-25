//using System;
//using System.Collections.Generic;

//[Serializable]
//public class SaveData
//{
//    public int dmgLevel = 0;
//    public int hpLevel = 0;
//    public int speedLevel = 0;
//    public int fireSlots = 1;
//    public int waterSlots = 1;
//    public int earthSlots = 1;
//    public int windSlots = 1;
//    public int lightningSlots = 1;
//    public int totalGold = 0;
//    public int Firecoin = 0;
//    public int Watercoin = 0;
//    public int Earthcoin = 0;
//    public int Windcoin = 0;
//    public int Lightcoin = 0;
//    public List<EquippedSpells> equippedSpells = new List<EquippedSpells>();
//    public List<string> unlockedSpellNames = new List<string>();
//}
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int dmgLevel = 0;
    public int hpLevel = 0;
    public int speedLevel = 0;
    public int cdrLevel = 0;
    public int fireSlots = 1;
    public int waterSlots = 1;
    public int earthSlots = 1;
    public int windSlots = 1;
    public int lightningSlots = 1;
    public int totalGold = 0;
    public int Firecoin = 0;
    public int Watercoin = 0;
    public int Earthcoin = 0;
    public int Windcoin = 0;
    public int Lightcoin = 0;
    public List<EquippedSpells> equippedSpells = new List<EquippedSpells>
    {
        new EquippedSpells(new Fireball(), 1),       // Fire group (Slots 1-3)
        new EquippedSpells(new Lightningball(), 4),  // Lightning group (Slots 4-6)
        new EquippedSpells(new Iceball(), 7),      // Water group (Slots 7-9)
        new EquippedSpells(new Earthball(), 10),     // Earth group (Slots 10-12)
        new EquippedSpells(new Windball(), 13)       // Wind group (Slots 13-15)
    };
    public List<string> unlockedSpellNames = new List<string>
    {
        "Fireball",
        "Iceball",
        "Earthball",
        "Windball",
        "Lightningball"
    };
}