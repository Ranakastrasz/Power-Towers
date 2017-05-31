using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {

    /*
    List<Action> list = new List<Action>();
    list.Add( () => ClassA.MethodX(paramM) );
    list.Add( () => ClassB.MethodY(paramN, ...) );

    foreach (Action a in list) {
        a.Invoke();
    }
         */


    // Register a keypress -> Functioncall. Possibly with an Argument, but only an Integer.
    // Integer index would let you lookup whatever you need to in that function, no need for
    // Anything more complicated.

    // Throw exception for double registration, I think

    // Allow modded keys, E.g. Shift-Q
    // Make sure modded keys get priority.

    // Loop all registered keys on update.
    // Call corresponding functions with integer metadata.

    // All hotkeys will flow through this class.

	public static KeyManager Active;
    public List<Hotkey> _list = new List<Hotkey>();

    
	// Use this for initialization
	void Start ()
    {
		Active = this;

    }
	
	// Update is called once per frame
	void Update ()
    {

		foreach(Hotkey hotkey in _list)
        {
            if (hotkey.check()) hotkey.call();
        }
        
    }

	public void Clear()
	{
		_list.Clear ();

	}
}
