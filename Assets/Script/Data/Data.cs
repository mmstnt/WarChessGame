using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class Data
{
    public string playerName;
    public List<BasicAttribute> basicAttributeListKey;
    public List<int> basicAttributeListValue;
    public List<CharacterSO> characterList;


    public Dictionary<BasicAttribute, int> basicAttributeList = new Dictionary<BasicAttribute, int>();
    public Data(CharacterListSO characterListData) 
    {
        foreach (BasicAttribute attribute in Enum.GetValues(typeof(BasicAttribute))) 
        {
            basicAttributeList[attribute] = 0;
        }
        this.characterList = characterListData.charactersList;
    }

    public void saveDictionary() 
    {
        basicAttributeListKey = new List<BasicAttribute>(basicAttributeList.Keys);
        basicAttributeListValue = new List<int>(basicAttributeList.Values);
    }

    public void loadDictionary() 
    {
        var count = Math.Min(basicAttributeListKey.Count, basicAttributeListValue.Count);
        basicAttributeList = new Dictionary<BasicAttribute, int>(count);
        for(int i = 0; i < count; i++) 
        {
            basicAttributeList.Add(basicAttributeListKey[i], basicAttributeListValue[i]);
        }
    }
}
