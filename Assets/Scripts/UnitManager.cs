using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private HexGrid hexGrid;

    [SerializeField]
    private MovementSystem movementSystem;

    [SerializeField]
    private PopupManager popupManager;

    public bool PlayersTurn { get; private set; } = true;

    [SerializeField]
    private unit selectedUnit;
    private Hex previouslySelectedHex;

    public void HandleUnitSelected(GameObject unit)
    {
        if (PlayersTurn == false)
            return;

        unit unitReference = unit.GetComponent<unit>();

        if (CheckIfTheSameUnitSelected(unitReference))
            return;

        PrepareUnitForMovement(unitReference);
    }

    private bool CheckIfTheSameUnitSelected(unit unitReference)
    {
        if(this.selectedUnit == unitReference)
        {
            ClearOldSelection();
            return true;
        }
        return false;
    }

    public void HandleTerrainSelected(GameObject hexGO)
    {
        if(selectedUnit == null || PlayersTurn == false)
        {
            return;
        }

        Hex selectedHex = hexGO.GetComponent<Hex>();

        if (HandleHexOutOfRange(selectedHex.HexCoords) || HandleSelectedHexIsUnitHex(selectedHex.HexCoords))
            return;

        HandleTargetHexSelected(selectedHex);
    }

    private void PrepareUnitForMovement(unit unitReference)
    {
        if(this.selectedUnit != null)
        {
            ClearOldSelection();
        }

        this.selectedUnit = unitReference;
        this.selectedUnit.Select();
        movementSystem.ShowRange(this.selectedUnit, this.hexGrid);
    }

    private void ClearOldSelection()
    {

        previouslySelectedHex = null;
        this.selectedUnit.Deselect();
        movementSystem.HideRange(this.hexGrid);
        this.selectedUnit = null;
    }

    private void HandleTargetHexSelected(Hex selectedHex)
    {
        if (previouslySelectedHex == null || previouslySelectedHex != selectedHex)
        {
            previouslySelectedHex = selectedHex;
            movementSystem.ShowPath(selectedHex.HexCoords, this.hexGrid);
        }
        else
        {
            bool hasEnemy = selectedHex.transform.Find("Props/Enemy");
            if (hasEnemy)
            {
                Enemy enemyComponent = selectedHex.transform.GetComponentInChildren<Enemy>();
                if (enemyComponent != null && enemyComponent.gameObject.activeInHierarchy)
                {
                    popupManager.ShowPopup("Enemy Power: " + enemyComponent.power, (bool isConfirmed) =>
                    {
                        if (!isConfirmed)
                        {
                            Debug.Log("Popup -> NO");
                            return;
                        }
                        Debug.Log("Popup -> YES");

                        movementSystem.MoveUnit(selectedUnit, this.hexGrid);
                        PlayersTurn = false;
                        selectedUnit.MovementFinished += ResetTurn;
                        if (selectedHex.hexType is HexType.gold)
                        {
                            selectedHex.hexType = HexType.Default;
                            this.selectedUnit.gold += 50;
                            Debug.Log("Gold");
                        }
                        ClearOldSelection();
                    });
                }
                else
                {
                    movementSystem.MoveUnit(selectedUnit, this.hexGrid);
                    PlayersTurn = false;
                    selectedUnit.MovementFinished += ResetTurn;
                    if (selectedHex.hexType is HexType.gold)
                    {
                        selectedHex.hexType = HexType.Default;
                        this.selectedUnit.gold += 50;
                        Debug.Log("Gold");
                    }
                    ClearOldSelection();
                }
            }
            else
            {
                movementSystem.MoveUnit(selectedUnit, this.hexGrid);
                PlayersTurn = false;
                selectedUnit.MovementFinished += ResetTurn;
                if (selectedHex.hexType is HexType.gold)
                {
                    selectedHex.hexType = HexType.Default;
                    this.selectedUnit.gold += 50;
                    Debug.Log("Gold");
                }
                ClearOldSelection();
            }
        }
    }

    private bool HandleSelectedHexIsUnitHex(Vector3Int hexPosition)
    {
        if(hexPosition == hexGrid.GetClosestHex(selectedUnit.transform.position))
        {
            selectedUnit.Deselect();
            ClearOldSelection();
            return true;
        }
        return false;
    }

    private bool HandleHexOutOfRange(Vector3Int hexPosition)
    {
        if(movementSystem.IsHexInRange(hexPosition) == false)
        {
            Debug.Log("Hex Out of range!");
            return true;
        }
        return false;
    }

    private void ResetTurn(unit selectedUnit)
    {
        selectedUnit.MovementFinished -= ResetTurn;
        PlayersTurn = true;
    }
}
