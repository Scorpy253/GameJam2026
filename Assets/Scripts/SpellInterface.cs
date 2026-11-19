using UnityEngine;

public interface SpellInterface
{
    void OnCasting(Vector3 aimDir, float damage, string casterTag, Transform caster);
}