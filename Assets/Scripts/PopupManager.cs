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
    private GameObject uiBar;

    [SerializeField]
    private GameObject notifPanel;

    [SerializeField]
    public TextMeshProUGUI popupText;

    [SerializeField]
    public TextMeshProUGUI loseText;

    [SerializeField]
    public Button yesButton;

    [SerializeField]
    public Button noButton;

    [SerializeField]
    public Button okayButton;

    [SerializeField]
    public Button quitButton;

    public delegate void PopupResponse(bool response);
    private PopupResponse callback;

    private void Awake()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
        okayButton.onClick.AddListener(OnOkayClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        ClosePopup();
        uiBar.SetActive(true);
        notifPanel.SetActive(true);


    }

    public void ShowAreYouSurePopup(string message, PopupResponse responseCallback)
    {
        Debug.Log("ARE YOU SURE");
        popupText.text = message;
        callback = responseCallback;
        popupPanel.SetActive(true);
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        okayButton.gameObject.SetActive(false);
    }

    public void ShowNoticePopup(string message)
    {
        Debug.Log("SHOW NOTICE");
        popupText.text = message;
        popupPanel.SetActive(true);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        okayButton.gameObject.SetActive(true);
    }

    public void ShowLossPopup(string message)
    {
        loseText.text = message;
        lossPanel.SetActive(true);
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
        ClosePopup();
    }

    private void OnQuitClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void ClosePopup()
    {
        popupPanel.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        okayButton.gameObject.SetActive(false);
    }
}
