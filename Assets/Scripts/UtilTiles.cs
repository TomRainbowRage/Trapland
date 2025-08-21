using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Tilemaps;

public class UtilTiles : MonoBehaviour
{
    private Tilemap UtilMap;
    public Tile Origin;

    public GameObject TransportToGMOBJ;
    // Start is called before the first frame update
    public void Start()
    {
        UtilMap = GetComponent<Tilemap>();
        UtilMap.SetTile(new Vector3Int(0, 0, 0), Origin);
    }

    public void TransportTnastance()
    {
        UtilMap = GetComponent<Tilemap>();

        int x = 0;
        int y = 0;

        for(x = UtilMap.cellBounds.min.x; x < UtilMap.cellBounds.max.x; x++)
        {
            for(y = UtilMap.cellBounds.min.y; y < UtilMap.cellBounds.max.y; y++)
            {
                Tile tile = UtilMap.GetTile<Tile>( new Vector3Int(x, y, 0));

                //Debug.Log("Tb = " + tile);
                if(tile != null)
                {
                    GameObject buttonObj = new GameObject() {name = "Level Button "};

                    buttonObj.transform.position = UtilMap.CellToWorld(new Vector3Int(x, y, 0));
                    buttonObj.transform.SetParent(TransportToGMOBJ.transform.GetChild(0));
                    buttonObj.transform.localPosition += new Vector3(50, 50, 0);

                    buttonObj.transform.localScale = new Vector3(1, 1, 1);

                    buttonObj.AddComponent<CanvasRenderer>();

                    Image buttonImage = buttonObj.AddComponent<Image>();
                    buttonImage.sprite = tile.sprite;
                    buttonImage.color = Color.white;

                    Button buttonComp = buttonObj.AddComponent<Button>();
                    buttonComp.interactable = true;
                    buttonComp.transition = Selectable.Transition.None;

                    buttonObj.AddComponent<ButtonScreens>();
                }
            }
        }

        UtilMap.ClearAllTiles();
    
    }

}

#if UNITY_EDITOR

[CustomEditor(typeof(UtilTiles))]
public class UtilTilesEditor : Editor 
{
    //SerializedProperty lookAtPoint;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //serializedObject.Update();
        //EditorGUILayout.PropertyField(lookAtPoint);
        //serializedObject.ApplyModifiedProperties();

        //LoadLevelRuntime()
        UtilTiles myTarget = (UtilTiles)target;

        if(GUILayout.Button("Draw TileMap"))
        {
            myTarget.Start();
        }

        if(GUILayout.Button("Transport Tilemap"))
        {
            myTarget.TransportTnastance();
        }
    }
}

#endif