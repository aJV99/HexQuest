using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public Button easyButton;

    [SerializeField]
    public Button mediumButton;

    [SerializeField]
    public Button hardButton;

    private Difficulty selectedDifficulty; // Field to store the selected difficulty

    private void Awake()
    {
        if (easyButton == null) Debug.LogError("EasyButton is not assigned in the Inspector");
        if (mediumButton == null) Debug.LogError("MediumButton is not assigned in the Inspector");
        if (hardButton == null) Debug.LogError("HardButton is not assigned in the Inspector");

        easyButton?.onClick.AddListener(OnEasyClicked);
        mediumButton?.onClick.AddListener(OnMediumClicked);
        hardButton?.onClick.AddListener(OnHardClicked);
    }

    // Loads the main menu immediately
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GlobalSettings.SelectedDifficulty = selectedDifficulty;

    }

    // Quit the game 
    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnEasyClicked()
    {
        selectedDifficulty = Difficulty.Easy;
        PlayGame();
    }

    private void OnMediumClicked()
    {
        selectedDifficulty = Difficulty.Medium;
        PlayGame();
    }

    private void OnHardClicked()
    {
        selectedDifficulty = Difficulty.Hard;
        PlayGame();

    }
}
