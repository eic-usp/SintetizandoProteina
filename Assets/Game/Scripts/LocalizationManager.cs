using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
using System.Threading;

public class AutoLanguageDetection : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("LoadLocale");
    }

    private IEnumerator LoadLocale()
    {
        // Wait for the initialization of LocalizationSettings
        yield return LocalizationSettings.InitializationOperation;

        // Get the system language
        var systemLanguage = Thread.CurrentThread.CurrentCulture.Name;;
        Locale systemLocale = null;
        // Check if a matching locale is available
        foreach (var locale in LocalizationSettings.Instance.GetAvailableLocales().Locales)
        {
            var code = locale.Identifier.Code;
            if (code == systemLanguage)
            {
                systemLocale = locale;
                break;
            }

            if (systemLocale == null && CompareLanguagesFirstCode(systemLanguage, code))
            {
                systemLocale = locale;
            }
            
        }

        if(systemLocale != null) LocalizationSettings.SelectedLocale = systemLocale;

        yield return null;
    }

    public static bool CompareLanguagesFirstCode(string language, string toCompareLanguage)
    {
        return string.Equals(
            language.Split('-')[0], 
            toCompareLanguage.Split('-')[0], 
            StringComparison.OrdinalIgnoreCase);
    }
}