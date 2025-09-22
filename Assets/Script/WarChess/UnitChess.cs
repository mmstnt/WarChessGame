using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Outline))]
public class UnitChess : MonoBehaviour
{
    public int test;
    public BattleData battleData;

    public UnitChessState currentState;
    public HexRenderer currentCell;

    private Outline selfOutline;

    public void Awake()
    {
        selfOutline = GetComponent<Outline>();
    }

    //public void OnValidate()
    //{
    //    Dictionary<BasicAttribute, int> testAttribute = new Dictionary<BasicAttribute, int>();
    //    foreach (BasicAttribute attribute in Enum.GetValues(typeof(BasicAttribute)))
    //    {
    //        testAttribute[attribute] = test;
    //    }
    //    battleData = new BattleData(testAttribute);
    //}

    public void preSelect()
    {
        if (currentState == UnitChessState.Select)
            return;
        selfOutline.enabled = true;
        selfOutline.OutlineColor = Color.white;
        currentState = UnitChessState.PreSelect;
    }

    public void enableSelect() 
    {
        selfOutline.OutlineColor = Color.blue;
        currentState = UnitChessState.Select;
    }

    public void disableSelect() 
    {
        if (currentState == UnitChessState.Select) 
            return;
        selfOutline.enabled = false;
        currentState = UnitChessState.Idle;
    }

    public void setCell(HexRenderer cell)
    { 
        currentCell = cell;
        transform.position = cell.transform.position;
    }

    public void moveToCell(List<HexRenderer> path) 
    {
        StartCoroutine(moveCor(path));
    }

    private IEnumerator moveCor(List<HexRenderer> path) 
    {
        while (path.Count > 0) 
        {
            float workTime = 0;
            Vector3 originPos = transform.position;
            Vector3 desPos = path[0].transform.position;

            GameObject tmp = new GameObject();
            tmp.transform.position = transform.position;
            tmp.transform.LookAt(desPos);
            Quaternion originRot = transform.rotation;
            Quaternion desRot = tmp.transform.rotation;
            Destroy(tmp);

            while (true)
            {
                workTime += Time.deltaTime;
                transform.position = Vector3.Lerp(originPos, desPos, workTime);
                transform.rotation = Quaternion.Lerp(originRot, desRot, workTime * 2);

                if (workTime >= 1)
                {
                    currentCell = path[0];
                    path.RemoveAt(0);
                    break;
                }

                yield return null;
            }
        }

        WarChessManager.instance.switchState(WarChessManagerState.ChooseChess);
    }
}
