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
    [Header("文本")]
    public TextAsset dialogDataFile;
    [Header("圖像")]
    public SpriteRenderer sprite;
    [Header("文字物件")]
    public TMP_Text nameText;
    public TMP_Text dialogText;
    [Header("角色圖片")]
    public List<Sprite> characterImageList = new List<Sprite>();
    public Dictionary<string, Sprite> characterImageDic = new Dictionary<string, Sprite>();
    [Header("對話索引")]
    public int dialogIndex;
    public string[] dialogRows;
    [Header("對話選項按鈕")]
    public Transform buttleGroup;
    public GameObject dialogOptionsButton;

    private PlayerInputControl playerInputControl;
    private bool isDialog;

    private void Awake()
    {
        playerInputControl = new PlayerInputControl();

        readText(dialogDataFile);
        updataText("1","1");
    }

    private void OnEnable()
    {
        playerInputControl.Enable();
        playerInputControl.GamePlay.Confirm.started += onConfirm;
    }

    private void OnDisable()
    {
        playerInputControl.Disable();
        playerInputControl.GamePlay.Confirm.started -= onConfirm;
    }

    private void onConfirm(InputAction.CallbackContext context)
    {
        onClickNext();
    }

    public void updataText(string name, string dialog) 
    {
        nameText.text = name;
        dialogText.text = dialog;
    }

    public void updateImage(string name, string vector)
    {
        //sprite.sprite = characterImageDic[name];
    }

    public void readText(TextAsset textAsset) 
    {
        dialogRows = textAsset.text.Split("\n");
        foreach(var row in dialogRows) 
        {
            string[] cell = row.Split(",");
        }
    }

    public void onClickNext() 
    {
        showDialogRow();
    }

    public void showDialogRow() 
    {
        for (int i = 0; i < dialogRows.Length; i++)  
        {
            string[] cells = dialogRows[i].Split(",");
            if (cells[0] == "對話" && int.Parse(cells[1]) == dialogIndex)
            {
                updataText(cells[2], cells[4]);
                updateImage(cells[2], cells[3]);

                dialogIndex = int.Parse(cells[5]);
                isDialog = true;
                break;
            }
            else if (cells[0] == "選項" && int.Parse(cells[1]) == dialogIndex && isDialog)
            {
                generateOptionButton(i);
                isDialog = false;
            }
            else if (cells[0] == "結束" && int.Parse(cells[1]) == dialogIndex) 
            {
                Debug.Log("結束");
                updataText(cells[2], cells[4]);
                updateImage(cells[2], cells[3]);
                isDialog = false;
            }
        }
    }

    public void generateOptionButton(int index) 
    {
        string[] cells = dialogRows[index].Split(",");

        if (cells[0] == "選項") 
        {
            GameObject button = Instantiate(dialogOptionsButton, buttleGroup);
            button.GetComponentInChildren<TMP_Text>().text = cells[4];
            button.GetComponent<Button>().onClick.AddListener
                (
                delegate
                {
                    onOptionClick(int.Parse(cells[5]));
                    if (cells[6] != "") 
                    {
                        string[] effect = cells[6].Split("@");
                        cells[7] = Regex.Replace(cells[7], "[\r\n]", "");
                        onOptionEffect(effect[0], int.Parse(effect[1]), cells[7]);
                    }
                }
                );
            if (index + 1 < dialogRows.Length) 
            {
                generateOptionButton(index+1);
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