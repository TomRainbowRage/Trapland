using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Trap : MonoBehaviour
{
    public Tile tile;
    public string Class;
    public bool hasContainedChildren;

    // Start is called before the first frame update
    /*
    void Start()
    {
        if(Class.ToLower() == "spinner")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        }
        else if(Class.ToLower() == "trapblock")
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Class.ToLower() == "spinner") { SpinnerUpdate(); }
    }

    void SpinnerUpdate()
    {

    }
    */

    void Start()
    {
        if(Class.ToLower() == "spinner")
        {
            var trap = gameObject.AddComponent<Spinner>();
            trap.tile = tile;
            trap.Class = Class;
            trap.hasContainedChildren = hasContainedChildren;
            trap.trapClass = this;
        }
    }
}

public class Spinner : Trap
{
    [HideInInspector] public Trap trapClass;
    //public override bool hasContainedChildren;
    void Start()
    {
        //this.tile;
        Destroy(trapClass);
        Debug.Log("SPINNER CLASS : " + this.Class + " Tile : " + this.tile);
    }
}