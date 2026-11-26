using System.Collections.Generic;
using UnityEngine;

public class SpellExecutor : MonoBehaviour
{
    private static readonly Dictionary<string, GameObject> spellCache = new Dictionary<string, GameObject>();
    private PlayerInformation playerInfo;

    private void Awake()
    {
        playerInfo = GetComponent<PlayerInformation>();
    }

    public void CastSpellFromSlot(int targetSlot, Vector3 aimDir)
    {
        EquippedSpells equipped = playerInfo.currentSpells.Find(s => s.SpellSlot == targetSlot);
        if (equipped == null) return;
        GameObject prefab = GetSpellPrefab(equipped.SpellName);
        if (prefab == null) return;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        float totalDamage = equipped.SpellDmg + (playerInfo.BaseDMG * equipped.SpellScaling);
        GameObject spawned = Instantiate(prefab, transform.position, Quaternion.Euler(0f, 0f, angle));
        if (spawned.TryGetComponent(out SpellInterface spell))
            spell.OnCasting(aimDir, totalDamage, transform, playerInfo.CurrentArenaElement);
    }

    private GameObject GetSpellPrefab(string spellName)
    {
        if (spellCache.TryGetValue(spellName, out GameObject cached))
            return cached;
        GameObject loaded = Resources.Load<GameObject>($"SpellPrefabs/{spellName}");
        if (loaded != null)
            spellCache[spellName] = loaded;
        return loaded;
    }
}