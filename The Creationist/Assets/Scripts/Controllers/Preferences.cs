using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

public static class Preferences {

    private static PreferenceData data;

    public static void Update(PreferenceKey key, string value)
    {
        string keyType = CustomHelper.GetDescription(key);

        Preference targetPreference = CustomHelper.GetPreference(data, key);

        if(targetPreference == null)
        {
            Debug.LogError("Target Preference Not Found " + key.ToString());
            return;
        }

        switch(keyType)
        {
            case "String":

                break;

            case "Int":

                break;

            case "Float":

                break;

            case "Bool":

                bool b = false;

                if(bool.TryParse(value, out b))
                {
                    Debug.Log("Parsed Correctly");
                    Save();
                }
                else
                {
                    Debug.LogError("Incorrectly Value Type Passed.");
                }

                break;
        }
    }

    private static void Save()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText("C:\\Users\\alasdair\\Desktop\\JsonData.txt", json);
    }

    private static Preference Load(PreferenceKey key)
    {
        if(File.Exists("C:\\Users\\alasdair\\Desktop\\JsonData.txt"))
        {
            Debug.LogError("Preferences Do Not Exist Yet");
            return null;
        }

        string json = File.ReadAllText("C:\\Users\\alasdair\\Desktop\\JsonData.txt");
        PreferenceData data = (PreferenceData)JsonUtility.FromJson(json, typeof(PreferenceData));

        Preference preference = data.GetPreference(key);
        return preference;
    }

    public class PreferenceData
    {
        public List<Preference> preferences = new List<Preference>();
    }

    public class Preference
    {
        public PreferenceKey key;
        public string value;
    }
}

public enum PreferenceKey
{
    [Description("Bool")]Draw_Biomes
}
