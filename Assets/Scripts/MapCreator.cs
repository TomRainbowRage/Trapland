using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;
using System;

public class MapCreator : MonoBehaviour
{
    [HideInInspector] public XmlLoader xmlLoad;
    [HideInInspector] public string buttonParams;
    [SerializeField] private Camera Cammain;

    [Space]
    //public Tilemap BackGround;
    //public Tilemap WorldObjects;
    //public Tilemap ForeGround;
    public TileMapLayer BackGround = new TileMapLayer();
    public TileMapLayer WorldObjects = new TileMapLayer();
    public TileMapLayer ForeGround = new TileMapLayer();
    public TileMapLayer TrapBlocks = new TileMapLayer();
    public TileMapLayer Collision = new TileMapLayer();

    [Space]
    public TileBase tilePlace;

    public Tile backuptile;
    public Tile StartTile;

    [Space]
    public bool DebugMapCreation;
    public bool DrawOnStart;

    [Space]
    public bool background;
    public bool worldobjects;
    public bool collision;

    [Space]
    public List<GameObject> DynamicTiles = new List<GameObject>();

    [Space]
    public List<WKeyXML> ConditionsTesting = new List<WKeyXML>();


    private IDictionary<Vector3Int, int> TextTile = new Dictionary<Vector3Int, int>();

    void Awake()
    {
        xmlLoad = GetComponent<XmlLoader>();

        //Camera.main.gameObject.SetActive(true);
        Cammain.gameObject.SetActive(true);

        if(!DebugMapCreation) {
        Cammain.GetComponent<PanAndZoom>().enabled = false; }

        /*
        if(DrawOnStart)
        {
            LoadLevel();
        }
        */
    }

    void Start()
    {
        
        //LoadLevel();


        /*
        float numNeedRound = 144;

        if (!((numNeedRound % 48) == 0))
        {
            Debug.LogError("Not Multiple 48 : " + numNeedRound);
            //val is not a multiple of 48, or 0
        }
        */

        if(DrawOnStart)
        {
            LoadLevelRuntime(FindObjectOfType<XmlLoader>().Map.name);
        }
    }

    public void LoadLevelRuntime(string mapName)
    {
        //BackGround.defaultTilemap.ClearAllTiles();
        //WorldObjects.defaultTilemap.ClearAllTiles();
        //ForeGround.defaultTilemap.ClearAllTiles();
        //TrapBlocks.defaultTilemap.ClearAllTiles();
        BackGround.ClearAllTiles();
        WorldObjects.ClearAllTiles();
        ForeGround.ClearAllTiles();
        TrapBlocks.ClearAllTiles();
        Collision.ClearAllTiles();

        for (int i = 0; i < GameObject.Find("InsatancesTraps").transform.childCount; i++)
        {
            Destroy(GameObject.Find("InsatancesTraps").transform.GetChild(i).gameObject);
        }

        DynamicTiles.Clear();

        TextAsset instanceNameText = new TextAsset() {name = mapName};

        FindObjectOfType<XmlLoader>().Map = instanceNameText;

        FindObjectOfType<XmlLoader>().Awake();

        LoadLevel();
    }

    public void LoadLevel()
    {
        int line = 1;
        foreach(XmlLoader.W w in xmlLoad.main.Zone.W)
        {
            //if(w.N.ToLower() == "worldobject" || w.N.ToLower() == "torch")
            //if(w.OT.ToLower() == "ground" || w.OT.ToLower() == "worldobject" || w.OT.ToLower() == "background" || w.OT.ToLower() == "trapblock" || w.OT.ToLower() == "trap" || w.OT.ToLower() == "trampoline" || w.OT.ToLower() == "quickgoal") //WORLD OBJECT
            if(true)
            {
                float Xval = (XmlLoader.StringToInt(w.PX) / 48);
                float Yval = (XmlLoader.StringToInt(w.PY) / 48);
                /*
                if ((Xval % 48 == 0))
                {
                    Debug.LogError("Not Multiple X : " + Xval);
                    //val is not a multiple of 48, or 0
                    Xval = Mathf.Round(Xval/48) * 48;
                }
                if ((Yval % 48 == 0))
                {
                    Debug.LogError("Not Multiple Y : " + Yval);
                    //val is not a multiple of 48, or 0
                    Yval = Mathf.Round(Yval/48) * 48;
                }
                */

                //Xval = Mathf.Round(Xval/48) * 48;
                //Yval = Mathf.Round(Yval/48) * 48;

                Xval = Mathf.RoundToInt(Xval);
                Yval = Mathf.RoundToInt(Yval);

                //Xval = RoundToMultiple(Xval, 48);
                //Yval = RoundToMultiple(Yval, 48);

                //Debug.LogError("Place " + Xval + " : " + Yval);
                

                //Debug.LogWarning("GROUND");
                Vector3Int v3Place = new Vector3Int((int)Xval, -(int)Yval, 0);
                //Debug.Log("Place Tile " + v3Place);
                //Tile tile = (Tile)Resources.Load(xmlLoad.GetTilePath(w.TN, new Vector2(xmlLoad.StringToInt(w.PX), xmlLoad.StringToInt(w.PY)))) as Tile;
                Tile tile = TilesGet.instance.GetTileNewProg(w.TN);

                

                TextTile[v3Place] = XmlLoader.GetNumbersFromString(w.TN);
                Debug.Log("Added " + v3Place + " " + TextTile[v3Place] + " To Dict");
                
                //tile.color = tile.color.gamma;
                if(tile == null)// || w.CT == "15")
                {
                    Debug.LogWarning("Tryed to place " + xmlLoad.GetTilePath(w.TN, new Vector2(XmlLoader.StringToInt(w.PX), XmlLoader.StringToInt(w.PY))) + "  Tile at " + v3Place + "  Line : " + line + "\nBUT FAILED");
                    tile = backuptile;
                }

                //Debug.Log("w.OT = " + XmlLoader.W.GetValueByString("OT", w));
                //Debug.Log("w.L = " + XmlLoader.W.GetValueByEnumInt(6, w));
                foreach (WKeyXML wkey in ConditionsTesting)
                {
                    //string Value = (int)wkey.Property
                    if(wkey.Enabled)
                    {
                        XmlLoader.W.PropertyWEnum Prop = wkey.Property;
                        int index = Array.IndexOf(Enum.GetValues(Prop.GetType()), Prop);

                        string Value = XmlLoader.W.GetValueByEnumInt(index, w);

                        if(wkey.Value == Value)
                        {
                            tile = backuptile;
                        }
                    }
                    
                }

                //tile = backuptile;
                bool othertile = !(w.OT.ToLower() == "quickgoal" || 
                w.OT.ToLower() == "tutorial" || 
                w.OT.ToLower() == "music" || 
                w.OT.ToLower() == "weather" || 
                w.OT.ToLower() == "spspawn" || 
                w.OT.ToLower() == "quickgoal" || 
                w.OT.ToLower() == "quickgoal");

                if(w.OT.ToLower() == "worldobject" || w.OT.ToLower() == "ground")
                {
                    
                    //Debug.LogWarning("GROUND");
                    //tile.color = new Color(tile.color.r - (tile.color.r / 10), tile.color.g - (tile.color.g / 10), tile.color.b - (tile.color.b / 10), tile.color.a);
                    
                    //WorldObjects.defaultTilemap.SetTile(v3Place, tile);
                    Int32.TryParse(w.L, out int ParsedL);
                    Int32.TryParse(w.ZL, out int ParsedZL);

                    WorldObjects.Layer(ParsedL, ParsedZL).SetTile(v3Place, tile);
                    WorldObjects.ColorBasedZl(v3Place, ParsedZL, ParsedL);
                    
                    

                    //tile.color = new Color(tile.color.r - (tile.color.r * 10), tile.color.g - (tile.color.g * 10), tile.color.b - (tile.color.b * 10), tile.color.a);
                }
                //else if(w.OT.ToLower() == "background" && background)
                else if(w.OT.ToLower() == "background" && background)
                {
                    //Debug.LogWarning("BACKGROUND");
                    //BackGround.defaultTilemap.SetTile(v3Place, tile);
                    Int32.TryParse(w.L, out int ParsedL);
                    Int32.TryParse(w.ZL, out int ParsedZL);

                    BackGround.Layer(ParsedL, ParsedZL).SetTile(v3Place, tile);
                    BackGround.ColorBasedZl(v3Place, ParsedZL, ParsedL);
                    
                    

                    //SetTileColour(new Color(shadeBackGround, shadeBackGround, shadeBackGround, 255), v3Place, BackGround);

                    //tb.color = new Color(tb.color.r, tb.color.g, tb.color.b, 50);
                    //tb.color = new Color(tile.color.r - (tile.color.r / 10), tile.color.g - (tile.color.g / 10), tile.color.b - (tile.color.b / 10), tile.color.a);
                    
                }
                //else if(w.OT.ToLower() == "trapblock" || w.OT.ToLower() == "trap" || w.OT.ToLower() == "trampoline" || w.OT.ToLower() == "quickgoal")
                else if(othertile)
                {
                    //Debug.LogError("TRAPBLOCK");
                    //Int32.TryParse(w.L, out int ParsedL);
                    //Int32.TryParse(w.ZL, out int ParsedZL);
                    //TrapBlocks.Layer(ParsedL, ParsedZL).SetTile(v3Place, tile);
                    CreateDynamicTile(v3Place, tile, w);
                }


                if((w.CT == "15" || w.CT.ToLower() == "all") && (w.OT.ToLower() == "worldobject" || w.OT.ToLower() == "ground"))
                {
                    Collision.defaultTilemap.SetTile(v3Place, tile);
                    //SetTileColour(new Color(0, 0, 0, 0), v3Place, Collision.defaultTilemap);
                    if(collision == true) SetTileColour(new Color(1, 0, 0, 1), v3Place, Collision.defaultTilemap);
                    else SetTileColour(new Color(1, 0, 0, 0), v3Place, Collision.defaultTilemap);
                    
                }
            }
            

            line++;
        }

        //BackGround.AddCollision();
        //WorldObjects.AddCollision(WorldObjects.defaultTilemap);
        //ForeGround.AddCollision();
        //TrapBlocks.AddCollision(null);
        Collision.AddCollision(Collision.defaultTilemap);

        //WorldObjects.Layer(-1);
    }

    [Serializable]
    public struct WKeyXML
    {
        //public string Letter;
        public XmlLoader.W.PropertyWEnum Property;
        public string Value;
        public bool Enabled;
    }

    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.F))
        {
            Vector3Int tilemapPos = WorldObjects.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            //Tile tile = WorldObjects.GetTile<Tile>(tilemapPos);
        }
        */
    }

    void OnGUI()
    {
        if(DebugMapCreation)
        {
            var mousePosition = Input.mousePosition;

            float x = mousePosition.x;
            float y = Screen.height - mousePosition.y;
            float width = 200;
            float height = 200;
            var rect = new Rect(x + 20, y + 5, width, height);

            Vector3Int tilemapPos = WorldObjects.defaultTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Tile t = WorldObjects.defaultTilemap.GetTile<Tile>(tilemapPos);

            string tileName = "None";

            if(t != null)
            {
                tileName = t.name;
            }
            else if(t == null)
            {
                t = ForeGround.defaultTilemap.GetTile<Tile>(tilemapPos);
                if(t != null)
                {
                    tileName = t.name;
                }
                else if(t == null)
                {
                    t = BackGround.defaultTilemap.GetTile<Tile>(tilemapPos);
                    if(t != null)
                    {
                        tileName = t.name;
                    }
                    else if(t == null)
                    {
                        //t = FindObjectOfType<TilesGet>().iceObjects.GetTile<Tile>(tilemapPos);
                        //if(t != null)
                        //{
                            //tileName = t.name;
                        //}
                    }
                }
            }

            string numtext = "";

            if(TextTile.ContainsKey(tilemapPos))
            {
                numtext = TextTile[tilemapPos].ToString();
            }
            

            GUI.Label(rect, tilemapPos.ToString() + "\n[" + (tilemapPos * 48).ToString() + "]\nNumber Text : " + numtext + "\n" + tileName);
        }

        
    }

    private void CreateDynamicTile(Vector3Int pos, Tile t, XmlLoader.W w)
    {
        Vector3 WorldPos = WorldObjects.defaultTilemap.CellToWorld(pos);
        WorldPos = new Vector3(WorldPos.x + 0.5f, WorldPos.y + 0.5f, 0);
        GameObject ParentTrapsInstance = GameObject.Find("InsatancesTraps");

        GameObject spriteTileDynamic = new GameObject(w.TN + " - " + w.OT);
        spriteTileDynamic.transform.SetParent(ParentTrapsInstance.transform);
        spriteTileDynamic.transform.position = WorldPos;
        if(FindObjectOfType<XmlLoader>().Map.name.Contains("worldmap"))
        {
            spriteTileDynamic.name = w.D;
        }

        SpriteRenderer sp = spriteTileDynamic.AddComponent<SpriteRenderer>();
        sp.sprite = t.sprite;
        sp.color = new Color(1, 1, 1, 1);

        Trap tr = spriteTileDynamic.AddComponent<Trap>();
        tr.tile = t;
        tr.Class = w.OT;

        DynamicTiles.Add(tr.gameObject);
    }

    private void SetTileColour(Color colour, Vector3Int position, Tilemap tilemap)
    {
        // Flag the tile, inidicating that it can change colour.
        // By default it's set to "Lock Colour".
        tilemap.SetTileFlags(position, TileFlags.None);

        // Set the colour.
        tilemap.SetColor(position, colour);
    }

    private int RoundToMultiple(int val, int multiple)
    {
        int rem = val % multiple;
        int result = val - rem;
        if (rem >= (multiple / 2))
            result += multiple;


        return result;
    }
}

[Serializable]
public class TileMapLayer
{
    public Tilemap defaultTilemap;
    [HideInInspector]
    public GameObject parentObject;

    public Tilemap Layer(int layer, int zl)
    {
        zl -= 10;
        layer -= 1;
        //if(zl == 1 && layer == 0)

        if(parentObject == null)
        {
            parentObject = defaultTilemap.transform.parent.gameObject;
        }

        if(layer == 0 && zl == 0)
        {
            return defaultTilemap;
        }

        if(!GameObject.Find("Layer " + layer + " ZL " + zl))
        {
            GameObject TileMapNew = new GameObject("Layer " + layer + " ZL " + zl);
            TileMapNew.transform.SetParent(parentObject.transform);

            Tilemap tilemapTile = TileMapNew.AddComponent<Tilemap>();
            TilemapRenderer tilemapRend = TileMapNew.AddComponent<TilemapRenderer>();

            float CalcZL = (zl / 10000f);
            float CalcL = (layer / 1000f);
            float CalcZ = -(TileMapNew.transform.localPosition.z + CalcL + CalcZL);
            Debug.Log(CalcZL);
            Debug.Log(CalcL);
            Debug.Log(CalcZ);

            TileMapNew.transform.localPosition = new Vector3(TileMapNew.transform.localPosition.x, TileMapNew.transform.localPosition.y, CalcZ);

            return tilemapTile;
        }
        else if(GameObject.Find("Layer " + layer + " ZL " + zl))
        {
            GameObject parentTilemap = GameObject.Find("Layer " + layer + " ZL " + zl);
            Tilemap tilemapMapReq = parentTilemap.GetComponentInChildren<Tilemap>();

            return tilemapMapReq;
        }
        else
        {
            return null;
        }

        
    }

    public void ColorBasedZl(Vector3Int pos, int zl, int layer)
    {
        layer -= 1;
        zl -= 10;

        Tilemap tilemapMapReq;

        GameObject parentTilemap = GameObject.Find("Layer " + layer + " ZL " + zl);
        if(parentTilemap == null && layer == 0)
        {
            tilemapMapReq = defaultTilemap;
        }
        else
        {
            tilemapMapReq = parentTilemap.GetComponentInChildren<Tilemap>();
        }
        
        
        if(zl == 0)
        {
            tilemapMapReq.SetTileFlags(pos, TileFlags.None);
            tilemapMapReq.SetColor(pos, new Color(defaultTilemap.color.r, defaultTilemap.color.g, defaultTilemap.color.b, defaultTilemap.color.a));
        }
        else if(zl == -2)
        {
            tilemapMapReq.SetTileFlags(pos, TileFlags.None);
            //tilemapMapReq.SetColor(pos, new Color(defaultTilemap.color.r - 0.15f, defaultTilemap.color.g - 0.15f, defaultTilemap.color.b - 0.15f, defaultTilemap.color.a - 0.5f));
            tilemapMapReq.SetColor(pos, new Color(1 - 0.15f, 1 - 0.15f, 1 - 0.15f, 1 - 0.5f));
        }
        
    }

    public void AddCollision(Tilemap tilemapIn)
    {
        if(parentObject == null)
        {
            parentObject = defaultTilemap.transform.parent.gameObject;
        }

        if(tilemapIn != null)
        {
            if(!tilemapIn.GetComponent<TilemapCollider2D>())
            {
                TilemapCollider2D collider = tilemapIn.gameObject.AddComponent<TilemapCollider2D>();
                CompositeCollider2D compcollider = tilemapIn.gameObject.AddComponent<CompositeCollider2D>();
                Rigidbody2D rb = tilemapIn.gameObject.GetComponent<Rigidbody2D>();

                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                collider.usedByComposite = true;
            }
            
        }
        else
        {
            foreach(Tilemap tilemap in parentObject.GetComponentsInChildren<Tilemap>())
            {
                if(!tilemap.GetComponent<TilemapCollider2D>())
                {
                    TilemapCollider2D collider = tilemap.gameObject.AddComponent<TilemapCollider2D>();
                    CompositeCollider2D compcollider = tilemap.gameObject.AddComponent<CompositeCollider2D>();
                    Rigidbody2D rb = tilemap.gameObject.GetComponent<Rigidbody2D>();

                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                    rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;

                    collider.usedByComposite = true;
                }
                
            }
        }

        
    }

    public void ClearAllTiles()
    {
        if(parentObject == null)
        {
            parentObject = defaultTilemap.transform.parent.gameObject;
        }

        foreach(Tilemap tilemap in parentObject.GetComponentsInChildren<Tilemap>())
        {
            tilemap.ClearAllTiles();
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MapCreator))]
public class MapCreatorEditor : Editor 
{
    //SerializedProperty lookAtPoint;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //serializedObject.Update();
        //EditorGUILayout.PropertyField(lookAtPoint);
        //serializedObject.ApplyModifiedProperties();

        //LoadLevelRuntime()
        MapCreator myTarget = (MapCreator)target;

        GUILayout.Space(17);

        EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);

        GUILayout.Space(10);


        myTarget.buttonParams = GUILayout.TextField(myTarget.buttonParams);
        if(GUILayout.Button("Draw TileMap"))
        {
            myTarget.LoadLevelRuntime(myTarget.buttonParams);
        }
    }
}

#endif