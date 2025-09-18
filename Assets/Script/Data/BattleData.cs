using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleData
{
    [Header("基本屬性")]
    public int maxHealth;

    [Header("戰鬥屬性")]
    public int curHealth;


    public BattleData(Dictionary<BasicAttribute, int> attributeDic)
    {
        battleAttributeConversion(attributeDic);
        curHealth = maxHealth;
    }

    public void battleAttributeConversion(Dictionary<BasicAttribute, int> attributeDic)
    {
        maxHealth = maxHealthConversion(attributeDic[BasicAttribute.Constitution]);
    }

    public int maxHealthConversion(int x) 
    {
        float health = 5 + ((x - 10) > 0 ? Mathf.Pow(x, 2) : Mathf.Pow(x, 2)) / 10;
        return Mathf.RoundToInt(health);
    }
}
