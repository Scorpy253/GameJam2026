using UnityEngine;

public interface SpellInterface
{
    void OnCasting(Vector3 aimDir, float damage, Transform caster, SpellsList.Element element);
}