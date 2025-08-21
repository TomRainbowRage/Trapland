using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class CustomLevels : ScreenState
{
    public Image levelImagePreview;
    public TextMeshProUGUI levelTitle;
    public TextMeshProUGUI levelAuthor;

    [Space(5)]
    public GameObject handPoint;
    public Sprite goldenStar;
    public Sprite grayStar;

    private bool refreshing = false;

    private List<GameManager.CustomLevel> customLevelsGotton = new List<GameManager.CustomLevel>();

    public override void OnSelectionChange(GameObject selected, GameObject lastSelected)
    {
        if(!selected.name.Contains("Entry"))
        {
            handPoint.transform.localPosition = new Vector3(handPoint.transform.localPosition.x, selected.transform.localPosition.y - 9, handPoint.transform.localPosition.z);
            Debug.Log("HAND POS");
        }
        else if(selected.name.Contains("Entry"))
        {
            //levelImagePreview.sprite =
            GameManager.CustomLevel customLvl = (GameManager.CustomLevel)selected.GetComponent<ButtonScreens>().buttonData[0];
            if(customLvl.levelImage == null)
            {
                levelImagePreview.sprite = MenuScreens.Instance.previewImage;
                customLvl.levelImage = MenuScreens.Instance.previewImage;
            }
            else
            {
                levelImagePreview.sprite = customLvl.levelImage;
            }

            levelTitle.text = customLvl.levelName;
            levelAuthor.text = "Made by " + customLvl.levelAuthor;
            
        }
    }
    public override void OnClickButton(string buttonname)
    {
        GameObject buttonObj = GameObject.Find(buttonname);

        if(buttonname.Contains("Entry"))
        {
            GameManager.CustomLevel customLevel = (GameManager.CustomLevel)buttonObj.GetComponent<ButtonScreens>().buttonData[0];

            GameManager.Instance.LaunchGamemodeCustom(GameManager.Instance.LocalPlayerColor, customLevel);
        }
        else if(buttonname == "refresh")
        {
            if(!refreshing)
            { Refresh(); }
            else if(refreshing)
            {
                Debug.Log("Cant Refresh now already refreshing");
            }
            
        }
        else if(buttonname == "password")
        {
            GameManager.Instance.popupManager.InputBox("Password");
        }
        else if(buttonname == "morelevels")
        {
            GameManager.Instance.GetMoreLevels();
        }
        else if(buttonname == "create")
        {
            GameManager.Instance.LaunchLevelEditor();
        }
    }
    public override void InputBox(string title, string text)
    {
        //passwordtext.text = text;

        StartButton.Select();
        EventSystem.current.SetSelectedGameObject(StartButton.gameObject);
    }
    
    void Start() 
    {
        customLevelsGotton = GameManager.Instance.GetCustomLevels();

        if(customLevelsGotton.Count != 0)
        {
            GameObject EntryContainer = GameObject.Find("Entrys");
            GameObject Entry = EntryContainer.transform.GetChild(0).gameObject;

            if(customLevelsGotton.Count == 1)
            {
                FillEntryData(customLevelsGotton[0], Entry);
            }
            else if(customLevelsGotton.Count != 1)
            {
                FillEntryData(customLevelsGotton[0], Entry);
                

                for (int i = 0; i < customLevelsGotton.Count - 1; i++)
                {
                    GameObject newEntry = Instantiate<GameObject>(Entry);
                    newEntry.name = "Entry " + i;
                    newEntry.transform.SetParent(EntryContainer.transform);
                    newEntry.transform.localScale = new Vector3(1, 1, 1);
                    newEntry.transform.localPosition = EntryContainer.transform.GetChild(i).transform.localPosition;
                    newEntry.transform.localPosition -= new Vector3(0, 57.9f, 0);

                    FillEntryData(customLevelsGotton[i + 1], newEntry);

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

            if(customLevelsGotton[0].levelImage == null)
            {
                levelImagePreview.sprite = MenuScreens.Instance.previewImage;
                customLevelsGotton[0] = new GameManager.CustomLevel() 
                {levelName = customLevelsGotton[0].levelName, levelAuthor = customLevelsGotton[0].levelAuthor,
                starRating = customLevelsGotton[0].starRating, levelData = customLevelsGotton[0].levelData, 
                levelImage = MenuScreens.Instance.previewImage};
            }
            else
            {
                levelImagePreview.sprite = customLevelsGotton[0].levelImage;
            }

            levelTitle.text = customLevelsGotton[0].levelName;
            levelAuthor.text = "Made by " + customLevelsGotton[0].levelAuthor;
        }
        else
        {
            //Debug.Log("Destroying");
            GameObject EntryContainer = GameObject.Find("Entrys");
            if(EntryContainer.transform.childCount != 0)
            {
                GameObject Entry = EntryContainer.transform.GetChild(0).gameObject;
                Destroy(Entry);
            }

        }

        refreshing = false;


    }

    void Refresh()
    {
        Transform EntryContainer = GameObject.Find("Entrys").transform;

        for (int i = 0; i < EntryContainer.childCount; i++)
        {
            GameManager.CustomLevel customLevel = (GameManager.CustomLevel)EntryContainer.GetChild(i).GetComponent<ButtonScreens>().buttonData[0];
            if(customLevel.starRating != 0)
            {
                Destroy(EntryContainer.GetChild(i).gameObject);
            }
            
        }

        for (int i = 1; i < EntryContainer.childCount; i++)
        {
            Destroy(EntryContainer.GetChild(i).gameObject);
        }

        if(EntryContainer.childCount != 0)
        {
            Transform child = EntryContainer.GetChild(0);

            child.GetComponent<Button>().navigation = new Navigation()
            {mode = Navigation.Mode.Explicit};
        }

        StartCoroutine(StartAsync());


    }
    IEnumerator StartAsync()
    {
        refreshing = true;
        yield return new WaitForSeconds(2);

        Start();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject EntryContainer = GameObject.Find("Entrys");
            if(EntryContainer.transform.childCount != 0)
            {
                GameObject Entry = EntryContainer.transform.GetChild(0).gameObject;
                handPoint.SetActive(false);

                Entry.GetComponent<Button>().Select();
                EventSystem.current.SetSelectedGameObject(Entry);
            }
            

            
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartButton.Select();
            EventSystem.current.SetSelectedGameObject(StartButton.gameObject);

            handPoint.SetActive(true);
        }
    }

    void FillEntryData(GameManager.CustomLevel entrydata, GameObject entry)
    {
        if(entry.GetComponent<ButtonScreens>().buttonData.Count == 0)
        {
            entry.GetComponent<ButtonScreens>().buttonData.Add(entrydata);
        }

        entry.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = entrydata.levelName;

        GameObject starManager = entry.transform.GetChild(1).gameObject;

        Debug.Log("entryData StarRating " + entrydata.starRating);

        if(entrydata.starRating == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject starObj = starManager.transform.GetChild(i).gameObject;
                starObj.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                starManager.transform.GetChild(i).gameObject.SetActive(true);
                Image star = starManager.transform.GetChild(i).GetComponent<Image>();
                star.sprite = grayStar;   
            }

            for (int i = 0; i < entrydata.starRating; i++)
            {
                Image star = starManager.transform.GetChild(i).GetComponent<Image>();
                star.sprite = goldenStar;   
            }
        }

        
    }
}
