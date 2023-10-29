using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject popupPanel;

    [SerializeField]
    private TextMeshProUGUI popupText;

    [SerializeField]
    private Button yesButton;

    [SerializeField]
    private Button noButton;

    public delegate void PopupResponse(bool response);
    private PopupResponse callback;

    private void Awake()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    public void ShowPopup(string message, PopupResponse responseCallback)
    {
        popupText.text = message;
        callback = responseCallback;
        popupPanel.SetActive(true);
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

    private void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}
