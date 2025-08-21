using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.IO;

public class TileSetter : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tilePlace;

    public List<Vector4> v4s = new List<Vector4>();
    void Start ()
    {
        /*
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++) 
        {
            for (int y = 0; y < bounds.size.y; y++) 
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null) 
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                } else 
                {
                    Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }
        */
        /*
        XmlSerializer serializer = new XmlSerializer(typeof(Tileset));
        using (StringReader reader = new StringReader(Application.dataPath + "Resources/Content/Tilesets/Castle/Castle_MainXML.xml"))
        {
            var test = (Tileset)serializer.Deserialize(reader);
        }
        */
        /*
        XmlSerializer serializer = new XmlSerializer(typeof(Tileset));

        using (StreamReader reader = new StreamReader(Application.dataPath + "/Resources/Content/Tilesets/Castle/Castle_MainXML.xml"))
        {
            Tileset tileset = (Tileset)serializer.Deserialize(reader);

            foreach(Tile t in tileset.Tile)
            {
                
                Vector4 v4 = StringToVector4(t.Rectangle);

                Debug.Log(v4);
                v4s.Add(v4);

                tilemap.SetTile(new Vector3Int((int)v4[0] / 48, (int)v4[1] / 48, 0), tilePlace);
            }
        }
        
        Debug.Log(Application.dataPath); //D:/unity projects/Trapland/Assets
        */

        //tilemap.SetTile(new Vector3Int(1, 1, 0), tilePlace);


        

    }   

    [XmlRoot(ElementName="tile")]
    public class Tile { 

        [XmlAttribute(AttributeName="Name")] 
        public string Name { get; set; } 

        [XmlAttribute(AttributeName="Rectangle")] 
        public string Rectangle { get; set; } 
    }

    [XmlRoot(ElementName="tileset")]
    public class Tileset { 

        [XmlElement(ElementName="tile")] 
        public List<Tile> Tile { get; set; } 

        [XmlAttribute(AttributeName="Path")] 
        public string Path { get; set; } 
    }

    public static Vector4 StringToVector4(string sVector)
	{
        /*
		// Remove the parentheses
		if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
			sVector = sVector.Substring(1, sVector.Length-2);
		}
        */

		// split the items
		string[] sArray = sVector.Split(' ');
		// store as a Vector3
		Vector4 result = new Vector4(
			float.Parse(sArray[0]),
			float.Parse(sArray[1]),
			float.Parse(sArray[2]),
            float.Parse(sArray[3]));

		return result;
	}
}
