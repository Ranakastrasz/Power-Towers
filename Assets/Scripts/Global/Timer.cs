using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All the time related stuff.
/// From Gamespeed modification to periodic calls.
/// </summary>
public class Timer : MonoBehaviour {

    static Timer Active;

    public enum TIME_SCALE
    {
        QUARTER,
        HALF,
        FULL,
        DOUBLE,
        QUADRUPLE
    }

    public TIME_SCALE _timeScale { get; protected set; }
    public bool _paused;

    // Use this for initialization
    void Start()
    {
        Active = this;
        _timeScale = TIME_SCALE.FULL;
        SetGameSpeed(TIME_SCALE.FULL);
        InvokeRepeating("Tick", 1.0f, 1.0f);
    }

    private void Tick()
    {
        PowerManager.GlobalTick();

    }

    private void RefreshTimeScale()
    {
        if (_paused)
        {
            Time.timeScale = 0.0f ;
        }
        else
        {
            switch (_timeScale)
            {
                case TIME_SCALE.QUARTER:
                    Time.timeScale = 0.25f;
                    break;
                case TIME_SCALE.HALF:
                    Time.timeScale = 0.5f;
                    break;
                case TIME_SCALE.FULL:
                    Time.timeScale = 1.0f;
                    break;
                case TIME_SCALE.DOUBLE:
                    Time.timeScale = 2.0f;
                    break;
                case TIME_SCALE.QUADRUPLE:
                    Time.timeScale = 4.0f;
                    break;
                default:
                    Time.timeScale = 1.0f;
                    Debug.Log("Timer.SetTimeScale("+_timeScale+") Unexpected TIME_SCALE value");
                    break;
            }
        }
        InputManager.Active.SetTimeSliderPosition((int)_timeScale);
        //Debug.Log("timeScale: " + Time.timeScale);
    }

    
    public static void SetGameSpeed(int iTimeScale)
    {
        Active._timeScale = (TIME_SCALE) iTimeScale;
        Active.RefreshTimeScale();
    }
    
    public static void SetGameSpeed(TIME_SCALE iTimeScale)
    {
        Active._timeScale = iTimeScale;
        Active.RefreshTimeScale();
    }
    
    public static void IncreaseGameSpeed()
    {
        int index = (int)Active._timeScale;

        index = Math.Min(index + 1, 4);

        Active._timeScale = (TIME_SCALE)index;
        Active.RefreshTimeScale();
    }
    public static void DecreaseGameSpeed()
    {
        int index = (int)Active._timeScale;

        index = Math.Max(index - 1, 0);

        Active._timeScale = (TIME_SCALE)index;
        Active.RefreshTimeScale();
    }

    public static void TogglePause()
    {
        Active._paused = !Active._paused;
        Active.RefreshTimeScale();
    }
}
