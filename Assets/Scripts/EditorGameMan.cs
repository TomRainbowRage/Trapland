using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorGameMan : MonoBehaviour
{
    public GameObject enableManagerObject;

    void Awake()
    {
        if(GameManager.Instance == null)
        {
            enableManagerObject.transform.SetParent(null);
            Debug.Log("Enabling Manager");
            enableManagerObject.SetActive(true);
            
        }
    }
}
