using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using Newtonsoft.Json;
using System.IO;

public class Options : ScreenState
{
    public int howToPlay = 2;
    public GameObject selectedLocal;

    [Space(5)]

    public GameObject MainOption;
    public GameObject howPlay;
    public Button backHowPlay;

    [HideInInspector] public int musicVolMul = 5;
    [HideInInspector] public int effectVolMul = 5;
    [HideInInspector] public string[] fullscreenInc = {"ON", "OFF"};
    [HideInInspector] public string[] effectsInc = {"Low", "Medium", "High"};
    [HideInInspector] public int effectsIndex = 0;
    [HideInInspector] public string[] goreInc = {"ON", "OFF"};

    [Space(10)]

    public TextMeshProUGUI musicVolText;
    public TextMeshProUGUI effectVolText;
    public TextMeshProUGUI fullscreenText;
    public TextMeshProUGUI effectsText;
    public TextMeshProUGUI goreText;

    [Space(5)]

    public int musicVol;
    public int effectVol;
    public bool fullscreen;
    public string effects;
    public bool gore;

    public override void OnSelectionChange(GameObject selected, GameObject lastSelected)
    {
        selectedLocal = selected;
    }
    public override void OnClickButton(string buttonname)
    {
        if(buttonname == "fullscreen")
        {
            fullscreen = !fullscreen;

            if(fullscreen)
            {
                fullscreenText.text = "Fullscreen: " + fullscreenInc[0];
            }
            else if(!fullscreen)
            {
                fullscreenText.text = "Fullscreen: " + fullscreenInc[1];
            }

            GameManager.settings.ApplySettings(musicVol, effectVol, fullscreen, effects, gore);
            SaveJsonData();
        }
        else if (buttonname == "gore")
        {
            gore = !gore;

            if(gore)
            {
                goreText.text = "Gore: " + goreInc[0];
            }
            else if(!gore)
            {
                goreText.text = "Gore: " + goreInc[1];
            }

            GameManager.settings.ApplySettings(musicVol, effectVol, fullscreen, effects, gore);
            SaveJsonData();
        }
        else if(buttonname == "how to play")
        {
            howToPlay -= 1;

            MainOption.SetActive(false);
            howPlay.SetActive(true);

            //EventSystem.current.curr = backHowPlay.gameObject;
            //EventSystem.current.SetSelectedGameObject(backHowPlay.gameObject);

        }
    }

    void Awake()
    {
        LoadJsonData();

        musicVolText.text = "Music volume: " + musicVol + "%";
        effectVolText.text = "Effect volume: " + effectVol + "%";

        if(fullscreen) {fullscreenText.text = "Fullscreen: " + fullscreenInc[0];}
        else if(!fullscreen) {fullscreenText.text = "Fullscreen: " + fullscreenInc[1];}

        effectsText.text = "Effects: " + effects;

        if(gore) {goreText.text = "Gore: " + goreInc[0];}
        else if(!gore) {goreText.text = "Gore: " + goreInc[1];}
    }

    void Start() {}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            if(howToPlay == 1)
            {
                howToPlay = 0;
            }
            else if(howToPlay == -1)
            {
                howToPlay = 2;
                MainOption.SetActive(true);
                howPlay.SetActive(false);

                StartButton.Select();
                EventSystem.current.SetSelectedGameObject(StartButton.gameObject);
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(selectedLocal.name == "musicvol")
            {
                musicVol -= musicVolMul;
                if(musicVol == -5)
                {
                    musicVol = 0;
                }

                musicVolText.text = "Music volume: " + musicVol + "%";

            }
            else if(selectedLocal.name == "effectvol")
            {
                effectVol -= effectVolMul;
                if(effectVol == -5)
                {
                    effectVol = 0;
                }

                effectVolText.text = "Effect volume: " + effectVol + "%";
            }
            else if(selectedLocal.name == "fullscreen")
            {
                fullscreen = false;
                fullscreenText.text = "Fullscreen: " + fullscreenInc[1];
            }
            else if(selectedLocal.name == "effects")
            {
                effectsIndex -= 1;
                if(effectsIndex == -1)
                {
                    effectsIndex = 0;
                }

                effects = effectsInc[effectsIndex];
                effectsText.text = "Effects: " + effects;
            }
            else if(selectedLocal.name == "gore")
            {
                gore = false;
                goreText.text = "Gore: " + goreInc[1];
            }

            GameManager.settings.ApplySettings(musicVol, effectVol, fullscreen, effects, gore);
            SaveJsonData();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(selectedLocal.name == "musicvol")
            {
                musicVol += musicVolMul;
                if(musicVol == 105)
                {
                    musicVol = 100;
                }

                musicVolText.text = "Music volume: " + musicVol + "%";
            }
            else if(selectedLocal.name == "effectvol")
            {
                effectVol += effectVolMul;
                if(effectVol == 105)
                {
                    effectVol = 100;
                }

                effectVolText.text = "Effect volume: " + effectVol + "%";
            }
            else if(selectedLocal.name == "fullscreen")
            {
                fullscreen = true;
                fullscreenText.text = "Fullscreen: " + fullscreenInc[0];
            }
            else if(selectedLocal.name == "effects")
            {
                effectsIndex += 1;
                if(effectsIndex == effectsInc.Length)
                {
                    effectsIndex = effectsInc.Length - 1;
                }

                effects = effectsInc[effectsIndex];
                effectsText.text = "Effects: " + effects;
            }
            else if(selectedLocal.name == "gore")
            {
                gore = true;
                goreText.text = "Gore: " + goreInc[0];
            }

            GameManager.settings.ApplySettings(musicVol, effectVol, fullscreen, effects, gore);
            SaveJsonData();
        }
    }

    public void LoadJsonData()
    {
        OptionJson optionSaveData = JsonConvert.DeserializeObject<OptionJson>(File.ReadAllText(Application.streamingAssetsPath + @"\OptionSettings.json"));
        musicVol = optionSaveData.musicVolume;
        effectVol = optionSaveData.effectVolume;
        fullscreen = optionSaveData.fullscreen;
        effects = optionSaveData.effects;
        gore = optionSaveData.gore;

        GameManager.settings.ApplySettings(musicVol, effectVol, fullscreen, effects, gore);
    }

    public void SaveJsonData()
    {
        OptionJson optionSaveData = JsonConvert.DeserializeObject<OptionJson>(File.ReadAllText(Application.streamingAssetsPath + @"\OptionSettings.json"));
        optionSaveData.musicVolume = musicVol;
        optionSaveData.effectVolume = effectVol;
        optionSaveData.fullscreen = fullscreen;
        optionSaveData.effects = effects;
        optionSaveData.gore = gore;

        string saveJsonText = JsonConvert.SerializeObject(optionSaveData);
        File.WriteAllText(Application.streamingAssetsPath + @"\OptionSettings.json", saveJsonText);

        GameManager.settings.ApplySettings(musicVol, effectVol, fullscreen, effects, gore);
    }

    
}

public class OptionJson
{
    public int musicVolume { get; set; }
    public int effectVolume { get; set; }
    public bool fullscreen { get; set; }
    public string effects { get; set; }
    public bool gore { get; set; }
}