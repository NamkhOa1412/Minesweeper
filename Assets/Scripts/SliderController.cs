using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderController : MonoBehaviour, IPointerDownHandler
{
    public Slider slider;
    int value = 0;

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    private void OnSliderValueChanged(float newValue)
    {
        value = Mathf.RoundToInt(newValue);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            slider.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out localMousePos
        );

        float percent = Mathf.InverseLerp(-slider.GetComponent<RectTransform>().rect.width / 2f,
                                           slider.GetComponent<RectTransform>().rect.width / 2f,
                                           localMousePos.x);

        slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, percent);
        value = Mathf.RoundToInt(slider.value);
    }

    public int ValueSlider()
    {
        return value;
    }
}
