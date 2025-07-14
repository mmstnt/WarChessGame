using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "CharacterSO/CharacterListSO")]
public class CharacterListSO : ScriptableObject
{
    public List<CharacterSO> charactersList;
}
