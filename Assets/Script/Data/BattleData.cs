using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleData
{
    [Header("基本資源")]
    public int healthMax;
    public int actionPointsMax;
    public int magicPointsMax;

    [Header("基本屬性")]
    public int actionPointsRecovery;
    public int magicPointsRecovery;
    public int movement;

    public float hitRate;
    public float criticalHitRate;
    public float retaliationRate;
    public float dodgeRate;

    [Header("戰鬥資源")]
    public int curHealth;
    public int curActionPoints;
    public int curMagicPoints;

    [Header("戰鬥屬性")]
    public int curActionPointsRecovery;
    public int curMagicPointsRecovery;
    public int curMovement;



    public BattleData(Dictionary<BasicAttribute, int> attributeDic)
    {
        battleAttributeConversion(attributeDic);
        curHealth = healthMax;
    }

    public void battleAttributeConversion(Dictionary<BasicAttribute, int> attributeDic)
    {
        int strength = attributeDic[BasicAttribute.Strength];
        int dexterity = attributeDic[BasicAttribute.Dexterity];
        int constitution = attributeDic[BasicAttribute.Constitution];
        int intelligence = attributeDic[BasicAttribute.Intelligence];
        int wisdom = attributeDic[BasicAttribute.Wisdom];
        int charisma = attributeDic[BasicAttribute.Charisma];

        baseAttributeConversion(strength, dexterity, constitution, intelligence, wisdom, charisma);

    }

    public void baseAttributeConversion(int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma) 
    {
        //最大生命
        healthMax = Mathf.RoundToInt(5 + ((constitution - 10) > 0 ? Mathf.Pow(constitution, 2) : Mathf.Pow(constitution, 2)) / 10);

        //資源點
        actionPointsMax = 8;
        magicPointsMax = charisma / 5;

        //資源回復
        actionPointsRecovery = 3;
        magicPointsRecovery = 1;

        //移動力
        movement = 2 + dexterity / 15;

        //命中、爆擊、反擊、閃避率
        hitRate = 100;
        criticalHitRate = 0.64f * strength + 0.32f * wisdom;
        retaliationRate = 0.56f * constitution + 0.28f * wisdom;
        dodgeRate = 0.48f * dexterity + 0.24f * wisdom;
    }
}
