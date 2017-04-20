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

    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        for (int z = 0; z < ShopCard.Active.towerList.Count; z++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1+z)) // Temperary
            {
                ShopCard.Active.Button_Pressed(ShopCard.Active.towerList[z]);
            }
        }
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MouseState == MOUSE_STATE.SELECT_UNIT)
            {
                Player.Active.SelectUnit(null);
            }
            else
            {
                SetMouseState(MOUSE_STATE.SELECT_UNIT);
            }
        }*/
	}
}
