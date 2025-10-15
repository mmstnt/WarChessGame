using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitChessHealthBar : MonoBehaviour
{
    [SerializeField]
    Vector2 offset;

    Slider selfBar;
    UnitChess currentUnitTarget;

    private void Awake()
    {
        selfBar = GetComponent<Slider>();
    }

    private void Update()
    {
        if (currentUnitTarget != null) 
        {
            Vector3 desPos = Camera.main.WorldToScreenPoint(currentUnitTarget.transform.position);
            transform.position = desPos + Vector3.up * offset.y + Vector3.right * offset.x;
        }
    }

    public void setUnitTarget(UnitChess unitTarget)
    {
        currentUnitTarget = unitTarget;
        changeHealth();
    }

    public void changeHealth()
    {
        selfBar.value = currentUnitTarget.battleData.curHealth * 1.0f / currentUnitTarget.battleData.healthMax;
    }
}
