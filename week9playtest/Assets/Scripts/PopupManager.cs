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
    public OrbitCamera maincamera;

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
    public Button IncreaseTextSize;

    [SerializeField]
    public Button DecreaseTextSize;

    [SerializeField]
    public Button Close;

    [SerializeField]
    public Button MainMenu;


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
        questButton.onClick.AddListener(AcceptQuest);
        IncreaseTextSize.onClick.AddListener(OnIncreaseTextClicked);
        DecreaseTextSize.onClick.AddListener(OnDecreaseTextClicked);
        Close.onClick.AddListener(CloseEsc);
        MainMenu.onClick.AddListener(OnQuitClicked);


        ClosePopup();
        uiBar.SetActive(true);
        notifPanel.SetActive(true);


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
        if (UIManager.textSize < 20)
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
                    StartCoroutine(PanCameraToObject(keys[i].gameObject));

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

    public IEnumerator PanCameraToObject(GameObject targetObject)
    {
        Debug.Log("Coroutine started: Panning to object.");

        // Save the original camera position and rotation
        Vector3 originalPosition = Camera.main.transform.position;
        Quaternion originalRotation = Camera.main.transform.rotation;

        // Calculate the target position and rotation relative to the target object
        Vector3 targetPositionOffset = new Vector3(-7.211359f, 11.66f, -6.627083f);
        Quaternion targetRotationOffset = Quaternion.Euler(49.971f, 47.418f, 0f);
        Vector3 targetPosition = targetObject.transform.TransformPoint(targetPositionOffset);
        Quaternion targetRotation = targetObject.transform.rotation * targetRotationOffset;

        float timeToFocus = 1.0f; // Duration of the focus animation

        // Animate camera to the target
        float t = 0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / timeToFocus;
            Camera.main.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            Camera.main.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, t);
            yield return null;
        }

        Debug.Log("Focus on object achieved. Starting focus timer.");

        // Keep the focus on the key for 2 seconds using a timer
        float focusTimeElapsed = 0f;
        while (focusTimeElapsed < 2.0f)
        {
            focusTimeElapsed += Time.deltaTime;
            // Keep setting the camera's position and rotation to ensure it stays put.
            Camera.main.transform.position = targetPosition;
            Camera.main.transform.rotation = targetRotation;
            yield return null;
        }

        Debug.Log("Focus duration ended. Returning to player.");

        // Return the camera to the player
        t = 0f; // Reset t to 0 to start the lerp for the return journey
        while (t < 1.0f)
        {
            t += Time.deltaTime / timeToFocus;
            Camera.main.transform.position = Vector3.Lerp(targetPosition, originalPosition, t);
            Camera.main.transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, t);
            yield return null;
        }

        // Ensure camera is exactly in the original position and rotation
        Camera.main.transform.position = originalPosition;
        Camera.main.transform.rotation = originalRotation;

        Debug.Log("Coroutine finished: Camera returned to player.");
    }
}
