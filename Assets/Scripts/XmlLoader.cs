using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Linq;
using System;
using System.Runtime;
using System.Reflection;

public class XmlLoader : MonoBehaviour
{
    public TextAsset Map;
    [SerializeField]
    public Main main;
    private IDictionary<string, int> ThemeToOffset = new Dictionary<string, int>()
    {
        {"castle", 10},
        {"desert", -1},
        {"hell", 0},
        {"ice", 8},
        {"plains", 8}
    };

    public void Awake()
    {
        string MapName = Map.name;
        
        string[] MapsPaths = new string[]
        {
            "DLC/Unity",
            "DLC/Custom",
            "Deathmatch",
            "Story",
            "World_1",
            "World_2",
            "World_3",
            "World_4",
            "World_5",
            "World_6",
            "Worldmaps"
            
        };

        string mappath = "";
        foreach(string p in MapsPaths)
        {
            Debug.Log("Checking : " + p);
            if(File.Exists(Application.dataPath + "/Resources/Content/Maps/" + p + "/" + MapName + ".xml"))
            {
                Debug.Log("Sucsess");
                mappath = Application.dataPath + "/Resources/Content/Maps/" + p + "/" + MapName + ".xml";
            }
        }
        

        XmlSerializer serializer = new XmlSerializer(typeof(Main));

        using (StreamReader reader = new StreamReader(mappath))
        {
            main = (Main)serializer.Deserialize(reader);
            Debug.Log("Main = " + main);
        }
    }

    public void ChangeLevel(string LevelName)
    {
        
    }

    //MAKE GET VALUE TYPE

    //<W TN="Object_World79" N="Object_World79" PX="1152" PY="432" S="1" R="0" L="2" OT="Ground" CT="None" ZL="8" F="0" M="False" D="" />
    //TileName, Name/Class?, XVal, YVal, Size, Rotation, Sort (ONLY SAME TILE), LayerGround, Collision, Alpha??, Better Bet For Alpha, 
    //CT = 15 (ONLY TILES THAT WERE NOT SHADED IN BACKGROUND) (COLLISION)
    //D = "a-1" IF worldmap (PROBS DATA?)

    void Start()
    {
    }

    //object_castle23
    //castle_main8
    
    public string GetTilePath(string InTile, Vector2 PosTile)
    {
        string PathEnd = "";

        string theme = "";
        string type = "";
        int tileindex;

        //int NumbersIn = GetAmountOfNumbersInString(InTile);

        string[] Split_ = InTile.Split('_');
        theme = Split_[0];

        if(theme.ToLower() == "object")
        {
            type = Split_[0];

            char[] type_index = Split_[1].ToCharArray();
            //Int32.TryParse(type_index[type_index.Length - NumbersIn].ToString(), out int ind);
            //tileindex = ind;
            tileindex = GetNumbersFromString(Split_[1]);

            //Debug.Log("Numbers In String is " + GetAmountOfNumbersInString(Split_[1]));

            //Debug.Log("type_index = " + ReadableCharArray(type_index) + " Length = " + (type_index.Length - 1) + "  Index 4 = " + ()type_index[4]);

            //type = Split_[1].Remove(Split_[1].Length - NumbersIn, NumbersIn);
            theme = RemoveNumbersFromString(Split_[1]);

            if(theme.ToLower() == "world")
            {
                theme = "plains";
            }

            PathEnd = "Tiles/" + FirstLetterToUpper(type) + "_" + FirstLetterToUpper(theme) + "_Tiles/" + FirstLetterToUpper(type) + "_" + FirstLetterToUpper(theme) + "_Tileset_" + tileindex ;
        }
        else if(theme.ToLower() == "traps")
        {
            PathEnd = "TRAP";
        }
        else if(theme.ToLower() != "object")
        {
            //char[] type_index = Split_[1].ToCharArray();
            //Int32.TryParse(type_index[type_index.Length - NumbersIn].ToString(), out int ind);
            //tileindex = ind;
            tileindex = GetNumbersFromString(Split_[1]);

            //Debug.Log("Numbers In String is " + GetAmountOfNumbersInString(Split_[1]));

            //Debug.Log("type_index = " + ReadableCharArray(type_index) + " Length = " + (type_index.Length - 1) + "  Index 4 = " + ()type_index[4]);

            //type = Split_[1].Remove(Split_[1].Length - NumbersIn, NumbersIn);
            type = RemoveNumbersFromString(Split_[1]);

            //Debug.Log("theme = " + theme + "  type = " + type + "  tileindex = " + tileindex);

            PathEnd = "Tiles/" + FirstLetterToUpper(theme) + "_Tiles/" + FirstLetterToUpper(theme) + "_" + FirstLetterToUpper(type) + "_Tileset_" + (tileindex - 1) ;
        }

        

        return PathEnd;
    }

    

    public static string ReadableCharArray(char[] clist)
    {
        string PrintString = "";
        foreach(char c in clist)
        {
            PrintString += "[" + c + "], ";
        }

        return PrintString;
    }

    public static string FirstLetterToUpper(string input)
    {
        //char[] charlist = input.ToCharArray();
        //charlist[0] = char.ToUpper(charlist[0]);
        //return charlist.ToString();
        return(input.Substring(0, 1).ToUpper() + input.Substring(1).ToLower());
    }

    public static int GetAmountOfNumbersInString(string numstring)
    {
        int returnint = 0;
        char[] chararray = numstring.ToCharArray();

        foreach(char c in chararray)
        {
            if(c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
            {
                returnint += 1;
            }
        }

        return returnint;
    }

    public static int GetNumbersFromString(string numstring)
    {
        string stringnums = "";
        char[] chararray = numstring.ToCharArray();

        foreach(char c in chararray)
        {
            if(c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
            {
                stringnums += c;
            }
        }

        Int32.TryParse(stringnums, out int outparse);

        return outparse;
    }

    public static string RemoveNumbersFromString(string numstring)
    {
        string stringnonums = "";
        char[] chararray = numstring.ToCharArray();

        foreach(char c in chararray)
        {
            if(!(c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9'))
            {
                stringnonums += c;
            }
        }

        return stringnonums;
    }

    public static float StringToInt(string instring)
    {
        //float testing = 77.33f;
        //int upround = Mathf.RoundToInt(testing);


        if(Int32.TryParse(instring, out int i))
        {
            return i;
        }
        else
        {
            Debug.LogError("Failed to Pass Int " + instring + "\nTrying Pass Rounded");

            if(float.TryParse(instring, out float f))
            {
                //Debug.Log("Passed Float " + Mathf.RoundToInt(f));
                //return Mathf.RoundToInt(f);
                return f;
            }
            else
            {
                Debug.LogError("Failed to Pass Float " + instring);
                return -1;
            }

            



            
        }
        
    }

    public static Vector3 Between(Vector3 v1, Vector3 v2, float percentage)
    {
        return (v2 - v1) * percentage + v1;
    }

    [XmlRoot(ElementName="W"), System.Serializable]
    public class W { 

        [XmlAttribute(AttributeName="TN")] 
        public string TN { get; set; } 

        [XmlAttribute(AttributeName="N")] 
        public string N { get; set; } 

        [XmlAttribute(AttributeName="PX")] 
        public string PX { get; set; } 

        [XmlAttribute(AttributeName="PY")] 
        public string PY { get; set; } 

        [XmlAttribute(AttributeName="S")] 
        public string S { get; set; } 

        [XmlAttribute(AttributeName="R")] 
        public string R { get; set; } 

        [XmlAttribute(AttributeName="L")] 
        public string L { get; set; } 

        [XmlAttribute(AttributeName="OT")] 
        public string OT { get; set; } 

        [XmlAttribute(AttributeName="CT")] 
        public string CT { get; set; } 

        [XmlAttribute(AttributeName="ZL")] 
        public string ZL { get; set; } 

        [XmlAttribute(AttributeName="F")] 
        public string F { get; set; } 

        [XmlAttribute(AttributeName="M")] 
        public string M { get; set; } 

        [XmlAttribute(AttributeName="D")] 
        public string D { get; set; }

        public static string GetValueByString(string letter, W w)
        {
            string value = "";
            if(letter == "TN") {value = w.TN;}
            else if(letter == "N") {value = w.N;}
            else if(letter == "PX") {value = w.PX;}
            else if(letter == "PY") {value = w.PY;}
            else if(letter == "S") {value = w.S;}
            else if(letter == "R") {value = w.R;}
            else if(letter == "L") {value = w.L;}
            else if(letter == "OT") {value = w.OT;}
            else if(letter == "CT") {value = w.CT;}
            else if(letter == "ZL") {value = w.ZL;}
            else if(letter == "F") {value = w.F;}
            else if(letter == "M") {value = w.M;}
            else if(letter == "D") {value = w.D;}

            return value;
        }

        public static string GetValueByEnumInt(int index, W w)
        {
            string value = "";
            if(index == 0) {value = w.TN;}
            else if(index == 1) {value = w.N;}
            else if(index == 2) {value = w.PX;}
            else if(index == 3) {value = w.PY;}
            else if(index == 4) {value = w.S;}
            else if(index == 5) {value = w.R;}
            else if(index == 6) {value = w.L;}
            else if(index == 7) {value = w.OT;}
            else if(index == 8) {value = w.CT;}
            else if(index == 9) {value = w.ZL;}
            else if(index == 10) {value = w.F;}
            else if(index == 11) {value = w.M;}
            else if(index == 12) {value = w.D;}

            return value;
        }

        public enum PropertyWEnum
        {
            TN,
            N,
            PX,
            PY,
            S,
            R,
            L,
            OT,
            CT,
            Zl,
            F,
            M,
            D
        }
    }

    [XmlRoot(ElementName="Zone"), System.Serializable]
    public class Zone { 

        [XmlElement(ElementName="W")] 
        public List<W> W { get; set; } 

        [XmlAttribute(AttributeName="Name")] 
        public string Name { get; set; } 
    }

    [XmlRoot(ElementName="Main"), System.Serializable]
    public class Main { 

        [XmlElement(ElementName="Zone")] 
        public Zone Zone { get; set; } 

        [XmlAttribute(AttributeName="Palette")] 
        public string Palette { get; set; } 

        [XmlAttribute(AttributeName="SaveOptions")] 
        public string SaveOptions { get; set; } 

        [XmlAttribute(AttributeName="Offset")] 
        public string Offset { get; set; } 
    }
}

/*
    [XmlRoot(ElementName="W")]
    public class W { 

        [XmlAttribute(AttributeName="TN")] 
        public string TN { get; set; } 

        [XmlAttribute(AttributeName="N")] 
        public string N { get; set; } 

        [XmlAttribute(AttributeName="PX")] 
        public int PX { get; set; } 

        [XmlAttribute(AttributeName="PY")] 
        public int PY { get; set; } 

        [XmlAttribute(AttributeName="S")] 
        public int S { get; set; } 

        [XmlAttribute(AttributeName="R")] 
        public int R { get; set; } 

        [XmlAttribute(AttributeName="L")] 
        public int L { get; set; } 

        [XmlAttribute(AttributeName="OT")] 
        public string OT { get; set; } 

        [XmlAttribute(AttributeName="CT")] 
        public string CT { get; set; } 

        [XmlAttribute(AttributeName="ZL")] 
        public int ZL { get; set; } 

        [XmlAttribute(AttributeName="F")] 
        public int F { get; set; } 

        [XmlAttribute(AttributeName="M")] 
        public string M { get; set; } 

        [XmlAttribute(AttributeName="D")] 
        public string D { get; set; } 
    }

    [XmlRoot(ElementName="Zone")]
    public class Zone { 

        [XmlElement(ElementName="W")] 
        public List<W> W { get; set; } 

        [XmlAttribute(AttributeName="Name")] 
        public string Name { get; set; } 
    }

    [XmlRoot(ElementName="Main")]
    public class Main { 

        [XmlElement(ElementName="Zone")] 
        public Zone Zone { get; set; } 

        [XmlAttribute(AttributeName="Palette")] 
        public string Palette { get; set; } 

        [XmlAttribute(AttributeName="SaveOptions")] 
        public string SaveOptions { get; set; } 

        [XmlAttribute(AttributeName="Offset")] 
        public string Offset { get; set; } 
    }
*/


