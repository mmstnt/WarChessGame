using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [Header("監聽")]
    public VoidEventSO newGameEvent;
    public VoidEventSO loadGameEvent;
    [Header("資料")]
    public CharacterListSO characterList;

    public Data gameData;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        //saveData = new Data();
        //save("save2", saveData, "Save");
        //saveData = load("save2","Save");
    }

    private void OnEnable()
    {
        newGameEvent.onEventRaised += onNewGameEvent;
        loadGameEvent.onEventRaised += onLoadGameEvent;
    }

    private void OnDisable()
    {
        newGameEvent.onEventRaised -= onNewGameEvent;
        loadGameEvent.onEventRaised -= onLoadGameEvent;
    }

    private void onNewGameEvent()
    {
        gameData = new Data(characterList);
        //gameData = load("save01", "Save");
        //Debug.Log(gameData.basicAttributeDictionary[BasicAttribute.Strength]);
    }

    private void onLoadGameEvent()
    {
        gameData = load("save01", "Save");
    }

    public static void save(string name, Data data, string dir) 
    {
        data.saveDictionary();

        string jsonData = JsonUtility.ToJson(data, true);
        string filePath = $"{Application.dataPath}/{dir}";
        
        Directory.CreateDirectory(filePath);
        File.WriteAllText($"{filePath}/{name}.sav", jsonData);
    }

    public static Data load(string name, string dir) 
    {
        string filePath = $"{Application.dataPath}/{dir}/{name}.sav";
        string jsonData = null;

        try 
        {
            jsonData = File.ReadAllText(filePath);
        }
        catch (System.IO.FileNotFoundException) 
        {
            Debug.Log("找不到檔案");
            return default(Data);
        }
        catch (System.IO.DirectoryNotFoundException) 
        {
            Debug.Log("找不到路徑");
            return default(Data);
        }

        Data loadData = JsonUtility.FromJson<Data>(jsonData);
        loadData.loadDictionary();

        return loadData;
    }
}
