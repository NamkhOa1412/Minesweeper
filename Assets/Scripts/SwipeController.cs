using UnityEngine;
using UnityEngine.SceneManagement;
public class SwipeController : MonoBehaviour
{
    [SerializeField] int maxPage;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageSteps;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    public Game game;

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
    }
    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageSteps;
            MovePage();
        }
    }
    public void Previous()
    {
        if(currentPage > 1)
        {
            currentPage--;
            targetPos -= pageSteps;
            MovePage();
        }
    }
    public void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }

    public void SelectLevel()
    {
        if (currentPage == 1)
        {
            PlayerPrefs.SetInt("Width", 16);
            PlayerPrefs.SetInt("Height", 16);
        }
        else if (currentPage == 2)
        {
            PlayerPrefs.SetInt("Width", 32);
            PlayerPrefs.SetInt("Height", 32);
        }
        else if (currentPage == 3)
        {
            PlayerPrefs.SetInt("Width", 64);
            PlayerPrefs.SetInt("Height", 64);
        }

        SceneManager.LoadScene("GameScene");
    }
}
