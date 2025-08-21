using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class PlayerSelect : ScreenState
{
    public TextMeshProUGUI Readytext;
    public PlayerSprites[] playertoSprites;
    public Image playerShow;

    public string SelectedPlayer = "";

    private int IndexPlayer = 0;

    public override void OnSelectionChange(GameObject selected, GameObject lastSelected) {}
    public override void OnClickButton(string buttonname) {}
    void Start() {}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            IndexPlayer -= 1;
            if(IndexPlayer == -1)
            {
                IndexPlayer = 3;
            }

            SelectedPlayer = playertoSprites[IndexPlayer].color;
            GameManager.Instance.LocalPlayerColor = playertoSprites[IndexPlayer].color;

            playerShow.sprite = playertoSprites[IndexPlayer].sprite;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            IndexPlayer += 1;
            if(IndexPlayer == 4)
            {
                IndexPlayer = 0;
            }

            SelectedPlayer = playertoSprites[IndexPlayer].color;
            GameManager.Instance.LocalPlayerColor = playertoSprites[IndexPlayer].color;

            playerShow.sprite = playertoSprites[IndexPlayer].sprite;
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(Readytext.text == @"Press\nEnter\nto ready" || Readytext.text == "Press\nEnter\nto ready")
            {
                Debug.Log("SETTING TEXT");
                Readytext.text = "Ready";
            }

            if(Readytext.text == "Ready")
            {
                if(GameManager.Instance.PlayWay == "local") {
                GameManager.Instance.LaunchGamemode(SelectedPlayer, GameManager.Instance.ModeReady); }

                if(GameManager.Instance.PlayWay == "online") {
                GameManager.Instance.LocalPlayerColor = SelectedPlayer;
                FindObjectOfType<MenuScreens>().ChangeScene(MenuScreens.Instance.nameScreens["PlayOnline"]); }

                if(GameManager.Instance.PlayWay == "custom") {
                GameManager.Instance.LocalPlayerColor = SelectedPlayer;
                FindObjectOfType<MenuScreens>().ChangeScene(MenuScreens.Instance.nameScreens["Custom Levels"]); }
            }

            Debug.Log("TEXT = " + Readytext.text);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Readytext.text == @"Press\nEnter\nto ready" || Readytext.text == "Press\nEnter\nto ready")
            {
                Debug.Log("CHANGING SCENE");
                if(GameManager.Instance.ModeReady == "race" || GameManager.Instance.ModeReady == "adventure" || GameManager.Instance.ModeReady == "deathmatch")
                { FindObjectOfType<MenuScreens>().ChangeScene(MenuScreens.Instance.GetScreenFromName("PlayLocal")); }

                if(GameManager.Instance.ModeReady == "online")
                { FindObjectOfType<MenuScreens>().ChangeScene(MenuScreens.Instance.GetScreenFromName("PlayOnline")); }
                
            }
            else if(Readytext.text == "Ready")
            {
                Readytext.text = "Press\nEnter\nto ready";
            }
        }
    }

    [Serializable]
    public struct PlayerSprites
    {
        public string color;
        public Sprite sprite;
    }
}
