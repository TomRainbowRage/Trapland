using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro; 

public class HostScreen : ScreenState
{
    private string[] modeArray = {"Adventure", "Race", "Deathmatch"};
    private int modeindex = 0;

    private int maxWorldRange = 6;
    private int worldIndex = 1;

    private int maxLevelRange = 11;
    private int levelIndex = 1;

    GameObject selectedHost = null;

    public Image levelImageComponent;
    public TextMeshProUGUI passwordtext;

    public override void OnSelectionChange(GameObject selected, GameObject lastSelected)
    {
        selectedHost = selected;
    }
    public override void OnClickButton(string buttonname)
    {
        if(buttonname == "Password")
        {
            GameManager.Instance.popupManager.InputBox("Password");
        }
        else if(buttonname == "Start")
        {
            GameManager.Instance.LaunchGamemodeHost(GameManager.Instance.LocalPlayerColor, modeArray[modeindex], worldIndex, levelIndex, passwordtext.text);
        }
    }
    public override void InputBox(string title, string text)
    {
        passwordtext.text = text;

        StartButton.Select();
        EventSystem.current.SetSelectedGameObject(StartButton.gameObject);
    }

    void Start() {}
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(selectedHost.name == "Mode")
            {
                modeindex -= 1;
                if(modeindex == -1)
                {
                    modeindex = 0;
                }

                selectedHost.GetComponent<TextMeshProUGUI>().text = "Mode: " + modeArray[modeindex];
            }
            else if(selectedHost.name == "World")
            {
                worldIndex -= 1;
                if(worldIndex == 0)
                {
                    worldIndex = 1;
                }

                selectedHost.GetComponent<TextMeshProUGUI>().text = "World: " + worldIndex;
            }
            else if(selectedHost.name == "Level")
            {
                levelIndex -= 1;
                if(levelIndex == 0)
                {
                    levelIndex = 1;
                }

                selectedHost.GetComponent<TextMeshProUGUI>().text = "Level: " + levelIndex;
            }

            UpdateImage();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(selectedHost.name == "Mode")
            {
                modeindex += 1;
                if(modeindex == 3)
                {
                    modeindex = 2;
                }

                selectedHost.GetComponent<TextMeshProUGUI>().text = "Mode: " + modeArray[modeindex];
            }
            else if(selectedHost.name == "World")
            {
                worldIndex += 1;
                if(worldIndex == maxWorldRange + 1)
                {
                    worldIndex = maxWorldRange;
                }

                selectedHost.GetComponent<TextMeshProUGUI>().text = "World: " + worldIndex;
            }
            else if(selectedHost.name == "Level")
            {
                levelIndex += 1;
                if(levelIndex == maxLevelRange + 1)
                {
                    levelIndex = maxLevelRange;
                }

                selectedHost.GetComponent<TextMeshProUGUI>().text = "Level: " + levelIndex;
            }

            UpdateImage();
        }
    }

    void UpdateImage()
    {
        string predefPath = @"Content/Textures/Maps/Story";
        string worldLetter = "";

        if(worldIndex == 1) {worldLetter = "a";}
        if(worldIndex == 2) {worldLetter = "b";}
        if(worldIndex == 3) {worldLetter = "c";}
        if(worldIndex == 4) {worldLetter = "d";}
        if(worldIndex == 5) {worldLetter = "e";}
        if(worldIndex == 6) {worldLetter = "f";}

        predefPath = @"Content/Textures/Maps/Story/World_" + worldLetter + @"/";

        string levelImagePath = predefPath + worldLetter + "_" + levelIndex;
        Debug.Log("Image Path = " + levelImagePath);

        Sprite levelImage = Resources.Load<Sprite>(levelImagePath);

        levelImageComponent.sprite = levelImage;
    }
}
