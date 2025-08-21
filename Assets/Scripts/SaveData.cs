using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;


public class SaveData : MonoBehaviour
{
    private static bool IsJsonLoaded = false;
    private static bool JustSaved = false;
    private static SaveDataJson saveDataJson;

    private static void LoadJson()
    {
        if(!IsJsonLoaded || JustSaved)
        {
            saveDataJson = JsonConvert.DeserializeObject<SaveDataJson>(File.ReadAllText(Application.streamingAssetsPath + @"\SaveData.json"));

            Debug.Log("Loaded SaveData Json");
            JustSaved = false;
            IsJsonLoaded = true;
        }
    }

    private static void SaveJson()
    {
        string saveJsonText = JsonConvert.SerializeObject(saveDataJson);
        File.WriteAllText(Application.streamingAssetsPath + @"\SaveData.json", saveJsonText);

        Debug.Log("Saved SaveData Json");
        JustSaved = true;
    }
}

public class SaveDataJson
{
    public List<string> levelsUnlocked { get; set; }
}
