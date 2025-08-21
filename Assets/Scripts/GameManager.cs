using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal static GameManager Instance { get; private set; }
    void Awake() {
        GameManager.Instance = this;
        DontDestroyOnLoad(this.gameObject);
        popupManager = gameObject.GetComponent<MenuPopupManager>();
        //gameObject.hideFlags = (HideFlags)61;
        settings = new OptionSettings();
    }

    public string ModeReady = ""; // adventure, race, deathmatch, custom
    public string PlayWay = ""; // local, online, custom
    public string LocalPlayerColor = ""; // blue, green. purple, red
    public List<OnlineServer> OnlineServerList = new List<OnlineServer>();
    public List<CustomLevel> customLevelsEditor = new List<CustomLevel>();
    public MenuPopupManager popupManager;
    public static OptionSettings settings;
    public DisplaySettings editorSettings;
    
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LaunchGamemode(string character, string mode)
    {
        Debug.Log("Launching Gamemode " + mode + " With character " + character);
    }

    //ONLINE
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void LaunchGamemodeConnect(string character, string mode, OnlineServer onlineServer, string enteredPassword)
    {
        mode = onlineServer.mode;
        Debug.Log("Connecting... Launching Gamemode " + mode + " With character " + character + "\nOnlineserver " + onlineServer + " EnteredPass " + enteredPassword);
        //REMEBER TO SHOW CONNECTING MESSAGE BOX ASYNC

        //REMEBER MUST RELOAD SCENE IF FAIL
    }
    public void LaunchGamemodeConnect(string character, string address)
    {
        Debug.Log("Connecting... Launching Gamemode " + " With character " + character + " EnteredIP " + address);
        //REMEBER TO SHOW CONNECTING MESSAGE BOX ASYNC

        //REMEBER MUST RELOAD SCENE IF FAIL
    }

    public void QuickplayConnect()
    {
        Debug.Log("Quick Play");
    }

    public void LaunchGamemodeHost(string character, string mode, int world, int level, string password)
    {
        Debug.Log("Hosting Game " + mode + " With character " + character + "\nLevel " + world + "_" + level + " With Password " + password);
    }

    [System.Serializable]
    public struct OnlineServer
    {
        public string hostName;
        public string mode;
        public string level;
        public int players;
        public int maxPlayers;
        public int ping;

        public bool hasPassword;
        public string Password;
    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    //CUSTOM
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void LaunchGamemodeCustom(string character, CustomLevel customLevel)
    {
        Debug.Log("Launching Gamemode adventure With character " + character + " in Level " + customLevel.levelName + "\nby " + customLevel.levelAuthor + " with star rating " + customLevel.starRating);
        //customLevel.levelData
        //LAUNCH IN adventure
    }
    public void LaunchLevelEditor()
    {
        Debug.Log("Launch Level Editor");
    }
    public List<CustomLevel> GetCustomLevels()
    {
        return customLevelsEditor;
    }
    
    public void GetMoreLevels()
    {
        Debug.Log("Get More Levels (Steam, MOD.IO)");
    }

    [System.Serializable]
    public struct CustomLevel
    {
        public string levelName;
        public string levelAuthor;
        public int starRating;
        public Sprite levelImage;

        public string[] levelData;

    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    //Game Util
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    [System.Serializable]
    public struct DisplaySettings
    {
        public int musicVol;
        public int effectVol;
        public bool fullscreen;
        public OptionSettings.Effects effects;
        public bool gore;
    }
}

[System.Serializable]
public class OptionSettings
{
    public int musicVol;
    public int effectVol;
    public bool fullscreen;
    public Effects effects;
    public bool gore;

    public enum Effects
    {
        Low,
        Medium,
        High
    }

    public void ApplySettings(int musicVolARG, int effectVolARG, bool fullscreenBool, string effectsString, bool goreBool)
    {
        musicVol = musicVolARG;
        effectVol = effectVolARG;
        fullscreen = fullscreenBool;
        gore = goreBool;

        if(effectsString == "Low") {effects = Effects.Low;}
        if(effectsString == "Medium") {effects = Effects.Medium;}
        if(effectsString == "High") {effects = Effects.High;}


        GameManager.Instance.editorSettings.musicVol = musicVol;
        GameManager.Instance.editorSettings.effectVol = effectVol;
        GameManager.Instance.editorSettings.fullscreen = fullscreen;
        GameManager.Instance.editorSettings.effects = effects;
        GameManager.Instance.editorSettings.gore = gore;

    }
}
