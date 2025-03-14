using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SwitchToggle : MonoBehaviour
{
    

    [SerializeField] RectTransform uiHandleRectTransform;

    [SerializeField] Color backgroundActiveColor;
    [SerializeField] TMP_Text OpenView;

    Color backgroundDefaultColor;

    Image backgroundImage, handleImage;

    Toggle toggle;
    Vector2 handlePosition, OpenViewPosition;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        handlePosition = uiHandleRectTransform.anchoredPosition;

        OpenViewPosition = OpenView.rectTransform.anchoredPosition;

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        if(toggle.isOn)
        {
            OnSwitch(true);
        }
    }

    void OnSwitch(bool on)
    {
        //uiHandleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition;
        uiHandleRectTransform.DOAnchorPos(on ? handlePosition * -1 : handlePosition, 0.4f).SetEase(Ease.InOutBack);

        backgroundImage.DOColor( on ? backgroundActiveColor : backgroundDefaultColor, 0.6f );
        OpenView.text = on ? "공개" : "비공개" ;
        OpenView.rectTransform.DOAnchorPos(on ? OpenViewPosition * -1 : OpenViewPosition, 0.4f).SetEase(Ease.InOutBack);
        //backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;
        // handleImage.color = on ? handleActiveColor : handleDefaultColor;
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
