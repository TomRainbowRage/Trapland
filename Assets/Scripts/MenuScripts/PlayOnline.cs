using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor;
using UnityEngine.AI;
using Aspose.Zip;
using UnityEngine.Tilemaps;

public class PlayOnline : ScreenState
{
    public GameObject enterIp;
    public bool PasswordEntering = false;
    public bool IpEntering = false;
    private GameManager.OnlineServer onlineServerEntry = new GameManager.OnlineServer();
    
    public override void OnSelectionChange(GameObject selected, GameObject lastSelected)
    {
        if(!IsButtonInScreen(selected.GetComponent<Selectable>(), FindObjectOfType<PlayOnline>().gameObject))
        {
            return;
        }

        //Debug.Log("Selected = " + selected);

        if(selected.name == "Host" || selected.name == "Quickplay" || selected.name == "Manual Join")
        {
            selected.GetComponent<RectTransform>().sizeDelta = new Vector2(270.98f, 185.35f);
        }
        else if(selected.name == "Back")
        {
            selected.transform.localScale = new Vector3(1.63f, 1.63f, 1);
        }
        else if(selected.name.Contains("Entry"))
        {
            selected.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
        }
        

        if(lastSelected != null)
        {
            if(!IsButtonInScreen(lastSelected.GetComponent<Selectable>(), FindObjectOfType<PlayOnline>().gameObject))
            {
                return;
            }

            if(selected.name == "Host" || selected.name == "Quickplay" || selected.name == "Manual Join")
            {
                selected.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 171f);
            }
            else if(selected.name == "Back")
            {
                selected.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            }
            else if(selected.name.Contains("Entry"))
            {
                selected.GetComponent<Image>().color = new Color(0, 0, 0, 0.3176471f);
            }
        }
    }
    
    public override void OnClickButton(string buttonname)
    {
        if(buttonname.Contains("Entry"))
        {
            GameObject Entry = GameObject.Find(buttonname);
            if(Entry == null) {Debug.Log("No button found " + buttonname); return;}

            GameManager.OnlineServer onlineServerData = (GameManager.OnlineServer)Entry.GetComponent<ButtonScreens>().buttonData[0];

            if(onlineServerData.hasPassword)
            {
                onlineServerEntry = onlineServerData;
                //ShowInputEnter("Enter Password", true);
                GameManager.Instance.popupManager.InputBox("Enter Password");
            }
            else
            {
                GameManager.Instance.LaunchGamemodeConnect(GameManager.Instance.LocalPlayerColor, GameManager.Instance.ModeReady, onlineServerData, "");
            }
        }
        else if(buttonname == "Quickplay")
        {
            GameManager.Instance.QuickplayConnect();
        }
        else if(buttonname == "Manual Join")
        {
            //ShowInputEnter("Enter IP Adress", false);
            GameManager.Instance.popupManager.InputBox("Enter IP Adress");
        }
    }

    public override void InputBox(string title, string text)
    {
        Debug.Log("OVERRIDE INPUTBOX " + title + " " + text);

        if(title == "Enter IP Adress")
        {
            GameManager.Instance.LaunchGamemodeConnect(GameManager.Instance.LocalPlayerColor, text);
        }
        else if(title == "Enter Password")
        {
            GameManager.Instance.LaunchGamemodeConnect(GameManager.Instance.LocalPlayerColor, GameManager.Instance.ModeReady, onlineServerEntry, text);
        }

        //StartButton.Select();
        //EventSystem.current.SetSelectedGameObject(StartButton.gameObject);
    }
    
    void Start()
    {
        Debug.Log("DEFAULT BUTTON  " + StartButton);
        //EventSystem.current.firstSelectedGameObject = StartButton.gameObject;
        GameManager.Instance.ModeReady = "";
        //GameManager.Instance.ModeReady = buttonname.ToLower();

        StartButton.Select();
        EventSystem.current.SetSelectedGameObject(StartButton.gameObject);
        
        foreach (Button b in this.gameObject.GetComponentsInChildren<Button>())
        {
            GameObject button = b.gameObject;

            if(!IsButtonInScreen(b, FindObjectOfType<PlayOnline>().gameObject))
            {
                return;
            }

            if(button.name == "Host" || button.name == "Quickplay" || button.name == "Manual Join")
            {
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 171f);
            }
            else if(button.name == "Back")
            {
                button.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            }
            else if(button.name.Contains("Entry"))
            {
                button.GetComponent<Image>().color = new Color(0, 0, 0, 0.3176471f);
            }
        }

        EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(270.98f, 185.35f);


        if(GameManager.Instance.OnlineServerList.Count != 0)
        {
            GameObject EntryContainer = GameObject.Find("Entrys");
            GameObject Entry = EntryContainer.transform.GetChild(0).gameObject;

            if(GameManager.Instance.OnlineServerList.Count == 1)
            {
                FillEntryData(GameManager.Instance.OnlineServerList[0], Entry);
            }
            else if(GameManager.Instance.OnlineServerList.Count != 1)
            {
                FillEntryData(GameManager.Instance.OnlineServerList[0], Entry);
                

                for (int i = 0; i < GameManager.Instance.OnlineServerList.Count - 1; i++)
                {
                    GameObject newEntry = Instantiate<GameObject>(Entry);
                    newEntry.name = "Entry " + i;
                    newEntry.transform.SetParent(EntryContainer.transform);
                    newEntry.transform.localScale = new Vector3(1, 1, 1);
                    newEntry.transform.localPosition = EntryContainer.transform.GetChild(i).transform.localPosition;
                    newEntry.transform.localPosition -= new Vector3(0, 43.88f, 0);

                    FillEntryData(GameManager.Instance.OnlineServerList[i + 1], newEntry);

                    newEntry.GetComponent<Button>().navigation = new Navigation() {mode = Navigation.Mode.Explicit,
                    selectOnUp = EntryContainer.transform.GetChild(i).GetComponent<Button>(),
                    selectOnDown = newEntry.GetComponent<Button>().navigation.selectOnDown
                    };
                    //Debug.Log("Set " + newEntry.name + " NAV ON UP to " + EntryContainer.transform.GetChild(i).name);

                    EntryContainer.transform.GetChild(i).GetComponent<Button>().navigation = new Navigation() {mode = Navigation.Mode.Explicit,
                    selectOnDown = newEntry.GetComponent<Button>(),
                    selectOnUp = EntryContainer.transform.GetChild(i).GetComponent<Button>().navigation.selectOnUp
                    };
                    //Debug.Log("Set " + EntryContainer.transform.GetChild(i).name + " NAV ON DOWN to " + newEntry.name);
                }

                Entry.GetComponent<Button>().navigation = new Navigation() {mode = Navigation.Mode.Explicit, selectOnDown = EntryContainer.transform.GetChild(1).GetComponent<Button>()};

                EntryContainer.transform.GetChild(EntryContainer.transform.childCount - 1).GetComponent<Button>().navigation = new Navigation() {mode = Navigation.Mode.Explicit,
                selectOnDown = null,
                selectOnUp = EntryContainer.transform.GetChild(EntryContainer.transform.childCount - 1).GetComponent<Button>().navigation.selectOnUp
                };
            }
        }
        else
        {
            //Debug.Log("Destroying");
            GameObject EntryContainer = GameObject.Find("Entrys");
            GameObject Entry = EntryContainer.transform.GetChild(0).gameObject;

            Destroy(Entry);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject EntryContainer = GameObject.Find("Entrys");
            GameObject Entry = EntryContainer.transform.GetChild(0).gameObject;

            Entry.GetComponent<Button>().Select();
            EventSystem.current.SetSelectedGameObject(Entry);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartButton.Select();
            EventSystem.current.SetSelectedGameObject(StartButton.gameObject);
        }

        foreach (Button b in this.gameObject.GetComponentsInChildren<Button>())
        {
            
            GameObject button = b.gameObject;

            if(!IsButtonInScreen(b, FindObjectOfType<PlayOnline>().gameObject))
            {
                return;
            }

            //Debug.Log("Button = " + b.gameObject.name + " && Current = " + EventSystem.current.currentSelectedGameObject.name);

            if(button.name == "Host" || button.name == "Quickplay" || button.name == "Manual Join")
            {
                if(button.GetComponent<RectTransform>().sizeDelta == new Vector2(270.98f, 185.35f) && EventSystem.current.currentSelectedGameObject != button)
                {
                    button.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 171f);
                }
            }
            else if(button.name == "Back")
            {
                button.transform.localScale = new Vector3(1.5f, 1.5f, 1);
                if(button.transform.localScale == new Vector3(1.63f, 1.63f, 1))
                {
                    button.transform.localScale = new Vector3(1.5f, 1.5f, 1);
                }
            }
            else if(button.name.Contains("Entry"))
            {
                if(button.GetComponent<Image>().color == new Color(0, 0, 0, 1))
                {
                    button.GetComponent<Image>().color = new Color(0, 0, 0, 0.3176471f);
                }
                
            }
            
            
            
        }
        
        if(EventSystem.current.currentSelectedGameObject != null)
        {
            if(EventSystem.current.currentSelectedGameObject.name == "Host" || EventSystem.current.currentSelectedGameObject.name == "Quickplay" || EventSystem.current.currentSelectedGameObject.name == "Manual Join")
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(270.98f, 185.35f);
            }
            else if(EventSystem.current.currentSelectedGameObject.name == "Back")
            {
                EventSystem.current.currentSelectedGameObject.transform.localScale = new Vector3(1.63f, 1.63f, 1);
            }
            else if(EventSystem.current.currentSelectedGameObject.name.Contains("Entry"))
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            }
            
        }

        
    }

    void FixedUpdate()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Return) && GameManager.Instance.popupManager.textEnterBool == true && GameManager.Instance.popupManager.textEnter.GetComponentInChildren<TMP_InputField>().text != "")
        {
            if(GameManager.Instance.popupManager.GetTitle() == "Enter Password")
            {
                Debug.Log("Entered pass " + GameManager.Instance.popupManager.textEnterText);
                
            }
            else if(GameManager.Instance.popupManager.GetTitle() == "Enter IP Adress")
            {
                Debug.Log("Entered IP " + GameManager.Instance.popupManager.textEnterText);
            }
            else if(GameManager.Instance.popupManager.GetTitle() == "null")
            {
                Debug.LogError("Error (null)");
            }

        }
        */
    }

    void FillEntryData(GameManager.OnlineServer entrydata, GameObject entry)
    {
        if(entry.GetComponent<ButtonScreens>().buttonData.Count == 0)
        {
            entry.GetComponent<ButtonScreens>().buttonData.Add(entrydata);
        }

        entry.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = entrydata.hostName;
        entry.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = entrydata.mode;
        entry.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = entrydata.level;
        entry.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = (entrydata.players + "/" + entrydata.maxPlayers);
        entry.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = (entrydata.ping + "ms");
    }
}