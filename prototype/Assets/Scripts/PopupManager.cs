using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject popupPanel;

    [SerializeField]
    private GameObject lossPanel;

    [SerializeField]
    private GameObject townPanel;

    [SerializeField]
    private TextMeshProUGUI purchaseText;

    [SerializeField]
    private GameObject uiBar;

    [SerializeField]
    private GameObject notifPanel;

    [SerializeField]
    public TextMeshProUGUI popupText;

    [SerializeField]
    public TextMeshProUGUI areYouSureText;

    [SerializeField]
    public TextMeshProUGUI loseText;

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
    private unit selectedUnit;

    public delegate void PopupResponse(bool response);
    private PopupResponse callback;


    // Asign functions to buttons 
    private void Awake()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
        okayButton.onClick.AddListener(OnOkayClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        exitButton.onClick.AddListener(CloseTownPanel);
        buyTroopsButton.onClick.AddListener(BuyTroops);
        restButton.onClick.AddListener(Rest);

        ClosePopup();
        uiBar.SetActive(true);
        notifPanel.SetActive(true);


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

    private void ClosePopup()
    {
        popupPanel.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        okayButton.gameObject.SetActive(false);
    }

    private void CloseTownPanel()
    {
        townPanel.SetActive(false);

    }
}
