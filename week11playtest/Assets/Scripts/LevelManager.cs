using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData[] levels; // Array of levels

    // Adjusted to account for levelIndex starting from 1
    public void InitializeLevel(int levelIndex)
    {
        int adjustedIndex = levelIndex - 1; // Adjusting for zero-based indexing
        if (adjustedIndex < 0 || adjustedIndex >= levels.Length)
        {
            Debug.LogError("Level index out of range: " + levelIndex);
            return;
        }

        LevelData currentLevel = levels[adjustedIndex];
        // Initialize or reset level-specific data here
        // For example, setting gate object active/inactive
        currentLevel.gateObject.gameObject.SetActive(true);
        foreach (Hex hex in currentLevel.hexes)
        {
            hex.hexType = HexType.Default; // Or any other initialization as needed
        }
    }

    // Method to handle gate interactions based on the current level
    public void InteractWithGate(int levelIndex)
    {
        int adjustedIndex = levelIndex - 1; // Adjusting for zero-based indexing
        if (adjustedIndex < 0 || adjustedIndex >= levels.Length)
        {
            Debug.LogError("Level index out of range: " + levelIndex);
            return;
        }

        LevelData currentLevel = levels[adjustedIndex];
        currentLevel.gateObject.gameObject.SetActive(false);
        foreach (Hex hex in currentLevel.hexes)
        {
            hex.hexType = HexType.Default;
        }
    }

    public Hex GetCurrentLevelGateHex(int levelIndex)
    {
        int adjustedIndex = levelIndex - 1; // Adjusting for zero-based indexing
        if (adjustedIndex < 0 || adjustedIndex >= levels.Length)
        {
            Debug.LogError("Level index out of range: " + levelIndex);
            return null;
        }

        return levels[adjustedIndex].gateHex;
    }
}

[System.Serializable]
public class LevelData
{
    public Hex gateHex;
    public Gates gateObject;
    public Hex[] hexes;
}
