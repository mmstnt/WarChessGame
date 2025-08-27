using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class Data
{
    [Header("名稱")]
    public string playerName;

    [Header("屬性")]
    public Dictionary<BasicAttribute, int> basicAttributeDic = new Dictionary<BasicAttribute, int>();
    public List<BasicAttribute> basicAttributeListKey;
    public List<int> basicAttributeListValue;

    [Header("人物")]
    public Dictionary<string, CharacterSO> characterDic = new Dictionary<string, CharacterSO>();
    public List<string> characterIDList;
    public List<CharacterSO> characterList;

    public Data(CharacterListSO characterListData) 
    {
        foreach (BasicAttribute attribute in Enum.GetValues(typeof(BasicAttribute))) 
        {
            basicAttributeDic[attribute] = 0;
        }
        //this.characterList = characterListData.charactersList;

        foreach (CharacterSO characterSO in characterListData.charactersList)
        {
            //characterDictionary.Add(characterSO.characterID, characterSO);
            characterDic[characterSO.characterID] = characterSO;
        }

        //this.characterIDList = new List<int>(characterDictionary.Keys);
        //this.characterList = new List<CharacterSO>(characterDictionary.Values);
    }

    public void saveDictionary() 
    {
        basicAttributeListKey = new List<BasicAttribute>(basicAttributeDic.Keys);
        basicAttributeListValue = new List<int>(basicAttributeDic.Values);

        characterIDList = new List<string>(characterDic.Keys);
        characterList = new List<CharacterSO>(characterDic.Values);
    }

    public void loadDictionary() 
    {
        var count = Math.Min(basicAttributeListKey.Count, basicAttributeListValue.Count);
        basicAttributeDic = new Dictionary<BasicAttribute, int>(count);
        for(int i = 0; i < count; i++) 
        {
            basicAttributeDic.Add(basicAttributeListKey[i], basicAttributeListValue[i]);
        }

        count = Math.Min(characterIDList.Count, characterList.Count);
        characterDic = new Dictionary<string, CharacterSO>(count);
        for (int i = 0; i < count; i++)
        {
            characterDic.Add(characterIDList[i], characterList[i]);
        }
    }
}
