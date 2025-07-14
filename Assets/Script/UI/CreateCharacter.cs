using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateCharacter : MonoBehaviour
{
    [Header("監聽")]
    public VoidEventSO gameConfirmEvent;
    public VoidEventSO createCharacterFinishEvent;
    [Header("廣播")]
    public SceneLoadEventSO sceneLoadEvent;
    [Header("組件")]
    public TMP_InputField playerNameInput;
    public TMP_Text basicAttribute;

    private Dictionary<BasicAttribute, int> basicAttributeList = new Dictionary<BasicAttribute, int>();

    private void Awake()
    {
        foreach (BasicAttribute attribute in Enum.GetValues(typeof(BasicAttribute)))
        {
            basicAttributeList[attribute] = 0;
        }
    }

    private void OnEnable()
    {
        gameConfirmEvent.onEventRaised += onGameConfirmEvent;
        createCharacterFinishEvent.onEventRaised += onCreateCharacterFinishEvent;

        randomAttribute();
    }

    private void OnDisable()
    {
        gameConfirmEvent.onEventRaised -= onGameConfirmEvent;
        createCharacterFinishEvent.onEventRaised -= onCreateCharacterFinishEvent;
    }

    private void onGameConfirmEvent()
    {
        if (playerNameInput != null && !playerNameInput.isFocused)
        {
            randomAttribute();
        }
    }

    private void randomAttribute() 
    {
        List<BasicAttribute> attributeList = new List<BasicAttribute>(basicAttributeList.Keys);
        foreach (BasicAttribute attribute in attributeList)
        {
            basicAttributeList[attribute] = UnityEngine.Random.Range(7, 19);
        }
        updateAttribute();
    }

    private void updateAttribute() 
    {
        basicAttribute.text = $"力量：{basicAttributeList[BasicAttribute.Strength]}\n" +
                              $"敏捷：{basicAttributeList[BasicAttribute.Dexterity]}\n" +
                              $"體質：{basicAttributeList[BasicAttribute.Constitution]}\n" +
                              $"智力：{basicAttributeList[BasicAttribute.Intelligence]}\n" +
                              $"感知：{basicAttributeList[BasicAttribute.Wisdom]}\n" +
                              $"魅力：{basicAttributeList[BasicAttribute.Charisma]}";
    }

    private void onCreateCharacterFinishEvent()
    {
        DataManager.instance.gameData.playerName = playerNameInput.text;
        DataManager.instance.gameData.basicAttributeList = this.basicAttributeList;
        DataManager.save("save01", DataManager.instance.gameData, "Save");

        SceneManager.instance.sceneLoadEvent.LoadRequestEvent(SceneManager.instance.dialogScene, true);
    }
}
