using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject popupPanel;

    [SerializeField]
    public CameraFollow maincamera;

    [SerializeField]
    public GameObject lossPanel;

    [SerializeField]
    private GameObject townPanel;

    [SerializeField]
    private GameObject storyPanel;

    [SerializeField]
    private Button close;

    [SerializeField]
    private GameObject Enemy;

    [SerializeField]
    private GameObject Town;

    [SerializeField]
    private TextMeshProUGUI purchaseText;

    [SerializeField]
    private GameObject uiBar;

    [SerializeField]
    private GameObject notifPanel;

    [SerializeField]
    private TextMeshProUGUI notifText;

    [SerializeField]
    public TextMeshProUGUI popupText;

    [SerializeField]
    public TextMeshProUGUI storyText;

    [SerializeField]
    public TextMeshProUGUI areYouSureText;

    [SerializeField]
    public TextMeshProUGUI loseText;

    [SerializeField]
    public TextMeshProUGUI winText;

    [SerializeField]
    public TextMeshProUGUI TownName;

    [SerializeField]
    public Button yesButton;

    [SerializeField]
    public Button noButton;

    [SerializeField]
    public Button okayButton;

    [SerializeField]
    public Button quitButton;

    [SerializeField]
    public Button exitButton;

    [SerializeField]
    public Button buyTroopsButton;

    [SerializeField]
    public Button restButton;

    [SerializeField]
    public Button questButton;

    [SerializeField]
    private unit selectedUnit;

    [SerializeField]
    public GameObject Escpanel;

    [SerializeField]
    public PowerUI UIManager;

    [SerializeField]
    public Button SoundButton;

    [SerializeField]
    public Button nextButton;

    [SerializeField]
    public Button IncreaseTextSize;

    [SerializeField]
    public Button DecreaseTextSize;

    [SerializeField]
    public Button Close;

    [SerializeField]
    public Button MainMenu;

    [SerializeField]
    public MiniGame BigBoss;

    [SerializeField]
    public TextMeshProUGUI miniText;

    [SerializeField]
    public GameObject MiniOutcome;

    public string[] tutorial;
    public int count = 0;
    public delegate void PopupResponse(bool response);
    private PopupResponse callback;



    public int test = 0;



    // Asign functions to buttons 
    private void Awake()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
        close.onClick.AddListener(CloseStoryPanel);
        nextButton.onClick.AddListener(NextTutorialPage);

        okayButton.onClick.AddListener(OnOkayClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        exitButton.onClick.AddListener(CloseTownPanel);
        buyTroopsButton.onClick.AddListener(BuyTroops);
        restButton.onClick.AddListener(Rest);
        questButton.onClick.AddListener(AcceptQuest);
        IncreaseTextSize.onClick.AddListener(OnIncreaseTextClicked);
        DecreaseTextSize.onClick.AddListener(OnDecreaseTextClicked);
        Close.onClick.AddListener(CloseEsc);
        MainMenu.onClick.AddListener(OnQuitClicked);


        ClosePopup();
        uiBar.SetActive(true);
        notifPanel.SetActive(true);
        tutorial = new string[4];
        tutorial[0] = "Welcome Hero! The Kingdom is under the control of an evil ruler..." +
            " You must defeat him and restore peace!";
        tutorial[1] = "There are many enemies throughout the lands, make sure you check their power and become strong before fighting them!";
        tutorial[2] = "You can rest at towns and taverns throughout the Kingdom... for a fee of course";
        tutorial[3] = "You must collect a key before you can progress to the next area, maybe see if anyone knows where it is at the closest town?" +
            " Good luck!";
        ShowStoryPopup();
        Debug.Log("story panel");



    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) & Escpanel.activeSelf == false)
        {
            Debug.Log("Escape pressed");
            ShowEscMenu();
        }
       
    }
    // General popup for asking the user if they wish to continue
    public void ShowAreYouSurePopup(string message, PopupResponse responseCallback)
    {
        Debug.Log("ARE YOU SURE");
        popupText.text = message;
        callback = responseCallback;
        popupPanel.SetActive(true);
        areYouSureText.gameObject.SetActive(true);
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        okayButton.gameObject.SetActive(false);
    }

    public void ShowNoticePopup(string message)
    {
        Debug.Log("SHOW NOTICE");
        popupText.text = message;
        popupPanel.SetActive(true);
        areYouSureText.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        okayButton.gameObject.SetActive(true);
    }

    public void ShowNoticePopu(string message, PopupResponse responseCallback)
    {
        Debug.Log("SHOW NOTICE");
        popupText.text = message;
        callback = responseCallback;
        popupPanel.SetActive(true);
        //yesButton.gameObject.SetActive(true);
        //noButton.gameObject.SetActive(false);
        //okayButton.gameObject.SetActive(true);

    }

    public void ShowEscMenu()
    {

        Escpanel.SetActive(true);
        
    }

    // Popup for losing the game
    public void ShowLossPopup(string message)
    {
        loseText.text = message;
        lossPanel.SetActive(true);
    }

    // Popup for each town
    public void ShowTownPopup()
    {
        TownName.text = "Windhelm";
        townPanel.SetActive(true);
    }


    private void OnYesClicked()
    {
        callback?.Invoke(true);
        ClosePopup();
    }

    private void OnNoClicked()
    {
        callback?.Invoke(false);
        ClosePopup();
    }

    private void OnOkayClicked()
    {
        //callback?.Invoke(true);
        ClosePopup();
    }


    private void OnQuitClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void OnIncreaseTextClicked()
    {
        if (UIManager.textSize < 18)
        {
            Debug.Log("increased");
            UIManager.textSize += 2;
        }
    }

    private void OnDecreaseTextClicked()
    {
        if (UIManager.textSize > 10)
        {
            UIManager.textSize -= 2;

        }
    }


    // When the player rests in a town, take their gold and set their currentMoves to the maximum
    private void Rest()
    {
        if (selectedUnit.currentTurns == selectedUnit.maxTurns)
        {
            purchaseText.text = "You Can't Rest Anymore!";
            purchaseText.color = Color.red;
            purchaseText.gameObject.SetActive(true);
            return;
        }
        if (selectedUnit.gold >= 25)
        {
            selectedUnit.gold -= 25;
            selectedUnit.currentTurns = selectedUnit.maxTurns;
            purchaseText.text = "Purchase Complete!";
            purchaseText.color = Color.green;
            purchaseText.gameObject.SetActive(true);
        }
        else
        {
            purchaseText.text = "Not Enough Gold!";
            purchaseText.color = Color.red;
            purchaseText.gameObject.SetActive(true);
        }
    }

    // When the player buys troops or "power" take their gold and add to their power value
    private void BuyTroops()
    {
        if (selectedUnit.gold >= 20)
        {
            selectedUnit.gold -= 20;
            selectedUnit.currentPower += 10;
            purchaseText.text = "Purchase Complete!";
            purchaseText.color = Color.green;
            purchaseText.gameObject.SetActive(true);
        }
        else
        {
            purchaseText.text = "Not Enough Gold!";
            purchaseText.color = Color.red;
            purchaseText.gameObject.SetActive(true);
        }
    }

    private void AcceptQuest()
    {
        if (selectedUnit.questActive == false)
        {
            CloseTownPanel();
            Key[] keys = GameObject.FindObjectsOfType<Key>();
            for(var i = 0; i < keys.Length; i++)
            {
                if (keys[i].name == "key1")
                {
                    keys[i].SetActive();
                    notifText.text = "Get the Key to unlock the next gate";
                    StartCoroutine(maincamera.PanCameraToObject(keys[i].gameObject));

                }
            }

        }
        else
        {
            purchaseText.text = "You already have a quest!";
            purchaseText.color = Color.red;
            purchaseText.gameObject.SetActive(true);
        }
    }

    private void ClosePopup()
    {
        popupPanel.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        okayButton.gameObject.SetActive(false);
        
    }

    private void CloseEsc()
    {
        Escpanel.SetActive(false);

    }

    private void CloseTownPanel()
    {
        townPanel.SetActive(false);

    }

    public void ShowStoryPopup()
    {
        storyPanel.SetActive(true);
        storyText.text = tutorial[0];


    }

    public void NextTutorialPage()
    {
        count += 1;
        Debug.Log(count);

        if (count == 3){
            nextButton.gameObject.SetActive(false);
        }
        
        if (count == 1)
        {
            StartCoroutine(maincamera.PanCameraToObjectStory(Enemy, storyPanel));

        }
        if (count == 2)
        {

            StartCoroutine(maincamera.PanCameraToObjectStory(Town, storyPanel));
            


        }
        storyText.text = tutorial[count];
    }


    private void CloseStoryPanel()
    {
        storyPanel.SetActive(false);

    }

    public void MiniGame()
    {
        Debug.Log("In Big Boss");
        BigBoss.gameObject.SetActive(true);
        BigBoss.Begin_Game();

    }

    public void MiniGameWin(string message)
    {
        BigBoss.gameObject.SetActive(false);
        miniText.text = message;
        MiniOutcome.SetActive(true);
        StartCoroutine(HidePanelAfterDelay(10f));
        SceneManager.LoadScene("Menu");
    }

    public void MiniGameLose(string message)
    {
        test = 1;
        BigBoss.gameObject.SetActive(false);
        miniText.text = message;
        MiniOutcome.SetActive(true);
        StartCoroutine(HidePanelAfterDelay(5f));
    }

    public void CloseMiniGame()
    {
        test = 1;
        BigBoss.gameObject.SetActive(false);

    }

    IEnumerator HidePanelAfterDelay(float delayInSeconds)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(delayInSeconds);
        MiniOutcome.SetActive(false);
    }

}
