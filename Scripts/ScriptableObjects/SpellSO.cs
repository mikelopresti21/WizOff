using UnityEngine;

[CreateAssetMenu()]
public class SpellSO : ScriptableObject
{
    public GameObject spellPrefab;
    public string spellName;
    public Color spellEffectColor;
    public RuntimeAnimatorController spellRTAC;
    public Spell.SpellType spellType;
    public float damageAmount;
}
