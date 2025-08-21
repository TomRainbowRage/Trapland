using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayLocal : ScreenState
{
    public override void OnSelectionChange(GameObject selected, GameObject lastSelected) 
    {
        if(!IsButtonInScreen(selected.GetComponent<Button>(), FindObjectOfType<PlayLocal>().gameObject))
        {
            return;
        }

        if(selected.name != "Back")
            {
                selected.transform.localScale = new Vector3(1.8f, 1.8f, 1);
                selected.transform.localPosition = new Vector3(selected.transform.localPosition.x, 47, selected.transform.localPosition.z);
            }
            else if(selected.name == "Back")
            {
                selected.transform.localScale = new Vector3(1.61f, 1.61f, 1);
            }
            if(lastSelected != null)
            {
                if(!IsButtonInScreen(lastSelected.GetComponent<Button>(), FindObjectOfType<PlayLocal>().gameObject))
                {
                    return;
                }
                if(lastSelected.name != "Back")
                {
                    lastSelected.transform.localScale = new Vector3(1.4f, 1.4f, 1);
                    lastSelected.transform.localPosition = new Vector3(lastSelected.transform.localPosition.x, 23, lastSelected.transform.localPosition.z);
                }
                else if(lastSelected.name == "Back")
                {
                    lastSelected.transform.localScale = new Vector3(1.4f, 1.4f, 1);
                }
            }
    }
    
    public override void OnClickButton(string buttonname) 
    {
        //Debug.Log("Set ModeReady");
        GameManager.Instance.ModeReady = buttonname.ToLower();
        
    }
    
    void Start() 
    {
        Debug.Log("DEFAULT BUTTON  " + StartButton);
        //EventSystem.current.firstSelectedGameObject = StartButton.gameObject;
        GameManager.Instance.ModeReady = "";
        //GameManager.Instance.ModeReady = buttonname.ToLower();

        StartButton.Select();
        EventSystem.current.SetSelectedGameObject(StartButton.gameObject);

        foreach (Button b in this.gameObject.GetComponentsInChildren<Button>())
        {
            GameObject button = b.gameObject;

            if(!IsButtonInScreen(b, FindObjectOfType<PlayLocal>().gameObject))
            {
                return;
            }
            if(button.name != "Back")
            {
                button.transform.localScale = new Vector3(1.4f, 1.4f, 1);
                button.transform.localPosition = new Vector3(button.transform.localPosition.x, 23, button.transform.localPosition.z);
            }
            else if(button.name == "Back")
            {
                button.transform.localScale = new Vector3(1.4f, 1.4f, 1);
            }
        }

        EventSystem.current.currentSelectedGameObject.transform.localScale = new Vector3(1.8f, 1.8f, 1);

    }
    
    void Update()
    {
        foreach (Button b in this.gameObject.GetComponentsInChildren<Button>())
        {
            
            GameObject button = b.gameObject;

            if(!IsButtonInScreen(b, FindObjectOfType<PlayLocal>().gameObject))
            {
                return;
            }

            //Debug.Log("Button = " + b.gameObject.name + " && Current = " + EventSystem.current.currentSelectedGameObject.name);
            
            if(button.transform.localScale == new Vector3(1.8f, 1.8f, 1) && EventSystem.current.currentSelectedGameObject != button)
            {
                button.transform.localScale = new Vector3(1.4f, 1.4f, 1);
            }
        }

        if(EventSystem.current.currentSelectedGameObject != null) { EventSystem.current.currentSelectedGameObject.transform.localScale = new Vector3(1.8f, 1.8f, 1); }


    }
}
