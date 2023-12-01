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
    new HowToPlayEntry { header = "Hex Quest - How To Play", paragraph = "Welcome to Hex Quest! Step into a world of medieval strategy, where your decisions shape the destiny of a fallen knight seeking to reclaim his conquered castle. If you're new to this realm, fret not! Here's a step-by-step guide to master the game:\r\n\r\n1. Objective\r\nYour primary goal is to assemble, train, and lead an army robust enough to reclaim your castle from the opposing force that has captured it.\r\n\r\n2. Starting Out\r\nYou begin as a lone knight on a vast 3D map. Your immediate task is to explore the terrain, gather resources, and recruit troops." },
    new HowToPlayEntry { header = "Hex Quest - How To Play - Page 2", paragraph = "3. Movement and Turns\r\n- Movement: Navigate the game world by clicking on the desired tile.\r\n- Each turn grants you up to 5 moves.\r\n- Once your 5 moves are consumed, you must rest before resuming your journey.\r\n\r\n4. Structures & Key Locations\r\n- Taverns: These provide a full rest, recharging your moves to the maximum of 5.\r\n- Wild Camps: Rest here to regain up to 3 moves. But be cautious, as these camps can be ambushed by enemies.\r\n- Towns: Spend gold here to recruit troops. The larger your army, the stronger you are in combat.\r\n- Mines: Engage in mini-games to extract valuable resources which can be converted into gold." },
    new HowToPlayEntry { header = "Hex Quest - How To Play - Page 3", paragraph = "5. Army Dynamics\r\n- Your army's strength can grow or diminish based on your decisions and battle outcomes.\r\n- Engage in skirmishes to potentially earn gold and recruit additional troops. Beware, as you might also lose soldiers in these battles.\r\n\r\n6. Combat Mechanics\r\n- Engage with enemies on the map.\r\n- The outcome of battles is determined by the size and strength of your army compared to the enemy's.\r\n- Strategize wisely, as repeated defeats can end your quest." },
    new HowToPlayEntry { header = "Hex Quest - How To Play - Page 4", paragraph = "7. Economic System\r\n- Gold is a vital resource. Collect it by winning battles, exploring mines, and exchanging other resources.\r\n- Gold can be used to recruit troops in towns or barter for other necessities.\r\n-In specific locations, you can invest gold to enhance your troops' combat capabilities. A well-trained army is crucial for your endgame.\r\n\r\n9. Win & Loss Conditions\r\nVictory: Confront and defeat the guardian boss of your castle.\r\nDefeat: You can lose if any of the following occur:\r\n- Your entire army is wiped out.\r\n- You're defeated by the castle's guardian.\r\n- Experiencing a string of losses in skirmishes." },
    new HowToPlayEntry { header = "Hex Quest - How To Play - Page 5", paragraph = "10. Tips for Success\r\n- Always keep an eye on your remaining moves. It's often wise to rest and recharge before a potential battle.\r\n- Expand your army early on. A larger force will deter smaller enemies and grant you a better chance in skirmishes.\r\n- Diversify your journey. Visit various structures, engage in different activities, and ensure you're always strengthening your position.\r\n\r\nNow, brave knight, embark on your quest to reclaim your homeland! Good luck on the fields of HEX QUEST!" }
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
