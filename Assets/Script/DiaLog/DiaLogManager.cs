using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DiaLogManager : MonoBehaviour
{
    [Header("監聽")]
    public VoidEventSO gameConfirmEvent;
    [Header("文本")]
    public TextAsset dialogDataFile;
    [Header("圖像")]
    public SpriteRenderer sprite;
    [Header("文字物件")]
    public TMP_Text nameText;
    public TMP_Text dialogText;
    [Header("角色圖片")]
    public List<Sprite> characterImageList = new List<Sprite>();
    [Header("對話索引")]
    public int dialogIndex;
    //public string[] dialogRows;
    public Dictionary<string, TextSO> dialogRowDic = new Dictionary<string, TextSO>();
    public Dictionary<string, Transform> headImageDic = new Dictionary<string, Transform>();
    [Header("組件")]
    public Transform buttleGroup;
    public Transform headGroup;
    public GameObject dialogOptionsButton;
    public GameObject headImage;

    private bool isDialog;

    private void Awake()
    {
        readText(dialogDataFile);
    }

    private void OnEnable()
    {
        gameConfirmEvent.onEventRaised += onGameConfirmEvent;
    }

    private void OnDisable()
    {
        gameConfirmEvent.onEventRaised -= onGameConfirmEvent;
    }

    public void readText(TextAsset textAsset) 
    {
        string[] dialogRows = textAsset.text.Split("\n");
        foreach(var row in dialogRows) 
        {
            string[] cell = row.Split("\t");
            TextSO textSO = new TextSO();
            textSO.Type = cell[0];
            textSO.ID = cell[1];
            textSO.Character = cell[2];
            textSO.Site = cell[3];
            textSO.Content = cell[4];
            textSO.ToID = cell[5];
            textSO.Effect = cell[6];
            textSO.Target = cell[7];
            dialogRowDic[textSO.ID] = textSO;
        }
    }

    public void updataText(string name, string dialog) 
    {
        nameText.text = name;
        dialogText.text = dialog;
    }

    public void updateImage(string name, string site)
    {
        foreach (var headImage in headImageDic) 
        {
            headImage.Value.GetComponent<Image>().color = Color.gray;
        }
        string[] siteRow = site.Split(";");
        foreach (var row in siteRow)
        {
            string[] siteEffect = row.Split(",");
            if (siteEffect[0] == "show") 
            {
                if (!headImageDic.ContainsKey(siteEffect[1]))
                {
                    GameObject head = Instantiate(headImage, headGroup);
                    headImageDic[siteEffect[1]] = head.transform;
                    headImageDic[siteEffect[1]].GetComponent<Image>().sprite = DataManager.instance.gameData.characterDic[siteEffect[1]].characterImage;
                    headImageDic[siteEffect[1]].GetComponent<Image>().color = Color.white;
                    headImageDic[siteEffect[1]].position = new Vector2(float.Parse(siteEffect[2]), float.Parse(siteEffect[3]));
                    headImageDic[siteEffect[1]].rotation = Quaternion.Euler(0, siteEffect[3] == "0" ? 0 : 180, 0);
                    headImageDic[siteEffect[1]].SetAsLastSibling();
                }
                else
                {
                    headImageDic[siteEffect[1]].GetComponent<Image>().sprite = DataManager.instance.gameData.characterDic[siteEffect[1]].characterImage;
                    headImageDic[siteEffect[1]].GetComponent<Image>().color = Color.white;
                    headImageDic[siteEffect[1]].GetComponent<DialogHead>().moveTo(new Vector2(float.Parse(siteEffect[2]), float.Parse(siteEffect[3])),5);
                    headImageDic[siteEffect[1]].rotation = Quaternion.Euler(0, siteEffect[3] == "0" ? 0 : 180, 0);
                    headImageDic[siteEffect[1]].SetAsLastSibling();
                }
            }
        }
        
        //sprite.sprite = characterImageDic[name];
    }

    private void onGameConfirmEvent()
    {
        onClickNext();
    }

    public void onClickNext() 
    {
        showDialogRow();
    }

    public void showDialogRow() 
    {
        TextSO row = dialogRowDic[dialogIndex.ToString()];
        if (row.Type == "對話")
        {
            updataText(row.Character, row.Content);
            updateImage(row.Character, row.Site);

            dialogIndex = int.Parse(row.ToID);
            isDialog = true;
        }
        else if (row.Type == "選項" && isDialog)
        {
            generateOptionButton(dialogIndex);
            isDialog = false;
        }
        else if (row.Type == "結束")
        {
            Debug.Log("結束");
            updataText(row.Character, row.Content);
            updateImage(row.Character, row.Site);
            isDialog = false;
        }
    }

    public void generateOptionButton(int index) 
    {
        TextSO row = dialogRowDic[index.ToString()];

        if (row.Type == "選項")
        {
            GameObject button = Instantiate(dialogOptionsButton, buttleGroup);
            button.GetComponentInChildren<TMP_Text>().text = row.Content;
            button.GetComponent<Button>().onClick.AddListener
                (
                delegate
                {
                    onOptionClick(int.Parse(row.ToID));
                    if (row.Effect != "")
                    {
                        string[] effect = row.Effect.Split("@");
                        row.Target = Regex.Replace(row.Target, "[\r\n]", "");
                        onOptionEffect(effect[0], int.Parse(effect[1]), row.Target);
                    }
                }
                );
            if (dialogRowDic.ContainsKey((index + 1).ToString()))
            {
                generateOptionButton(index + 1);
            }
        }
    }

    public void onOptionClick(int id) 
    {
        dialogIndex = id;
        showDialogRow();
        for (int i = 0; i < buttleGroup.childCount; i++) 
        {
            Destroy(buttleGroup.GetChild(i).gameObject);
        }
    }

    public void onOptionEffect(string effect,int param,string target) 
    {
        
    }
}