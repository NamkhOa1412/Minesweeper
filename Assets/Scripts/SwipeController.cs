using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField] int maxPage;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageSteps;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;

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
}
