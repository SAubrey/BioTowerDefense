using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    private Color validColor = Color.green;
    private Color invalidColor = Color.red;

    private bool dragging = false;
    private float distance;

    private Vector3 originalPosition;
    private TowerSelected towerSelect;

    // Use this for initialization
    void Start()
    {
        towerSelect = gameObject.GetComponent<TowerSelected>();
    }

    void OnMouseDown()
    {
        if (gameObject.tag == "MenuItems")
        {
            originalPosition = gameObject.transform.position;
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 80f);
            Vector3 wordPos = Camera.main.ScreenToWorldPoint(mousePos);

            gameObject.layer = 2;

            distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            dragging = true;
            //drawCircle();
            towerSelect.drawCircle();
        }   
    }

    //Invoked when the mouse is released and towers dropped
    void OnMouseUp()
    {
        if (gameObject.tag == "MenuItems")
        {
          
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            validateDropPosition(mousePos);
            towerSelect.destroyCircle();

        }
        dragging = false;

    }

    void Update()
    {
        //Handles updating tower movement when being dragged
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            gameObject.transform.position = rayPoint;
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            if (validSpot(mousePos)){
                towerSelect.colorCircle(validColor);
            } else {
                towerSelect.colorCircle(invalidColor);

            }

        }
    }

    //Destroys instantiated tower upon dropping if it's in an invalid area
    void validateDropPosition(Vector3 pos)
    {
    
        if (validSpot(pos)) {
            gameObject.tag = "Tower";
            LoadTowers menuTower = gameObject.GetComponentInParent<LoadTowers>();
            gameObject.transform.parent = null;
            menuTower.reloadTowers();

        }
        //If invalid, move tower back to original spot
        else
        {
            gameObject.transform.position = originalPosition;
        }
        gameObject.layer = 0;


    }

    //returns false if the current position's in an invalid area, true if it's not
    bool validSpot(Vector3 position)
    {

        //Get length of the towers box collider
        float boxLengthX = gameObject.GetComponent<BoxCollider>().size.x;
        float boxLengthY = gameObject.GetComponent<BoxCollider>().size.y;

        //Get the corner points of the towers box collider
        Vector3 upRightCorner = new Vector3(gameObject.transform.position.x + boxLengthX / 2, gameObject.transform.position.y + boxLengthY / 2, -20.7f);
        Vector3 lowRightCorner = new Vector3(gameObject.transform.position.x + boxLengthX / 2, gameObject.transform.position.y - boxLengthY / 2, -20.7f);
        Vector3 upLeftCorner = new Vector3(gameObject.transform.position.x - boxLengthX / 2, gameObject.transform.position.y + boxLengthY / 2, -20.7f);
        Vector3 lowLeftCorner = new Vector3(gameObject.transform.position.x - boxLengthX / 2, gameObject.transform.position.y - boxLengthY / 2, -20.7f);
        Vector3 direction = new Vector3(0, 0, 1.0f);

        //Create the rays for each corner points
        Ray ray1 = new Ray(upRightCorner, direction);
        Ray ray2 = new Ray(lowRightCorner, direction);
        Ray ray3 = new Ray(upLeftCorner, direction);
        Ray ray4 = new Ray(lowLeftCorner, direction);
  
        List<RaycastHit> hits = new List<RaycastHit>();

        //TODO: adds redundant collisions most of the time, need to figure out how to reduce redundant collisions
        hits.AddRange(Physics.RaycastAll(ray1, Mathf.Infinity));
        hits.AddRange(Physics.RaycastAll(ray2, Mathf.Infinity));
        hits.AddRange(Physics.RaycastAll(ray3, Mathf.Infinity));
        hits.AddRange(Physics.RaycastAll(ray4, Mathf.Infinity));

        if (hits.Count > 0)
        {

            //Search through each collision, if any of these tags found, it's an invalid area
            foreach (RaycastHit h in hits)
            {
            if (h.collider.gameObject.tag == "Path" || h.collider.gameObject.tag == "Base" || h.collider.gameObject.tag == "Menu")
                {
                    return false;
                }
                //Ignore the sphere collider on the tower, only the box collider means an invalid area
                else if (h.collider.gameObject.tag == "Tower")
                {
                    if (h.collider.GetType() == typeof(BoxCollider))
                    {
                        return false;
                    }
                }
            }
        }
        return true;

    }

}

