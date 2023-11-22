using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    public LayerMask selectionMask;

    public UnityEvent<GameObject> OnUnitSelected;
    public UnityEvent<GameObject> TerrainSelected;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

    }

    public void HandleClick(Vector3 mousePosition)
    {
        if (IsPointerOverUIObject(mousePosition))
        {
            return;  // Return early if the mouse is over a UI element
        }

        GameObject result;
        if (FindTarget(mousePosition, out result))
        {
            if (UnitSelected(result))
            {
                OnUnitSelected?.Invoke(result);
            }
            else
            {
                TerrainSelected.Invoke(result);
            }
        }
    }

    // Check if the pointer is over any UI object
    private bool IsPointerOverUIObject(Vector3 mousePosition)
    {
        // Set up the new Pointer Event
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(mousePosition.x, mousePosition.y);

        // Create a list to store all hit objects with this ray
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast using the Graphics Raycaster and mouse click position
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // Return true if hit any UI object, otherwise false
        return results.Count > 0;
    }

    private bool UnitSelected(GameObject result)
    {
        return result.GetComponent<unit>() != null;
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit, 100, selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }
        result = null;
        return false;
    }
}
