using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocaleSelector : MonoBehaviour
{
    [SerializeField] Image[] circles = new Image[3];
    private void Awake()
    {
        int ID = PlayerPrefs.GetInt("LocaleKey", 0);
        ChangeLocale(ID);
        SetCircleSelector(ID); ;
    }

    public void ChangeLocale(int localeID)=>  StartCoroutine(SetLocale(localeID));
    

    IEnumerator SetLocale(int localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LocaleKey", localeID);

        SetCircleSelector(localeID);
    }

    private void SetCircleSelector(int localeID)
    {
        for (int i = 0; i < circles.Length; i++)
        {
            if (i == localeID)
                circles[i].enabled = true;
            else
                circles[i].enabled = false;
        }
    }
}
