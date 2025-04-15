using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public Button resumeButton;
    public GameObject MenuGame;
    public GameObject LevelScreen;
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()
    {
        LevelScreen.SetActive(true);
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("HasStartedGame", 0);
        PlayerPrefs.Save();

        Application.Quit();
        Debug.Log("Thoát game");
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

}
