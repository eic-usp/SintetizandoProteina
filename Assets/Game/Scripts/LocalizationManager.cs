using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
using System.Threading;
using UnityEngine.Localization.Tables;

public class LocalizationManager : MonoBehaviour
{
    public static readonly string DefaultTable = "DefaultTable";

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

    public static StringTable GetTable(string tableName)
    {
        var table = LocalizationSettings.StringDatabase.GetTable(tableName);

        return table;
    }
    
    public static StringTableEntry FindStringReference(string tableName, string keyName)
    {
        var table = LocalizationSettings.StringDatabase.GetTable(tableName);

        var entry = table.GetEntry(keyName);

        return entry;
    }

    public static bool CompareLanguagesFirstCode(string language, string toCompareLanguage)
    {
        return string.Equals(
            language.Split('-')[0], 
            toCompareLanguage.Split('-')[0], 
            StringComparison.OrdinalIgnoreCase);
    }
}