using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanelToggle : MonoBehaviour
{
    public GameObject panel; // Панель, которую будем показывать или скрывать
    private CanvasGroup canvasGroup;
    private bool isPanelVisible = false;

    public void Start()
    {
        // Получаем компонент CanvasGroup
        canvasGroup = panel.GetComponent<CanvasGroup>();

        // Скрываем панель при старте
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnButtonClick()
    {
        if (isPanelVisible)
        {
            Debug.Log("Here");
            panel.transform.SetParent(transform.root);
            panel.SetActive(false);
            isPanelVisible = !isPanelVisible;

            return;
        }
        isPanelVisible = !isPanelVisible;

        StartCoroutine(TogglePanel(isPanelVisible));
    }

    private IEnumerator TogglePanel(bool show)
    {
        float duration = 0.5f; // Длительность анимации
        float elapsedTime = 0f;
        panel.SetActive(true);
        if (show)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            // Переместить панель под кнопку
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            RectTransform buttonRect = GetComponent<RectTransform>();
            panelRect.SetParent(buttonRect.parent);
            panelRect.anchoredPosition = new Vector2(buttonRect.anchoredPosition.x, buttonRect.anchoredPosition.y - buttonRect.rect.height);
            panel.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            panelRect.sizeDelta=new Vector2(100,270);
            while (elapsedTime < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        canvasGroup.alpha = show ? 1 : 0;
    }
}
