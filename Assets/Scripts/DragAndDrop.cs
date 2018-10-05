using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    private Color validColor = Color.green;
    private Color invalidColor = Color.red;

    private bool dragging = false;
    private float distance;

    private Vector2 originalPosition;
    private TowerSelected towerSelect;
    private Game gameManager;
    
    // Use this for initialization
    void Start()
    {
        towerSelect = gameObject.GetComponent<TowerSelected>();
        gameManager = GameObject.Find("Game").GetComponent<Game>();
    }

    //Invoked when towers clicked
    void OnMouseDown()
    {
        if (gameObject.tag == "MenuItems" && gameManager.Currency >= gameObject.GetComponent<Tower>().towerCost)
        {
            originalPosition = gameObject.transform.position;

            gameObject.layer = 2;

            distance = Vector2.Distance(transform.position, Camera.main.transform.position);
            dragging = true;
            towerSelect.drawCircle(gameObject.GetComponent<Tower>().detectionRadius);
        }
    }

    //Invoked when towers released
    void OnMouseUp()
    {
        if (gameObject.tag == "MenuItems" && gameManager.Currency >= gameObject.GetComponent<Tower>().towerCost)
        {

            validateDropPosition();
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
            Vector2 rayPoint = ray.GetPoint(distance);
            gameObject.transform.position = rayPoint;
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (validSpot())
            {
                towerSelect.colorCircle(validColor);
            }
            else
            {
                towerSelect.colorCircle(invalidColor);
            }

        }
    }

    //Moves tower back to menu if dropped on an invalid area
    //Dissattaches sideMenu as parent if towers dropped on valid area
    void validateDropPosition()
    {

        if (validSpot()) 
        {
            gameObject.tag = "Tower";

            LoadTowers menuTower = gameObject.GetComponentInParent<LoadTowers>();
            gameObject.transform.parent = null;

            string towerName = gameObject.name;
            towerName = towerName.Replace("(Clone)", "");
            menuTower.reloadTower(towerName);


            gameManager.Currency -= gameObject.GetComponent<Tower>().towerCost;

        }
        //If invalid, move tower back to original spot
        else 
        {
            gameObject.transform.position = originalPosition;
        }
        gameObject.layer = 0;


    }

    //returns false if the current position's overlapping any other colliders
    bool validSpot()
    {

        //Get length of the towers box collider
        float boxLengthX = gameObject.GetComponent<BoxCollider2D>().size.x;
        float boxLengthY = gameObject.GetComponent<BoxCollider2D>().size.y;

        //Get the corner points of the towers box collider
        Vector2 upRightCorner = new Vector2(gameObject.transform.position.x + boxLengthX / 2, gameObject.transform.position.y + boxLengthY / 2);
        Vector2 lowRightCorner = new Vector2(gameObject.transform.position.x + boxLengthX / 2, gameObject.transform.position.y - boxLengthY / 2);
        Vector2 upLeftCorner = new Vector2(gameObject.transform.position.x - boxLengthX / 2, gameObject.transform.position.y + boxLengthY / 2);
        Vector2 lowLeftCorner = new Vector2(gameObject.transform.position.x - boxLengthX / 2, gameObject.transform.position.y - boxLengthY / 2);
        Vector2 direction = new Vector2(0, 0);

        //Add all Colliders that are hit from the four corners of the dragging towers box collider
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        hits.AddRange(Physics2D.RaycastAll(upRightCorner, direction));
        hits.AddRange(Physics2D.RaycastAll(lowRightCorner, direction));
        hits.AddRange(Physics2D.RaycastAll(upLeftCorner, direction));
        hits.AddRange(Physics2D.RaycastAll(lowLeftCorner, direction));

        if (hits.Count > 0)
        {
            return false;
        }
        return true;
           }
}

