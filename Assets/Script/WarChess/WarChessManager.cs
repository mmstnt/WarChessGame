using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WarChessManager : MonoBehaviour
{
    public static WarChessManager instance;

    public Transform currentTarget;
    public HexRenderer currentCell;
    public UnitChess currentUnit;
    public WarChessManagerState currentState;

    public GameObject A;

    [Header("∫ ≈•")]
    public VoidEventSO mouseClickEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        mouseClickEvent.onEventRaised += mouseInput;
    }

    private void OnDisable()
    {
        mouseClickEvent.onEventRaised -= mouseInput;
    }

    private void Update()
    {
        mouseDeteet();
    }

    private void mouseInput() 
    {
        switch (currentState) 
        {
            case WarChessManagerState.PlaceChess:
                if (currentCell != null)
                {
                    GameObject unit = Instantiate(A);
                    unit.GetComponent<UnitChess>().setCell(currentCell);

                    //
                    currentState = WarChessManagerState.ChooseChess;
                }
                break;
            case WarChessManagerState.ChooseChess:
                if (currentUnit != null) 
                {
                    currentUnit.enableSelect();

                    //
                    currentState = WarChessManagerState.ActionChess;
                }
                break;
            case WarChessManagerState.ActionChess:
                if (currentCell != null) 
                {
                    currentUnit.moveToCell(HexGridLayouts.instance.pathList);
                    //
                    currentState = WarChessManagerState.MoveChess;
                }
                break;
            case WarChessManagerState.MoveChess:
                if (currentCell != null)
                {
                    
                    //
                }
                break;
            default:
                break;
        }
    }

    public void switchState(WarChessManagerState state) 
    {
        currentState = state;
    }

    private void mouseDeteet() 
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit)) 
        {
            
            currentTarget = hit.transform;  

            HexRenderer cell = currentTarget.GetComponent<HexRenderer>();
            if (cell != null) 
            {
                if (currentState == WarChessManagerState.PlaceChess) 
                {
                    if (currentCell != null && currentCell != cell)
                    {
                        currentCell.setColor(HexGridLayouts.instance.baseColor);
                    }

                    currentCell = cell;
                    currentCell.setColor(HexGridLayouts.instance.highColor);
                }
                else if (currentState == WarChessManagerState.ActionChess)
                {
                    currentCell = cell;
                    HexGridLayouts.instance.caculatePath(currentUnit.currentCell, cell);
                }
                
            }

            UnitChess unit = currentTarget.GetComponent<UnitChess>();
            if (unit != null) 
            {
                if (currentUnit != null && currentUnit != unit) 
                {
                    currentUnit.disableSelect();
                }
                currentUnit = unit;
                currentUnit.preSelect();
            }
            else 
            {
                if (currentUnit != null) 
                {
                    currentUnit.disableSelect();
                }
            }
            //CameraControl.instance.followTransform = currentTarget;
        }
        else 
        {
            if (currentCell != null && currentState != WarChessManagerState.ChooseChess)  
            {
                currentCell.setColor(HexGridLayouts.instance.baseColor);
                currentCell = null;
            }
        }
    }
}
