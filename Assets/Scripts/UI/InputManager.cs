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


    public GameObject TowerPrefab; // Tower Prefab to place when making a new Tower.
    public GameObject Database; // Prototype database.

    public GameObject TowerContainer; // Where to put new Towers.
    public GameObject PathingChecker; // For checking tower placement pathing.
    public GameObject PlacementPrototype; // For showing placement.

    public enum MOUSE_STATE{
        NONE,
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

        if (Input.GetKeyDown("q"))
        {
            UIManager.UpgradeTower();
        }
        else if (Input.GetKeyDown("r"))
        {
            UIManager.SellTower();
        }

        if (Input.GetKeyDown("t"))
        {
            if (MouseState == MOUSE_STATE.PLACE_TOWER)
            {
                MouseState = MOUSE_STATE.NONE;
                ClearPlacement();
            }
            else
            {
                MouseState = MOUSE_STATE.PLACE_TOWER;
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


    public bool validatePlacement(Vector3 iPosition)
    {

        PathingChecker.transform.position = new Vector3(iPosition.x, iPosition.y, iPosition.z);

        // Check all Unit positions for collision.

        var guo = new GraphUpdateObject(PathingChecker.GetComponent<Collider>().bounds);

        /*foreach (Unit unit in Unit.UnitList)
        {
            Bounds bounds = unit.gameObject.GetComponent<Collider>().bounds;
            if (PathingChecker.GetComponent<Collider>().bounds.Intersects(bounds))
            {

                PathingChecker.transform.position = OutOfTheWay;
                return false;
            }
        }*/

        // Make sure all creeps can get to the goal.


        List<GraphNode> nodes = new List<GraphNode>();

        nodes.Add(spawnPoint);
        nodes.Add(goalPoint);

        foreach (Creep creep in Creep.creepList)
        {
            nodes.Add(AstarPath.active.GetNearest(creep.transform.position).node);
        }

        if (!GraphUpdateUtilities.UpdateGraphsNoBlock(guo, nodes, true))
        {
            PathingChecker.transform.position = OutOfTheWay;
            return false;
        }

        PathingChecker.transform.position = OutOfTheWay;
        return true;

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
                Green = 1;


                sprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));

            }
            else
            {

                Red = 1;

                sprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));
            }
        }
        else
        {
            sprite.enabled = false;
        }
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

                int price = Database.GetComponent<PrototypeDatabase>().Wall.Price;
                if (Player.Active.SpendGold(price))
                {
                    GameObject obj = (GameObject)GameObject.Instantiate(TowerPrefab, p, Quaternion.identity);
                    obj.transform.parent = TowerContainer.transform;
                    obj.GetComponent<Tower>().ApplyPrototype(Database.GetComponent<PrototypeDatabase>().Wall);
                }
                else
                {
                    print("Not Enough Gold");
                }
            }
            else
            {
                // Can't build here.
            }

        }
    }
}
