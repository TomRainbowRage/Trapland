using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MainMenuClass : ScreenState
{
    /*
    public override void OnSelectionChange(GameObject selected, GameObject lastSelected)
    {
        Debug.Log("MAIN MENU DETECTION");
    }
    */
    
    public override void OnClickButton(string buttonname)
    {
        //Debug.Log("MAIN MENU DETECTION CLICK");

        if(buttonname == "playLocal - Button") {GameManager.Instance.PlayWay = "local";}
        if(buttonname == "playOnline - Button") {GameManager.Instance.PlayWay = "online";}
        if(buttonname == "customLevels - Button") {GameManager.Instance.PlayWay = "custom";}
    }
    
    void Start() 
    {
        Debug.Log("DEFAULT BUTTON  " + StartButton);
        //EventSystem.current.firstSelectedGameObject = StartButton.gameObject;

        StartButton.Select();
        EventSystem.current.SetSelectedGameObject(StartButton.gameObject);

        GameManager.Instance.ModeReady = "";
        GameManager.Instance.PlayWay = "";

        GameManager.Instance.LocalPlayerColor = "";

    }

    void Update()
    {
        //if(IsButtonInScreen(EventSystem.current.sel))
    }
}
