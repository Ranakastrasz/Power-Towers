using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hotkey
{
    KeyCode _key;
    Action<int> _action;
    int _argument;

    public Hotkey(KeyCode iKey, Action<int> iAction, int iArgument)
    {
        _key = iKey;
        _action = iAction;
        _argument = iArgument;
    }

    public bool check()
    {
        if (Input.GetKeyDown(_key))
        {
            return true;
            //_action = delegate (int i) { Timer.TogglePause(); };
        }
        return false;
    }

    public void call()
    {
        _action(_argument);
    }
    /*
        A hotkey needs
        A keycode,
        Modkey list. (shift, etc)
        Action to call
        Metadata Int.


        If modkeys don't match, ignore the action tho.


        Create - Populate stuff

        Something about if it is pressed
        Something about checking if it is pressed, and then throwing the action.
     */

}
