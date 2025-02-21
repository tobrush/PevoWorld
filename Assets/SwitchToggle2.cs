using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SwitchToggle2 : MonoBehaviour
{


    [SerializeField] RectTransform uiHandleRectTransform;

    [SerializeField] Color backgroundActiveColor;

    Color backgroundDefaultColor;

    Image backgroundImage, handleImage;

    Toggle toggle;
    Vector2 handlePosition, OpenViewPosition;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        handlePosition = uiHandleRectTransform.anchoredPosition;

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
        {
            OnSwitch(true);
        }
    }

    void OnSwitch(bool on)
    {
        //uiHandleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition;
        uiHandleRectTransform.DOAnchorPos(on ? handlePosition * -1 : handlePosition, 0.4f).SetEase(Ease.InOutBack);

        backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, 0.6f);
        //backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;
        // handleImage.color = on ? handleActiveColor : handleDefaultColor;
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
