using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private GameObject settingsPopup;
    private GameObject gameOverUI;
    private GameObject gameWinUI;
    void Start()
    {
        settingsPopup = GameObject.FindWithTag("SettingPopup");
        gameOverUI = GameObject.FindWithTag("GameOverUI");
        gameWinUI = GameObject.FindWithTag("GameWinUI");
        if (settingsPopup != null && gameOverUI != null)
        {
            settingsPopup.SetActive(false);
            gameOverUI.SetActive(false);
            gameWinUI.SetActive(false);
        }
    }

    public void OpenSettingPopup()
    {
        if (settingsPopup != null)
        {
            bool isActive = settingsPopup.activeSelf;
            settingsPopup.SetActive(!isActive);
        }
    }

    public void CloseSettingPopup()
    {
        if (settingsPopup != null)
        {
            bool isActive = settingsPopup.activeSelf;
            settingsPopup.SetActive(!isActive);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ShowGameOverUI() {
        gameOverUI.SetActive(true);
    }
    public void ShowGameWinUI()
    {
        gameWinUI.SetActive(true);
    }
}
