using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class WorldmapMenu : ScreenState
{
    public Button endButton;

    [Space(5)]

    public KeyDirection firstExitDir;
    public KeyDirection endExitDir;

    [Space]

    public GameObject SelectorObject;

    [Space]

    public TextMeshProUGUI mapInfoText;
    public Image levelImageComponent;

    private GameObject currentSelection;
    private bool justChanged;

    private bool lockdownButtons;

    public override void OnSelectionChange(GameObject selected, GameObject lastSelected)
    {
        currentSelection = selected;

        lockdownButtons = true;
        LockdownButtons(true);

        Lerper.Instance.Lerp(0.1f, SelectorObject.transform.localPosition, selected.transform.localPosition, Lerper.ClassType.Vector3);
        


        SelectorObject.transform.localPosition = Lerper.Instance.LerpingValueVEC;

        justChanged = true;

        string worldNum = selected.name[15].ToString();
        if(selected.name.Length == 17)
        {
            worldNum += selected.name[16].ToString();
            //Debug.Log("Adding worldNumbers " + worldNum);
        }


        mapInfoText.text = "W1-1 First steps";
        mapInfoText.text = "W" + LetterToWorld(selected.name[13].ToString()) + "-" + worldNum + " ";

        UpdateImage();
    }

    public override void OnClickButton(string buttonname) {}
    void Start() {}

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) && justChanged == false)
        {
            if(currentSelection == StartButton.gameObject)
            {
                if((firstExitDir == KeyDirection.Up && Input.GetKeyDown(KeyCode.UpArrow)) || (firstExitDir == KeyDirection.Right && Input.GetKeyDown(KeyCode.RightArrow)) ||
                    (firstExitDir == KeyDirection.Down && Input.GetKeyDown(KeyCode.DownArrow)) || (firstExitDir == KeyDirection.Left && Input.GetKeyDown(KeyCode.LeftArrow)))
                {
                    NextScene(-1);
                }
            }
            else if(currentSelection == endButton.gameObject)
            {
                if((endExitDir == KeyDirection.Up && Input.GetKeyDown(KeyCode.UpArrow)) || (endExitDir == KeyDirection.Right && Input.GetKeyDown(KeyCode.RightArrow)) ||
                    (endExitDir == KeyDirection.Down && Input.GetKeyDown(KeyCode.DownArrow)) || (endExitDir == KeyDirection.Left && Input.GetKeyDown(KeyCode.LeftArrow)))
                {
                    NextScene(1);
                }
            }
        }

        justChanged = false;

        if(Lerper.Lerping)
        {
            SelectorObject.transform.localPosition = Lerper.Instance.LerpingValueVEC;
        }
        if(!Lerper.Lerping && lockdownButtons)
        {
            lockdownButtons = false;
            LockdownButtons(false);
        }

        if(!Lerper.Lerping && (SelectorObject.transform.localPosition != currentSelection.transform.localPosition))
        {
            Lerper.Instance.Lerp(0.1f, SelectorObject.transform.localPosition, currentSelection.transform.localPosition, Lerper.ClassType.Vector3);
        }
    }

    void NextScene(int dir)
    {
        int currentIndex = Array.IndexOf(MenuScreens.Instance.Screens, this.gameObject);
        Debug.Log("currentIndex = " + currentIndex);

        int newIndex = currentIndex + dir;
        Debug.Log("newIndex = " + newIndex);

        GameObject ActivateScreen = MenuScreens.Instance.Screens[newIndex];
        Debug.Log("Need to Activate Screen " + ActivateScreen.name);

        MenuScreens.Instance.ChangeScene(ActivateScreen, 0);
    }

    void UpdateImage()
    {
        string predefPath = @"Content/Textures/Maps/Story";
        string worldLetter = "";
        string worldNum = "";

        worldLetter = currentSelection.name[13].ToString();

        worldNum = currentSelection.name[15].ToString();
        if(currentSelection.name.Length == 17)
        {
            worldNum += currentSelection.name[16].ToString();
            //Debug.Log("Adding worldNumbers " + worldNum);
        }


        

        predefPath = @"Content/Textures/Maps/Story/World_" + worldLetter + @"/";

        string levelImagePath = predefPath + worldLetter + "_" + worldNum;
        Debug.Log("Image Path = " + levelImagePath);

        Sprite levelImage = Resources.Load<Sprite>(levelImagePath);

        levelImageComponent.sprite = levelImage;
    }

    void LockdownButtons(bool lockdown)
    {
        foreach (Button b in this.gameObject.GetComponentsInChildren<Button>())
        {
            b.interactable = lockdown;
        }
    }

    int LetterToWorld(string letter)
    {
        int returnInt = 0;

        if(letter == "a") {returnInt = 1;}
        else if(letter == "b") {returnInt = 1;}
        else if(letter == "c") {returnInt = 2;}
        else if(letter == "d") {returnInt = 3;}
        else if(letter == "e") {returnInt = 4;}
        else if(letter == "f") {returnInt = 5;}

        return returnInt;
    }

    public enum KeyDirection
    {
        Up,
        Right,
        Down,
        Left,
        None
    }
}
