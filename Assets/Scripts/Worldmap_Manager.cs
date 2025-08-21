using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Worldmap_Manager : MonoBehaviour
{
    internal static Worldmap_Manager Instance { get; private set; }
    void Awake() {
        Worldmap_Manager.Instance = this;
    }

    public string loadedWorldmap = "";
    public GameObject CanvasWorldmap;
    public SpriteRenderer worldmapBackground;

    [Space]
    [Header("Map Icons")]

    public Sprite a_map;
    public Sprite b_map;
    public Sprite c_map;
    public Sprite d_map;
    public Sprite e_map;

    [Space]
    [Header("Map Backgrounds")]

    public Sprite a_map_background;
    public Sprite b_map_background;
    public Sprite c_map_background;
    public Sprite d_map_background;
    public Sprite e_map_background;

    [Space]
    [Header("Misc Sprites")]
    public Sprite link;

    [Space]
    [Header("Links")]
    List<Transform> notLinked = new List<Transform>();
    List<Transform> Linked = new List<Transform>();

    public void Start()
    {
        LoadWorldmap("a");
    }

    public void LoadWorldmap(string charWorldmap)
    {
        loadedWorldmap = "worldmap_" + charWorldmap;
        GameObject LevelsContainer = GameObject.Find("InsatancesTraps");

        FindObjectOfType<MapCreator>().LoadLevelRuntime("worldmap_" + charWorldmap);

        List<Transform> newButtonsPoses = new List<Transform>();

        /*

        for (int i = 0; i < LevelsContainer.transform.childCount; i++)
        {
            //levelPoses.Add(LevelsContainer.transform.GetChild(i));
            Transform levelPos = LevelsContainer.transform.GetChild(i);

            newButtonsPoses.Add(MakeButtonAtPos(levelPos.position, "levelButton " + levelPos.gameObject.name, a_map).transform);
            Destroy(levelPos.gameObject);
        }
        */

        for (int i = 0; i < 100; i++)
        {
            if(GameObject.Find(charWorldmap + "-" + i))
            {
                Transform levelPos = GameObject.Find(charWorldmap + "-" + i).transform;

                newButtonsPoses.Add(MakeButtonAtPos(levelPos.position, "levelButton " + levelPos.gameObject.name, a_map).transform);
                Destroy(levelPos.gameObject);
            }
        }
        

        worldmapBackground.sprite = a_map_background;
        

        //newButtonsPoses.Sort();
        /*
        newButtonsPoses.Sort((Transform t1, Transform t2) => 
        { 
            return t1.name.CompareTo(t2.name);
        });
        */

        int ins = 0;
        foreach(Transform lvl in newButtonsPoses)
        {
            //Debug.Log("lvl Name Order = " + lvl.gameObject.name);

            Transform first = lvl;
            Transform last = null;

            int firstNum = XmlLoader.GetNumbersFromString(lvl.gameObject.name);
            int lastNum = -1;

            if(ins == newButtonsPoses.Count - 1) {
            continue; }
            last = newButtonsPoses[ins + 1];
            lastNum = XmlLoader.GetNumbersFromString(newButtonsPoses[ins + 1].gameObject.name);

            if((lastNum - firstNum) != 1)
            {
                notLinked.Add(lvl);
                continue;
            }

            
            

            Linked.Add(MakeLinkBetween(first.transform, last.transform).transform);

            ins++;
        }
    }

    [System.Serializable]
    public struct Link
    {
        public Transform lvlTransformFirst;
        public Transform lvlTransformLast;
        public Vector3 firstPos;
        public Vector3 lastPos;
    }


    public GameObject MakeButtonAtPos(Vector3 pos, string objName, Sprite sprite)
    {
        GameObject buttonObj = new GameObject() {name = objName};
        buttonObj.transform.position = pos;
        buttonObj.transform.SetParent(CanvasWorldmap.transform);
        buttonObj.transform.localScale = new Vector3(1, 1, 1);

        buttonObj.AddComponent<CanvasRenderer>();

        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = sprite;
        buttonImage.color = Color.white;

        Button buttonComp = buttonObj.AddComponent<Button>();
        buttonComp.interactable = true;
        buttonComp.transition = Selectable.Transition.None;

        buttonComp.onClick.AddListener(delegate {OnButtonClicked(buttonComp); });

        buttonObj.AddComponent<ButtonScreens>();

        return buttonObj;
    }

    public GameObject MakeLinkBetween(Transform first, Transform last)
    {
        Vector3 inbetween = XmlLoader.Between(first.localPosition, last.localPosition, 0.5f);

        GameObject buttonObj = new GameObject() {name = "link (" + first.gameObject.name + ", " + last.gameObject.name + ")"};
        buttonObj.transform.SetParent(CanvasWorldmap.transform);
        buttonObj.transform.localPosition = inbetween;
            
        //buttonObj.transform.LookAt(last);
        //buttonObj.transform.eulerAngles = new Vector3(0, 0, buttonObj.transform.eulerAngles.z);

        buttonObj.AddComponent<CanvasRenderer>();

        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = link;
        buttonImage.color = Color.white;

        buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 7);
        buttonObj.transform.localScale = new Vector3(2, 2, 2);

        buttonObj.GetComponent<RectTransform>().LookAt(last);
        buttonObj.transform.eulerAngles = new Vector3(0, 0, buttonObj.transform.eulerAngles.y + 90);

        return buttonObj;
    }

    public void OnButtonClicked(Button button)
    {
        Debug.Log(button.gameObject.name);
    }
}
