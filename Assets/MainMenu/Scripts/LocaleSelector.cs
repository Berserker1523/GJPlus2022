using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocaleSelector : MonoBehaviour
{
    [SerializeField] Image circle;
    [SerializeField] LocaleID localeID;
    RectTransform rectTransform;
    Button flagButton;
    private void Awake()
    {
        circle.enabled = false;
        rectTransform = GetComponent<RectTransform>();
        flagButton = GetComponent<Button>();
        flagButton.onClick.AddListener(ChangeLocale);
    }

    private void OnEnable()
    {
        if (GetDefaultLocale() == (int)localeID)
            SetCircleSelector();
    }

    public void ChangeLocale() => StartCoroutine(SetLocale((int)localeID));

    IEnumerator SetLocale(int ID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[ID];
        PlayerPrefs.SetInt("LocaleKey", ID);
        SetCircleSelector();
    }

    private void SetCircleSelector()
    {
        circle.rectTransform.parent = rectTransform;
        circle.rectTransform.localPosition = Vector2.zero;
        circle.enabled = true;
    }

    public int GetDefaultLocale() => PlayerPrefs.GetInt("LocaleKey", 0);
}
