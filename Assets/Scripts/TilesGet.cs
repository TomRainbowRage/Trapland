using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;
using System;
using System.Linq;

public class TilesGet : MonoBehaviour
{
    public static TilesGet instance;

    //public Tilemap iceObjects;
    public Tile ErrorTile;
    //public List<Tile> allTiles = new List<Tile>();
    //public TileBase[] allTiles;
    //public Tile backuptile;

    //public List<TileMapTiles> DictTiles = new List<TileMapTiles>();
    /*
    public IDictionary<string, TileMapTiles> IDictTiles = new Dictionary<string, TileMapTiles>()
    {
        {"castle_main", new TileMapTiles()},
        {"desert_main", new TileMapTiles()},
        {"hell_main", new TileMapTiles()},
        {"ice_main", new TileMapTiles()},
        {"plains_main", new TileMapTiles()},

        {"object_castle", new TileMapTiles()},
        {"object_desert", new TileMapTiles()},
        {"object_hell", new TileMapTiles()},
        {"object_ice", new TileMapTiles()},
        {"object_world", new TileMapTiles()}
    };
    */
    public IDictionary<string, TileMapTiles> IDictTiles = new Dictionary<string, TileMapTiles>() {};

    
    public List<IDictSneak> DictTiles = new List<IDictSneak>();
    

    void Awake()
    {
        instance = this;
        /*
        //iceObjects.SetTile(new Vector3Int(0, 0, 0), backuptile);
        BoundsInt bounds = iceObjects.cellBounds;
        //Array.Reverse(allTiles);
        
        for(int y = 0; y < bounds.size.y; y++)
        {
            for(int x = 0; x < bounds.size.x; x++)
            {
                allTiles.Add(iceObjects.GetTile<Tile>(new Vector3Int(x, -y, 0)));
            }
        }

        iceObjects.gameObject.SetActive(false);

        */

        for(int i = 0; i < this.transform.childCount; i++)
        {
            Tilemap tilemap = this.transform.GetChild(i).GetComponent<Tilemap>();
            //Debug.Log("tilemap = " + tilemap);
            //Debug.Log("GameObject Child = " + this.transform.GetChild(i).gameObject);
            BoundsInt Mapbounds = tilemap.cellBounds;

            TileMapTiles tilemaptiles = new TileMapTiles()
            {
                map = tilemap,
                tiles = new List<Tile>()
            };

            for(int y = 0; y < Mapbounds.size.y; y++)
            {
                for(int x = 0; x < Mapbounds.size.x; x++)
                {
                    Tile t = tilemap.GetTile<Tile>(new Vector3Int(x, -y, 0));
                    //Debug.Log("t = " + t);
                    tilemaptiles.tiles.Add(t);
                }
            }

            IDictTiles.Add(tilemap.gameObject.name, tilemaptiles);
            DictTiles.Add(new IDictSneak() {key = tilemap.gameObject.name, value = tilemaptiles});
            /*
            if(IDictTiles.ContainsKey(tilemap.gameObject.name))
            {
                Debug.Log("Contains KEY");
                IDictTiles[tilemap.gameObject.name] = tilemaptiles;
                
            }
            */
            
            /*
            if(DictTiles.Contains<IDictSneak>(new IDictSneak() {key = tilemap.gameObject.name}))
            {
                Debug.Log("THOGUH IF");
                DictTiles[]
            }
            */

            tilemap.gameObject.SetActive(false);

        }

        //Debug.Log("Got " + IDictTiles.Count + " Tilemaps Loaded");

    }

    public Tile GetTileNewProg(string InTile)
    {
        Tile t = ErrorTile;
        /*
        if(InTile.Contains("object_ice"))
        {
            string[] Split_ = InTile.Split('_');
            int tileindex = XmlLoader.GetNumbersFromString(Split_[1]) - 1;

            t = allTiles[tileindex];
        }
        else if(!InTile.Contains("object_ice"))
        {
            XmlLoader xmlLoad = FindObjectOfType<XmlLoader>();
            t = (Tile)Resources.Load(xmlLoad.GetTilePath(InTile, Vector2.zero)) as Tile;
        }
        */
        string InTileTheme = XmlLoader.RemoveNumbersFromString(InTile).ToLower();
        int index = XmlLoader.GetNumbersFromString(InTile);

        if(IDictTiles.ContainsKey(InTileTheme))
        {
            TileMapTiles tilemaptiles = IDictTiles[InTileTheme];
            //Debug.Log("tilemaptiles = " + tilemaptiles);
            //Debug.Log("tiles list = " + tilemaptiles.tiles.Count);
            //Debug.Log("Get Tile t = " + tilemaptiles.tiles[index - 1]);
            t = tilemaptiles.tiles[index - 1];
        }

        return t;
    }

    public Tile EmptyTile()
    {
        Tile t = ErrorTile;
        return t;
    }

    public bool IsEmpty(Tile t)
    {
        if(t == ErrorTile)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    
    [Serializable]
    public struct TileMapTiles
    {
        public Tilemap map;
        public List<Tile> tiles;            
    }

    [Serializable]
    public struct IDictSneak
    {
        public string key;
        public TileMapTiles value;            
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(TilesGet))]
public class TilesGetEditor : Editor 
{
    //SerializedProperty lookAtPoint;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //serializedObject.Update();
        //EditorGUILayout.PropertyField(lookAtPoint);
        //serializedObject.ApplyModifiedProperties();

        //LoadLevelRuntime()
        TilesGet myTarget = (TilesGet)target;
        /*
        if(GUILayout.Button("Draw TileMap"))
        {
            //myTarget.Start();
        }
        */
    }
}

#endif