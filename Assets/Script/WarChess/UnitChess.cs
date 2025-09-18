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
}
