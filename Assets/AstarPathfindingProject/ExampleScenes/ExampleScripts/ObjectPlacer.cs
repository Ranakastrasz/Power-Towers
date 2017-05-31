using UnityEngine;
using System.Collections;
using Pathfinding;
using RanaLib;

/** Small sample script for placing obstacles */
[HelpURL("http://arongranberg.com/astar/docs/class_object_placer.php")]
public class ObjectPlacer : MonoBehaviour
{
    public GameObject spawnPointNode;
    public GameObject GoalNode;
    public GameObject go; /** GameObject to place. Make sure the layer it is in is included in the collision mask on the GridGraph settings (assuming a GridGraph) */

    public bool validPlacement = true;
    

    public GameObject PlacementPrototype;
	void Start ()
    {

	}

	// Update is called once per frame
	void Update ()
    {
        ShowPlacement();
        if (Input.GetKeyDown("p"))
        {
			PlaceObject();
		}
		if (Input.GetKeyDown("r"))
        {
			RemoveObject();
		}
	}
    
    public void ShowPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground"); // only check for collisions with layerX
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,layerMask))
        {
            /*
               public static bool CheckBox(Vector3 center, Vector3 halfExtents, 
               Quaternion orientation = Quaternion.identity, int layermask = DefaultRaycastLayers, 
               QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
             */

            Vector3 p = hit.point;


            p.x = (float)Math.Round(p.x, 0.5);
            p.y = (float)Math.Round(p.y, 0.5);


            PlacementPrototype.transform.position = new Vector3(p.x, p.y, p.z);
            

            //x = (Mathf.Round(transform.position.x / cell_size) + (float)0.0) * cell_size;

        }
    }

	public void PlaceObject ()
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

            if (hit.transform.gameObject.tag == "Wall") return;

            Vector3 p = hit.point;

            p.x = (float)Math.Round(p.x, 0.5);
            p.y = (float)Math.Round(p.y, 0.5);
            p.z = 0;

            //if (validPlacement)
            {
                /*GameObject obj = (GameObject)*/
                GameObject.Instantiate(go, p, Quaternion.identity);
            }
            //else
            {
                    // Can't build here.
            }
            
		}
	}

	public void RemoveObject ()
    {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

        int layerMask = 1 << 8; // Include only 8th layer.
                                // 8th Layer is Walls.
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,layerMask))
        {
			if (hit.collider.isTrigger || hit.transform.gameObject.name == "Ground") return;

			Destroy(hit.collider);
			Destroy(hit.collider.gameObject);

		}
	}
}
