using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gems/Gem Data")]
public class GemData : ScriptableObject
{
    public string gemName;
    public Sprite sprite;
    public int amount;
    public float rarity;
}
