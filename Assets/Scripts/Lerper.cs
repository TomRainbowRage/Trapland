using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Lerper : MonoBehaviour
{
    internal static Lerper Instance { get; private set; }
    void Awake() {
        Lerper.Instance = this;
    }

    [HideInInspector] public float LerpingValueFLOAT;
    [HideInInspector] public Vector3 LerpingValueVEC;
    public static bool Lerping;

    public void Lerp(float lerpDuration, object startValue, object endValue, ClassType classType)
    {
        StartCoroutine(LerpCoru(lerpDuration, startValue, endValue, classType));
    }

    public IEnumerator LerpCoru(float lerpDuration, object startValue, object endValue, ClassType classType)
    {
        Lerping = true;

        if(classType == ClassType.Float)
        {
            float timeElapsed = 0;
            while (timeElapsed < lerpDuration)
            {
                LerpingValueFLOAT = Mathf.Lerp((float)startValue, (float)endValue, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            LerpingValueFLOAT = (float)endValue;
        }
        else if(classType == ClassType.Vector3)
        {
            float timeElapsed = 0;
            while (timeElapsed < lerpDuration)
            {
                LerpingValueVEC = Vector3.Lerp((Vector3)startValue, (Vector3)endValue, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            LerpingValueVEC = (Vector3)endValue;
        }

        

        Lerping = false;
    }

    public enum ClassType
    {
        Float,
        Vector3,
    }
}
