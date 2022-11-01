using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaterialDataUI : MonoBehaviour
{
    public static MaterialDataUI Instance { get; private set; }
    [SerializeField] RectTransform canvasRectTransform;
    private RectTransform backgroundRectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform rectTransform;
    private void Awake()
    {
        Instance = this;
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();

        HideData();
    }

    private void SetText(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8,8);

        backgroundRectTransform.sizeDelta = textSize + padding;
    }

    private void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }

        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }


        rectTransform.anchoredPosition = anchoredPosition + new Vector2(8, 8);
    }

    private void ShowData(string data) {
        gameObject.SetActive(true);
        SetText(data);
    }

    private void HideData() {
        gameObject.SetActive(false);

    }

    public static void ShowMineralData(string data)
    {
        Instance.ShowData(data);
    }

    public static void HideMineralData()
    {
        Instance.HideData();
    }
}
