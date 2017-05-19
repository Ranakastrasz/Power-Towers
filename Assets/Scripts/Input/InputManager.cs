using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RanaLib;
using Pathfinding;
using UnityEngine.UI;


public class InputManager : MonoBehaviour {

    public static InputManager Active;

    // This part will be part of a Pathing-management script instead I think.
    public GameObject SpawnPointObject;
    public GameObject GoalNodeObject;

    private Pathfinding.GraphNode _spawnPoint; 
    private Pathfinding.GraphNode _goalPoint;

    
    public TowerPrototype SelectedTowerPrototype { protected set; get; }
    public GameObject Database; // Prototype database.

    public GameObject PathingChecker; // For checking tower placement pathing.
    public GameObject PlacementPrototype; // For showing placement.

    public GameObject GameSpeedSlider;

    
    //public Color CAN_BUILD                   = new Color(0.5f, 1.0f , 0.5f);


    public static Color TEXT_INSUFFICIENT_RESOURCES = new Color(1.0f, 0.85f, 0.0f);
    public static Color TEXT_CANNOT_BUILD_HERE      = new Color(1.0f, 0.2f , 0.2f);
    
    public enum MOUSE_STATE{
        SELECT_UNIT,
        PLACE_TOWER,
        ADD_SHORT_LINK,
        ADD_LONG_LINK,
        REMOVE_LINK
    }
    
    public MOUSE_STATE MouseState { get; private set; }

    private bool ShiftUsed = false;

    public bool _canPlaceTowerHere = true;
    public TowerPrototype _towerToPlace = null; // If null, none selected.



    private Vector3 _outOfTheWay = new Vector3(20, 0, 0);

    // Use this for initialization
    void Start ()
    {
        Active = this;
        _spawnPoint = AstarPath.active.GetNearest(SpawnPointObject.transform.position).node;
        _goalPoint = AstarPath.active.GetNearest(GoalNodeObject.transform.position).node;
    }
	
	// Update is called once per frame
	void Update ()
    {
        

        if ((ShiftUsed) && (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)))
        {
            SetMouseState(MOUSE_STATE.SELECT_UNIT);
            ShiftUsed = false;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MouseState == MOUSE_STATE.SELECT_UNIT)
            {
                Player.Active.SelectUnit(null);
            }
            else
            {
                SetMouseState(MOUSE_STATE.SELECT_UNIT);
            }
        }

        if (MouseState == MOUSE_STATE.PLACE_TOWER)
        {
            ShowPlacement();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
                Active.CheckShift();
            }
        }
    }

    public static void SelectTower(TowerPrototype iPrototype)
    {
        if (Active.MouseState == MOUSE_STATE.PLACE_TOWER && Active.SelectedTowerPrototype == iPrototype)
        {
            Active.SelectedTowerPrototype = null;
            SetMouseState(MOUSE_STATE.SELECT_UNIT);
        }
        else
        {
            Active.SelectedTowerPrototype = iPrototype;
            SetMouseState(MOUSE_STATE.PLACE_TOWER);

        }
        
    }
    /// <summary>
    /// Changes the MouseState to the give MouseState, or none if it is already set.
    /// </summary>
    /// <param name="iState"></param>
    public static void ToggleMouseState(MOUSE_STATE iState)
    {
        if (Active.MouseState == iState)
        {
            Active.MouseState = MOUSE_STATE.SELECT_UNIT;
            Active.ClearPlacement();
        }
        else
        {
            Active.MouseState = iState;
        }
    }
    public static void SetMouseState(MOUSE_STATE iState)
    {
        if (iState == MOUSE_STATE.SELECT_UNIT)
        {
            Active.ClearPlacement();
        }
        Active.MouseState = iState;
    }

    private void CheckShift()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            ShiftUsed = true;
        }
        else
        {
            SetMouseState(MOUSE_STATE.SELECT_UNIT);
        }
    }

    public static void ClickUnit(Unit iUnit)
    {
        if (Active.MouseState == MOUSE_STATE.SELECT_UNIT)
        {
            Player.Active.SelectUnit(iUnit);
        }
        else if (Active.MouseState == MOUSE_STATE.PLACE_TOWER)
        {
            // Override tower, maybe, but probably not.
        }

        if (iUnit != null && Player.Active._selectedUnit != null)
        {
            if (iUnit is Tower && Player.Active._selectedUnit is Tower)
            {
                PowerManager sourcePowerManager = (Player.Active._selectedUnit as Tower)._powerManager;
                PowerManager targetPowerManager = (iUnit as Tower)._powerManager;
                
                // This really calls for a UI_Button Library or something.
                if (Active.MouseState == MOUSE_STATE.ADD_SHORT_LINK)
                {
                    sourcePowerManager.AddLink(targetPowerManager, false);
                    Active.CheckShift();
                }
                else if (Active.MouseState == MOUSE_STATE.ADD_LONG_LINK)
                {
                    sourcePowerManager.AddLink(targetPowerManager, true);
                    Active.CheckShift();
                }
                else if (Active.MouseState == MOUSE_STATE.REMOVE_LINK)
                {
                    sourcePowerManager.RemoveLink(targetPowerManager);
                    Active.CheckShift();
                }
            }

        }
    }

    public void ShowPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            Vector3 p = hit.point;

            // Figure out mouse position.
            p.x = (float)Math.Round(p.x, 0.5);
            p.y = (float)Math.Round(p.y, 0.5);
            p.z = (float)0.0;


            PlacementPrototype.transform.position = new Vector3(p.x, p.y, p.z);

            _canPlaceTowerHere = validatePlacement(p);


            Redraw();
        }
    }
    public void ClearPlacement()
    {
        PlacementPrototype.transform.position = _outOfTheWay;
        Redraw();
    }
    
    public bool validatePlacement(Vector3 iPosition)
    {
        
        //SpriteRenderer sprite = PlacementPrototype.GetComponent<SpriteRenderer>();
        PathingChecker.transform.position = new Vector3(iPosition.x, iPosition.y, iPosition.z);

        // Check all Unit positions for collision.

        var guo = new GraphUpdateObject(PathingChecker.GetComponent<Collider>().bounds);

        foreach (Unit unit in EntityManager.GetTowers())
        {
            Bounds bounds = unit.gameObject.GetComponent<Collider>().bounds;
            if (PathingChecker.GetComponent<Collider>().bounds.Intersects(bounds))
            {
                
                //sprite.material.SetColor("_Color", new Color(1, 0, 0, 0.5f));
                PathingChecker.transform.position = _outOfTheWay;
                return false;
            }
        }

        // Make sure all runners can get to the goal.


        List<GraphNode> nodes = new List<GraphNode>();

        nodes.Add(_spawnPoint);
        nodes.Add(_goalPoint);

        foreach (Runner runner in EntityManager.GetRunners())
        {
            nodes.Add(AstarPath.active.GetNearest(runner.transform.position).node);
        }

        if (!GraphUpdateUtilities.UpdateGraphsNoBlock(guo, nodes, true))
        {
            //sprite.material.SetColor("_Color", new Color(1, 1, 0, 0.5f));
            PathingChecker.transform.position = _outOfTheWay;
            return false;
        }
        
        //sprite.material.SetColor("_Color", new Color(0, 1, 0, 0.5f));
        PathingChecker.transform.position = _outOfTheWay;
        return true;

    }
    public void PlaceObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            /*
               public static bool CheckBox(Vector3 center, Vector3 halfExtents, 
               Quaternion orientation = Quaternion.identity, int layermask = DefaultRaycastLayers, 
               QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
             */
             
            if (hit.transform.gameObject.tag == "Tower") return;
            
            Vector3 p = hit.point;

            p.x = (float)Math.Round(p.x, 0.5);
            p.y = (float)Math.Round(p.y, 0.5);
            p.z = 0;

            if (_canPlaceTowerHere)
            {
                if (SelectedTowerPrototype != null)
                {
                    int price = SelectedTowerPrototype._price;
                    if (Player.Active.SpendGold(price))
                    {
                        EntityManager.CreateTower(p, SelectedTowerPrototype);
                    }
                    else
                    {
                        EntityManager.CreateFloatingText(p, "Not Enough Resources", 1.0f,TEXT_INSUFFICIENT_RESOURCES);
                    }
                }
            }
            else
            {
                 EntityManager.CreateFloatingText(p, "Can't build here", 1.0f,TEXT_INSUFFICIENT_RESOURCES);
            }

        }
    }
    public void Redraw()
    {
        SpriteRenderer sprite = PlacementPrototype.GetComponent<SpriteRenderer>();
        SpriteRenderer turretSprite = PlacementPrototype.transform.FindChild("Turret").GetComponent<SpriteRenderer>();

        float Red = 0.5f;
        float Green = 0.5f;;
        float Blue = 0.5f;
        float Alpha = 0.8f;

        if (MouseState == MOUSE_STATE.PLACE_TOWER)
        {
            sprite.enabled = true;
            
            if (!_canPlaceTowerHere)
            {
                 Red = 1;
            }
            if (SelectedTowerPrototype != null)
            {
                int price = SelectedTowerPrototype._price;
                if (Player.Active._gold < price)
                {
                    Blue = 1;
                }
            }
            if (Red != 1 && Blue != 1)
            {
                Green = 1;
            }

            sprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));
            turretSprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));
            turretSprite.sprite = (SelectedTowerPrototype != null)? SelectedTowerPrototype._turretSprite : null;
        }
        else
        {
            sprite.enabled = false;
        }
    }
    
    public void SetGameSpeed()
    {
        int speed = (int) GameSpeedSlider.GetComponent<Slider>().value;
        Timer.SetGameSpeed(speed);
        
	}
	public void SetTimeSliderPosition(int iPos)
	{
		GameSpeedSlider.GetComponent<Slider>().value = iPos;
	}

	public void Button_Pause()
	{
		Timer.TogglePause();
	}

	public void Button_Quit()
	{
		Player.Active.Quit ();
	}


}
