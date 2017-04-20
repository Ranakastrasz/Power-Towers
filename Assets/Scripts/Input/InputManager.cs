using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RanaLib;
using Pathfinding;

public class InputManager : MonoBehaviour {

    public static InputManager Active;

    // This part will be part of a Pathing-management script instead I think.
    public GameObject SpawnPointObject;
    public GameObject GoalNodeObject;

    private Pathfinding.GraphNode spawnPoint; 
    private Pathfinding.GraphNode goalPoint;

    
    public TowerPrototype SelectedTowerPrototype { protected set; get; }
    public GameObject Database; // Prototype database.

    public GameObject PathingChecker; // For checking tower placement pathing.
    public GameObject PlacementPrototype; // For showing placement.
    
    public enum MOUSE_STATE{
        SELECT_UNIT,
        PLACE_TOWER,
        ADD_SHORT_LINK,
        ADD_LONG_LINK,
        REMOVE_LINK
    }
    
    public MOUSE_STATE MouseState { get; private set; }

    public bool CanPlaceTowerHere = true;
    public TowerPrototype TowerToPlace = null; // If null, none selected.

    private Vector3 OutOfTheWay = new Vector3(20, 0, 0);

    // Use this for initialization
    void Start ()
    {
        Active = this;
        spawnPoint = AstarPath.active.GetNearest(SpawnPointObject.transform.position).node;
        goalPoint = AstarPath.active.GetNearest(GoalNodeObject.transform.position).node;
    }
	
	// Update is called once per frame
	void Update ()
    {

        /*if (Input.GetKeyDown("q"))
        {
            UIManager.UpgradeTower();
        }
        else if (Input.GetKeyDown("r"))
        {
            UIManager.SellTower();
        }

        if (Input.GetKeyDown("t"))
        {
            SetMouseState(MOUSE_STATE.PLACE_TOWER);
          
        }*/

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

        if (iUnit != null && Player.Active.selectedUnit != null)
        {
            if (iUnit is Tower && Player.Active.selectedUnit is Tower)
            {
                PowerManager sourcePowerManager = (Player.Active.selectedUnit as Tower).PowerManager;
                PowerManager targetPowerManager = (iUnit as Tower).PowerManager;
                
                // This really calls for a UI_Button Library or something.
                if (Active.MouseState == MOUSE_STATE.ADD_SHORT_LINK)
                {
                    sourcePowerManager.AddLink(targetPowerManager, false);
                }
                else if (Active.MouseState == MOUSE_STATE.ADD_LONG_LINK)
                {
                    sourcePowerManager.AddLink(targetPowerManager, true);

                }
                else if (Active.MouseState == MOUSE_STATE.REMOVE_LINK)
                {
                    sourcePowerManager.RemoveLink(targetPowerManager);
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

            CanPlaceTowerHere = validatePlacement(p);


            Redraw();
        }
    }
    public void ClearPlacement()
    {
        PlacementPrototype.transform.position = OutOfTheWay;
        Redraw();
    }
    
    public bool validatePlacement(Vector3 iPosition)
    {
        
                SpriteRenderer sprite = PlacementPrototype.GetComponent<SpriteRenderer>();
        PathingChecker.transform.position = new Vector3(iPosition.x, iPosition.y, iPosition.z);

        // Check all Unit positions for collision.

        var guo = new GraphUpdateObject(PathingChecker.GetComponent<Collider>().bounds);

        foreach (Unit unit in EntityManager.GetTowers())
        {
            Bounds bounds = unit.gameObject.GetComponent<Collider>().bounds;
            if (PathingChecker.GetComponent<Collider>().bounds.Intersects(bounds))
            {
                
                sprite.material.SetColor("_Color", new Color(1, 0, 0, 0.5f));
                PathingChecker.transform.position = OutOfTheWay;
                return false;
            }
        }

        // Make sure all creeps can get to the goal.


        List<GraphNode> nodes = new List<GraphNode>();

        nodes.Add(spawnPoint);
        nodes.Add(goalPoint);

        foreach (Creep creep in EntityManager.GetCreeps())
        {
            nodes.Add(AstarPath.active.GetNearest(creep.transform.position).node);
        }

        if (!GraphUpdateUtilities.UpdateGraphsNoBlock(guo, nodes, true))
        {
                sprite.material.SetColor("_Color", new Color(1, 1, 0, 0.5f));
            PathingChecker.transform.position = OutOfTheWay;
            return false;
        }
        
                sprite.material.SetColor("_Color", new Color(0, 1, 0, 0.5f));
        PathingChecker.transform.position = OutOfTheWay;
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

            if (CanPlaceTowerHere)
            {
                if (SelectedTowerPrototype != null)
                {
                    int price = SelectedTowerPrototype.Price;
                    if (Player.Active.SpendGold(price))
                    {
                        EntityManager.CreateTower(p, SelectedTowerPrototype);
                    }
                    else
                    {
                        print("Not Enough Gold");
                    }
                }
            }
            else
            {
                // Can't build here.
            }

        }
    }
    public void Redraw()
    {
        SpriteRenderer sprite = PlacementPrototype.GetComponent<SpriteRenderer>();

        float Red = 0;
        float Green = 0;
        float Blue = 0.2f;
        float Alpha = 0.5f;

        if (MouseState == MOUSE_STATE.PLACE_TOWER)
        {
            sprite.enabled = true;
            if (CanPlaceTowerHere)
            {
                // Valid placement shows up as green
                Green = 1;


                sprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));

            }
            else
            {
                // Invalid placement is red.
                Red = 1;

                sprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));
            }
        }
        else
        {
            sprite.enabled = false;
        }
    }

}
