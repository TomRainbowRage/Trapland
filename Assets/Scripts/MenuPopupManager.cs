using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuPopupManager : MonoBehaviour
{
    public GameObject textEnter = null;
    public bool textEnterBool = false;

    public GameObject messageBox = null;

    public GameObject inputBool = null;
    public Button opt1 = null;
    public Button opt2 = null;
    [HideInInspector] public bool inputBoolBool = false;

    public Button buttonBefore;
    
    
    
    public void InputBox(string title)
    {
        buttonBefore = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        MenuScreens.Instance.selectionIntectable = false;

        textEnterBool = true;

        textEnter.SetActive(true);
        textEnter.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = title;

        GameObject InputFieldTMP = textEnter.GetComponentInChildren<TMP_InputField>().gameObject;

        InputFieldTMP.GetComponent<TMP_InputField>().Select();
        EventSystem.current.SetSelectedGameObject(InputFieldTMP);

        foreach(Button b in Resources.FindObjectsOfTypeAll<Button>())
        {
            b.interactable = false;
        }
    }

    public void MessageBox(string text, bool show, bool needrecordbutton = true)
    {
        if(needrecordbutton == true)
        {
            buttonBefore = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        }
        
        if(show)
        {
            MenuScreens.Instance.selectionIntectable = false;

            messageBox.SetActive(true);
            messageBox.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = text;

            foreach(Button b in Resources.FindObjectsOfTypeAll<Button>())
            {
                b.interactable = false;
            }
        }
        else if(!show)
        {
            messageBox.SetActive(false);
            messageBox.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";

            foreach(Button b in Resources.FindObjectsOfTypeAll<Button>())
            {
                b.interactable = true;
            }

            buttonBefore.Select();
            EventSystem.current.SetSelectedGameObject(buttonBefore.gameObject);

            MenuScreens.Instance.selectionIntectable = true;
        }
        
    }
    public void MessageBox(string text, float time)
    {
        buttonBefore = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        StartCoroutine(MessageBoxAsync(text, time));
    }
    public IEnumerator MessageBoxAsync(string text, float time)
    {
        MessageBox(text, true, false);

        yield return new WaitForSeconds(time);

        MessageBox(text, false, false);
    }

    public void InputBool(string title)
    {
        buttonBefore = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        MenuScreens.Instance.selectionIntectable = false;

        inputBoolBool = true;

        inputBool.SetActive(true);
        inputBool.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = title;

        opt1.Select();
        EventSystem.current.SetSelectedGameObject(opt1.gameObject);

        foreach(Button b in Resources.FindObjectsOfTypeAll<Button>())
        {
            if(!(b == opt1 || b == opt2))
            {
                b.interactable = false;
            }
        }
    }

    public void Update()
    {

        if(Input.GetKeyDown(KeyCode.Return) && textEnterBool == true)
        {
            //Debug.Log("GetKeyDown " + "");
            if(textEnter.GetComponentInChildren<TMP_InputField>().text != "")
            {
                textEnterBool = false;

                //Debug.Log("GetKeyDown " + "TEXT");

                string Title = textEnter.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
                string EnteredText = textEnter.GetComponentInChildren<TMP_InputField>().text;

                textEnter.GetComponentInChildren<TMP_InputField>().text = "";

                textEnter.SetActive(false);

                //buttonBefore.Select();
                //EventSystem.current.SetSelectedGameObject(buttonBefore.gameObject);

                foreach(Button b in Resources.FindObjectsOfTypeAll<Button>())
                {
                    b.interactable = true;
                }

                

                MenuScreens.Instance.selectionIntectable = true;

                MenuScreens.Instance.GetActiveScreenState().InputBox(Title, EnteredText);

                
            }
            
        }

        if(Input.GetKeyDown(KeyCode.Return) && inputBoolBool == true && inputBool.GetComponentInChildren<TMP_InputField>().text != "")
        {
            inputBoolBool = false;

            //string EnteredText = inputBool.GetComponentInChildren<TMP_InputField>().text;
            //inputBoolText = EnteredText;
            bool inputBoolOut = false;

            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if(selected != null)
            {
                if(selected == opt1.gameObject)
                {
                    inputBoolOut = true;
                }
                else if(selected == opt2.gameObject)
                {
                    inputBoolOut = false;
                }
                else
                {
                    Debug.Log("Error In InputBoolOut");
                    inputBoolOut = false;
                }
            }
            else
            {
                Debug.Log("Error In InputBoolOut");
                inputBoolOut = false;
            }

            inputBool.GetComponentInChildren<TMP_InputField>().text = "";

            inputBool.SetActive(false);

            foreach(Button b in Resources.FindObjectsOfTypeAll<Button>())
            {
                b.interactable = true;
            }

            //buttonBefore.Select();
            //EventSystem.current.SetSelectedGameObject(buttonBefore.gameObject);
            
            MenuScreens.Instance.selectionIntectable = true;

            MenuScreens.Instance.SetSelectionNull();

            MenuScreens.Instance.GetActiveScreenState().InputBool(inputBoolOut);

        }
    }

    public string GetTitle()
    {
        string returnnew = "";
        if(textEnter.activeSelf) {returnnew = textEnter.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;}
        else if(messageBox.activeSelf) {returnnew = messageBox.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;}
        else if(inputBool.activeSelf) {returnnew = inputBool.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;}
        else
        {
            returnnew = "null";
        }

        return returnnew;

    }
}
