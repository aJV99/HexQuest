using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI; 

public class HowToPlayMenuHandler : MonoBehaviour
{
    [System.Serializable]
    public class HowToPlayEntry
    {
        public string header;
        public string paragraph;
    }

    [SerializeField]
    private TextMeshProUGUI headerText;
    [SerializeField]
    private TextMeshProUGUI paragraphText;
    [SerializeField]
    private Button nextButton;  // Reference to the Next button
    [SerializeField]
    private Button previousButton;  // Reference to the Previous button

    private List<HowToPlayEntry> howToPlayEntries = new List<HowToPlayEntry>
{   // Instructions on how to play the game
    new HowToPlayEntry { header = "Hex Quest - How To Play", paragraph = "Welcome to Hex Quest! Step into a world of medieval strategy, where your decisions shape the destiny of a fallen knight seeking to reclaim his conquered castle. If you're new to this realm, fret not! Here's a step-by-step guide to master the game:\r\n\r\n1. Objective\r\nYour primary goal is to train and increase your power enough to reclaim your castle from the Barbarian that has captured it.\r\n\r\n2. Starting Out\r\nYou begin as a lone knight on a vast 3D map. Your immediate task is to explore the terrain, gather resources, and increase your strength." },
    new HowToPlayEntry { header = "Hex Quest - How To Play - Page 2", paragraph = "Quick Controls: \r\n• Look for the yellow boy. That's your character.\r\n• Control your character by clicking on them and then the hex you wish to travel to.\r\n• Start by heading towards the closest Town to accept a quest.\r\n• Keep an eye on Power, Gold, and Moves Remaining. If your power drops to zero, you lose a life and you only have 3 lives until Game Over!\r\n• Approach an enemy's tile to initiate a battle. A warning pop-up will appear.\r\n• Manage Your Moves: You have a limited number of moves before requiring rest. Refill your move count at towns or taverns. If you're out of gold or can't find a resting place in time, you'll sleep rough, depleting your power.\r\n• Press A or Y to orbit the camera around the player.\r\n• Press Q to toggle Map View." },
    new HowToPlayEntry { header = "Hex Quest - How To Play - Page 3", paragraph = "3. Movement and Turns\r\n- Movement: Navigate the game world by clicking on the desired tile.\r\n- Each turn grants you up to 5 moves.\r\n- Once your 5 moves are consumed, you must rest before resuming your journey.\r\n\r\n4. Structures & Key Locations\r\n- Taverns: These provide a full rest, recharging your moves to the maximum of 5.\r\n- Rough Sleep: Rest anywhere to regain up to 3 moves. But be cautious, as these camps can be ambushed by enemies.\r\n- Towns: Spend gold here to increase your power. The larger your power, the stronger you are in combat. You can also rest up here and accept quests too.\r\n" },
    new HowToPlayEntry { header = "Hex Quest - How To Play - Page 4", paragraph = "5. Powwer Dynamics\r\n- Your strength can grow or diminish based on your decisions and battle outcomes.\r\n- Engage in skirmishes to potentially earn gold and make your way. Beware, as you might also lose soldiers in these battles.\r\n\r\n6. Combat Mechanics\r\n- Engage with enemies on the map.\r\n- The outcome of battles is determined by the size and strength of your army compared to the enemy's.\r\n- Strategize wisely, as repeated defeats can end your quest." },
    new HowToPlayEntry { header = "Hex Quest - How To Play - Page 5", paragraph = "7. Economic System\r\n- Gold is a vital resource. Collect it by winning battles, exploring mines, and exchanging other resources.\r\n- Gold can be used to recruit troops in towns or barter for other necessities.\r\n-In specific locations, you can invest gold to enhance your combat capabilities. A high power is crucial for your endgame.\r\n\r\n8. Win & Loss Conditions\r\nVictory: Confront and defeat the guardian boss of your castle.\r\nDefeat: You can lose if any of the following occur:\r\n- You lose all your lives which can happen by experiencing a string of losses in battles." },
    new HowToPlayEntry { header = "Hex Quest - How To Play - Page 6", paragraph = "9. Tips for Success\r\n- Always keep an eye on your remaining moves. It's often wise to rest and recharge before a potential battle.\r\n- Expand your power early on. A larger force will deter smaller enemies and grant you a better chance in battles.\r\n- Diversify your journey. Visit various structures, engage in different activities, and ensure you're always strengthening your position.\r\n\r\nNow, brave knight, embark on your quest to reclaim your homeland! Good luck on the fields of HEX QUEST!" }
    // You can continue adding more entries here as needed

};

    private int currentIndex = 0;

    private void Start()
    {
        DisplayEntry(currentIndex);
        UpdateButtonStates();
    }

    // Cycle through pages of instructions
    public void OnNextButtonClicked()
    {
        if (currentIndex < howToPlayEntries.Count - 1)
        {
            currentIndex++;
            DisplayEntry(currentIndex);
            UpdateButtonStates();
        }
    }

    public void OnPreviousButtonClicked()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            DisplayEntry(currentIndex);
            UpdateButtonStates();
        }
    }

    private void DisplayEntry(int index)
    {
        if (index >= 0 && index < howToPlayEntries.Count)
        {
            headerText.text = howToPlayEntries[index].header;
            paragraphText.text = howToPlayEntries[index].paragraph;
        }
        else
        {
            Debug.LogError($"Invalid index: {index}");
        }
    }

    private void UpdateButtonStates()
    {
        nextButton.gameObject.SetActive(currentIndex < howToPlayEntries.Count - 1);
        previousButton.gameObject.SetActive(currentIndex > 0);
    }
}
