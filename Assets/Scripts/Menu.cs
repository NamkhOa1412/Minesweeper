using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public Button resumeButton;
    public GameObject MenuGame;
    public GameObject LevelScreen;
    private GameObject settingsScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LevelScreen.SetActive(false);
        GameObject resumeObj = GameObject.FindWithTag("ResumeButton");
        if (resumeObj != null)
        {
            resumeButton = resumeObj.GetComponent<Button>();

            if (PlayerPrefs.GetInt("HasStartedGame", 0) == 0)
            {
                resumeButton.gameObject.SetActive(false);
            }
        }
        settingsScreen = GameObject.FindWithTag("SettingScreen");
        if (settingsScreen != null)
        {
            settingsScreen.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy GameObject với tag 'SettingScreen'");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()
    {
        if (settingsScreen != null && settingsScreen.activeSelf)
        {
            return;
        }
        else
        {
            LevelScreen.SetActive(true);
        }
    }

    public void HideLevelSceen()
    {
        if (settingsScreen != null && settingsScreen.activeSelf)
        {
            return;
        }
        else
        {
            LevelScreen.SetActive(false);
        }
    }

    public void QuitGame()
    {
        if (settingsScreen != null && settingsScreen.activeSelf || LevelScreen != null && LevelScreen.activeSelf)
        {
            return;
        }
        else
        {

            Debug.Log("Quit game...");
            PlayerPrefs.SetInt("HasStartedGame", 0);
            PlayerPrefs.Save();

            Application.Quit();
        }
    }

    public void OnNewGameClicked()
    {
        PlayerPrefs.SetInt("HasStartedGame", 1);
        resumeButton.gameObject.SetActive(true);
    }

    public void OnResumeClicked()
    {
        Debug.Log("Resume game...");
    }

    public void OpenSettingPopup()
    {
        if (LevelScreen != null && LevelScreen.activeSelf)
        {
            return;
        }else
        {
            if (settingsScreen != null)
            {
                bool isActive = settingsScreen.activeSelf;
                settingsScreen.SetActive(!isActive);
            }
        }
        
    }

    public void CloseSettingPopup()
    {
        if (settingsScreen != null)
        {
            bool isActive = settingsScreen.activeSelf;
            settingsScreen.SetActive(!isActive);
        }
    }
}
