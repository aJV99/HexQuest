using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    private BFSResult movementRange = new BFSResult();
    private List<Vector3Int> currentPath = new List<Vector3Int>();

    public void HideRange(HexGrid hexGrid)
    {
        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            hexGrid.GetTileAt(hexPosition).DisableHighlight();
        }
        movementRange = new BFSResult();
    }

    public void ShowRange(unit selectedUnit, HexGrid hexGrid)
    {
        CalculateRange(selectedUnit, hexGrid);
        Vector3Int unitPos = hexGrid.GetClosestHex(selectedUnit.transform.position);

        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            if (unitPos == hexPosition)
                continue;
            hexGrid.GetTileAt(hexPosition).EnableHighlight();
        }
    }

    private void CalculateRange(unit selectedUnit, HexGrid hexGrid)
    {
        movementRange = GraphSearch.BFSGetRange(hexGrid, hexGrid.GetClosestHex(selectedUnit.transform.position), selectedUnit.MovementPoints);
    }

    public void ShowPath(Vector3Int selectedHexPosition, HexGrid hexGrid)
    {
        if (movementRange.GetRangePositions().Contains(selectedHexPosition))
        {
            foreach(Vector3Int hexPosition in currentPath)
            {
                hexGrid.GetTileAt(hexPosition).ResetHighlight();
            }
            currentPath = movementRange.GetPathTo(selectedHexPosition);
            foreach(Vector3Int hexPosition in currentPath)
            {
                hexGrid.GetTileAt(hexPosition).HighlightPath();
            }
        }
    }

    public void MoveUnit(unit selectedUnit, HexGrid hexGrid)
    {
        Debug.Log("Moving unit " + selectedUnit.name);
        selectedUnit.MoveThroughoutPath(currentPath.Select(pos => hexGrid.GetTileAt(pos).transform.position).ToList());
    }

    public bool IsHexInRange(Vector3Int hexPosition)
    {
        return movementRange.IsHexPositionInRange(hexPosition);
    }
        
}
