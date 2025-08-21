using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using Aftershock.Components;
using Aspose.Zip;

public class UnZipTest : MonoBehaviour
{
    public TextAsset ta = null;
    // Start is called before the first frame update
    void Start()
    {
        //TextAsset bindata = Resources.Load("TestBytes") as TextAsset; 
        //Debug.Log("bindata = " + ta.bytes);
        //Debug.Log("bindata to string = " + ta.ToString());
        //XDocument xdoc = _Gzip.UnZipXML(bindata.bytes.ToString());
        //Debug.Log("xdoc = " + xdoc);
        /*

        string responseText = Encoding.UTF8.GetString(ta.bytes);  //  ASCII assumed

        Debug.Log("responseText == " + responseText);
        if(responseText == null)
        {
            Debug.Log("responseText null");
        }
        else if(responseText != null)
        {
            Debug.Log("responseText not null");
            Debug.Log("responseText = "+ responseText);
        }

        //XDocument xdoc = _Gzip.UnZipXML(responseText);
        //Debug.Log("xdoc = " + xdoc);
        //XDocument xdoc = XDocument.Load(responseText);
        using(var ms = new MemoryStream(ta.bytes)) 
        {
            XDocument xdoc = XDocument.Load(ms, LoadOptions.SetBaseUri, );

            if(xdoc == null)
            {
                Debug.Log("xdoc null");
            }
            else if(xdoc != null)
            {
                Debug.Log("xdoc not null");
            }
        }
        */
        
        //string outp = "hello Test";

        //outp = _Gzip.Decompress(ta.bytes);



        
        var bytes = default(byte[]);
        using (var sr = new StreamReader(Application.dataPath + "/Resources/teeeest.xml.zip"))
        {
            
            using (var memstream = new MemoryStream())
            {
                sr.BaseStream.CopyTo(memstream);
                bytes = memstream.ToArray();
            }
        }

        if(bytes == null)
        {
            Debug.Log("bytes = null");
        }
        else if(bytes != null)
        {
            Debug.Log("bytes != null");
        }

        

        XDocument xmlDoc = XDocument.Parse(_Gzip.Decompress(bytes));

        if(xmlDoc == null)
        {
            Debug.Log("xml null");
        }
        else if(xmlDoc != null)
        {
            Debug.Log("xml not null");
        }

        //Debug.Log("output = " + outp);
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
