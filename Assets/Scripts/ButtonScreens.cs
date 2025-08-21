using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScreens : MonoBehaviour
{
    public GameObject SetActiveScreen;
    [SerializeField] public List<object> buttonData = new List<object>();
    
    Button selfbutton;
    void Awake()
    {
        selfbutton = GetComponent<Button>();
    }
    
    public void Pressed()
    {
        Debug.Log("Button Pressed!! " + this.gameObject.name);
        if(SetActiveScreen != null) // Button Wants to change screen.
        {
            FindObjectOfType<MenuScreens>().TempOnClickName(selfbutton.gameObject.name);
            FindObjectOfType<MenuScreens>().ChangeScene(SetActiveScreen);
            //FindObjectOfType<MenuScreens>().TempOnClickName(selfbutton.gameObject.name);
        }
        else if(SetActiveScreen == null) // Button Wants to custom script action
        {
            if(MenuScreens.Instance.selectionIntectable)
            {
                FindObjectOfType<MenuScreens>().TempOnClickName(selfbutton.gameObject.name);
            }
            
        }
        
    }
}
