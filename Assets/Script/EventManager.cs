using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public static event Action<GameObject>showHideInfo;
    public static event Action screenTap;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public static void ShowHideInfo(GameObject gameObject)
    {
        showHideInfo?.Invoke(gameObject);
    }

    public static void ScreenTapEvent()
    {
        screenTap?.Invoke();
    }
}
