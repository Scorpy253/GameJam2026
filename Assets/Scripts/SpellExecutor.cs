using System.Collections.Generic;
using UnityEngine;

public class SpellExecutor : MonoBehaviour
{
    private static Dictionary<string, GameObject> spellCache = new Dictionary<string, GameObject>();
    private PlayerInformation playerInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerInfo = GetComponent<PlayerInformation>();
    }

    public void CastSpellFromSlot(int targetSlot, Vector3 aimDir)
    {
        if (playerInfo == null) return;
        EquippedSpells equipped = playerInfo.currentSpells.Find(s => s.SpellSlot == targetSlot);
        Debug.Log(equipped.SpellName.ToString());
        if (equipped == null) return;
        GameObject prefab = GetSpellPrefab(equipped.SpellName);
        if (prefab == null) return;

        // 3. Instantiate at player position (facing aimDir)
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        float totalDamage = equipped.SpellDmg + (playerInfo.BaseDMG * equipped.SpellScaling);
        GameObject spawned = Instantiate(prefab, transform.position, Quaternion.Euler(0f, 0f, angle));
        if (spawned.TryGetComponent(out SpellInterface spell))
        {
            spell.OnCasting(aimDir, totalDamage, gameObject.tag, transform);
        }  
    }
    private GameObject GetSpellPrefab(string spellName)
    {
        if (string.IsNullOrEmpty(spellName)) return null;

        if (spellCache.TryGetValue(spellName, out GameObject cached))
            return cached;
        GameObject loaded = Resources.Load<GameObject>($"SpellPrefabs/{spellName}");
        if (loaded == null)
            Debug.LogWarning($"Spell prefab not found at Resources/SpellPrefabs/{spellName}");
        else
            spellCache[spellName] = loaded;
        return loaded;
    }
}