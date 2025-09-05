using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnMouse : MonoBehaviour
{
    public Transform currentTarget;
    public HexRenderer currentCell;

    private void Update()
    {
        mouseDeteet();
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
                if (currentCell != null && currentCell != cell) 
                {
                    currentCell.setColor(HexGridLayouts.instance.baseColor);                
                }

                currentCell = cell;
                cell.setColor(HexGridLayouts.instance.highColor);
            }
            
            //CameraControl.instance.followTransform = currentTarget;
        }
        else 
        {
            if (currentCell != null) 
            {
                currentCell.setColor(HexGridLayouts.instance.baseColor);
                currentCell = null;
            }
        }
    }
}
