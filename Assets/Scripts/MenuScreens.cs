using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public class MenuScreens : MonoBehaviour
{
    internal static MenuScreens Instance { get; private set; }

    public GameObject[] Screens;
    public IDictionary<string, GameObject> nameScreens = new Dictionary<string, GameObject>();
    //public IDictionary<string, Type> ObjNameClass = new Dictionary<string, Type>() {{"MainMenu", new MainMenuClass}};
    //public ObjNameStruct[] ObjNameClass = {new ObjNameStruct() {Screen = null, Script = new MainMenuClass}};

    [Space(5)]

    public AudioClip Select;
    public AudioClip Press;
    public Sprite previewImage;

    private GameObject lastSelection = null;

    public bool selectionIntectable = true;

    void Awake()
    {
        /*
        if(Screens[0].GetComponent<ScreenState>() == null)
        {
            foreach(GameObject gmobj in Screens)
            {
                if(gmobj.name == "MainMenu") {gmobj.AddComponent<MainMenuClass>();}
                if(gmobj.name == "PlayLocal") {gmobj.AddComponent<PlayLocal>();}
                if(gmobj.name == "PlayerSelect") {gmobj.AddComponent<PlayerSelect>();}
                if(gmobj.name == "PlayOnline") {gmobj.AddComponent<PlayOnline>();}
                if(gmobj.name == "HostScreen") {gmobj.AddComponent<HostScreen>();}
                if(gmobj.name == "Custom Levels") {gmobj.AddComponent<CustomLevels>();}
                if(gmobj.name == "Options") {gmobj.AddComponent<Options>();}
                if(gmobj.name == "Credits") {gmobj.AddComponent<Credits>();}
            }
        }
        
        */
        MenuScreens.Instance = this;

        foreach(GameObject screen in Screens)
        {
            nameScreens.Add(screen.name, screen);
        }
        
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastSelection == null)
        {
            lastSelection = EventSystem.current.currentSelectedGameObject;
            /*
            if(lastSelection == null && Main.activeSelf) {lastSelection = StartMainButton.gameObject;};
            if(lastSelection == null && PlayLocal.activeSelf) {lastSelection = StartLocalButton.gameObject;};
            if(lastSelection == null && PlayOnline.activeSelf) {lastSelection = StartOnlineButton.gameObject;};
            if(lastSelection == null && CustomLevels.activeSelf) {lastSelection = StartCustomButton.gameObject;};
            if(lastSelection == null && Options.activeSelf) {lastSelection = StartOptionsButton.gameObject;};
            */
            if(lastSelection == null)
            {
                lastSelection = GetStartButtonForActiveScene().gameObject;
            }
            
            TempOnSel(lastSelection, null);

            return;
        }
        if(EventSystem.current.currentSelectedGameObject != lastSelection)
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                lastSelection.GetComponent<Selectable>().Select();
                EventSystem.current.SetSelectedGameObject(lastSelection.GetComponent<Selectable>().gameObject);
                lastSelection = EventSystem.current.currentSelectedGameObject;
            }
            else
            {
                TempOnSel(EventSystem.current.currentSelectedGameObject, lastSelection);
                lastSelection = EventSystem.current.currentSelectedGameObject;
            }
            
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("Enter");
            if(EventSystem.current.currentSelectedGameObject != null)
            {
                TempOnClick(EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>());
            }
        }
    }

    void TempOnSel(GameObject selected, GameObject lastSelected)
    {
        //Debug.Log("Changed Selection to " + selected.name);
        GetActiveScreenState().OnSelectionChange(selected, lastSelected);
        SoundManager.PlaySound(Select, "Select");
    }

    void TempOnClick(Selectable button)
    {
        //SoundManager.PlaySound(Press, "Press");
        //SoundManager.PlaySound(Press, "Press");
        //if(PlayLocal.activeSelf)
        //{

        //}
        //GetActiveScreenState().OnClickButton(button);

        //if(button !=)
        if(button != null && selectionIntectable)
        {
            if(button.gameObject.GetComponent<ButtonScreens>() != null)
            {
                button.gameObject.GetComponent<ButtonScreens>().Pressed();
            }
        }

        
    }

    public void TempOnClickName(string name)
    {
        GetActiveScreenState().OnClickButton(name);
    }

    public Button GetStartButtonForActiveScene()
    {
        Button returnval = null;

        foreach(GameObject gmobj in Screens)
        {
            if(gmobj.activeSelf == true) 
            {
                ScreenState screen = gmobj.GetComponent<ScreenState>();
                //Debug.Log(screen);
                returnval = screen.StartButton;
                //Debug.Log(screen.StartButton);
            }
            
        }

        return returnval;
    }

    public ScreenState GetActiveScreenState()
    {
        ScreenState screenreturn = null;

        foreach(GameObject gmobj in Screens)
        {
            if(gmobj.activeSelf == true) 
            {
                ScreenState screen = gmobj.GetComponent<ScreenState>();
                screenreturn = screen;
                
            }
            
        }

        return screenreturn;
    }

    public GameObject GetScreenFromName(string name)
    {
        GameObject returnval = null;
        foreach(GameObject gmobj in Screens)
        {
            if(gmobj.name == name)
            {
                returnval = gmobj;
            }
        }

        return returnval;
    }


    public void ChangeScene(GameObject scene)
    {
        StartCoroutine(ChangeSceneAsync(scene, 0));
        //GetActiveScreenState().gameObject.SetActive(false);

        //scene.SetActive(true);

        //SoundManager.PlaySound(Press, "Press");
    }

    public void ChangeScene(GameObject scene, float wait)
    {
        StartCoroutine(ChangeSceneAsync(scene, wait));
        //GetActiveScreenState().gameObject.SetActive(false);

        //scene.SetActive(true);

        //SoundManager.PlaySound(Press, "Press");
    }
    
    private IEnumerator ChangeSceneAsync(GameObject scene, float waitVal)
    {
        GetActiveScreenState().gameObject.SetActive(false);

        yield return new WaitForSeconds(waitVal);

        yield return new WaitForEndOfFrame();

        scene.SetActive(true);

        SoundManager.PlaySound(Press, "Press");

        yield return new WaitForEndOfFrame();

        lastSelection = null;
        EventSystem.current.SetSelectedGameObject(null);

    }

    public void SetSelectionNull()
    {
        lastSelection = null;
        EventSystem.current.SetSelectedGameObject(null);

        GetActiveScreenState().OnSelectionChange(GetActiveScreenState().StartButton.gameObject, null);
    }
    
}

public class ScreenState : MonoBehaviour
{
    public Button StartButton;
    public virtual void OnSelectionChange(GameObject selected, GameObject lastSelected)
    {
        Debug.Log("OnSelect Screen " + selected);
    }

    public virtual void OnClickButton(string buttonname)
    {
        Debug.Log("On Click Button Screen " + buttonname);
    }

    public bool IsButtonInScreen(Selectable b, GameObject Screen)
    {
        if(b.gameObject.transform.IsChildOf(Screen.transform))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void InputBox(string title, string text)
    {
        Debug.Log("InputBox " + title + " " + text);
    }

    public virtual void InputBool(bool boolOut)
    {
        Debug.Log("InputBool " + boolOut);
    }
}