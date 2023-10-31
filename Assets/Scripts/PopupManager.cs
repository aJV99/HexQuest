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
    public Button quitButton;

    public delegate void PopupResponse(bool response);
    private PopupResponse callback;

    private void Awake()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        ClosePopup();
        uiBar.SetActive(true);
        notifPanel.SetActive(true);

    }

    public void ShowPopup(string message, PopupResponse responseCallback)
    {
        popupText.text = message;
        callback = responseCallback;
        popupPanel.SetActive(true);
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
    private void OnQuitClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}
