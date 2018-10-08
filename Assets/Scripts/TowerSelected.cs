using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelected : MonoBehaviour {

    private LineRenderer lineRenderer;
    private TowerManager towerManager;
    private Game gameManager;
    private bool selected;
    private bool mouseOverTower;

    // Use this for initialization
    void Start () {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        gameManager = GameObject.Find("Game").GetComponent<Game>();
        towerManager = GameObject.Find("Game").GetComponent<TowerManager>();
        
    }

    // Update is called once per frame
    void Update () {

        //Accessed when towers selected, detects if mouse is clocked
        //On other tower or gray area
        if (selected)
        {
            //Listen to Click events
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 direction = new Vector2(0, 0);
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), direction);

                //If clicked a different collider or no collider
                if ((hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.gameObject.tag != "SellTower") 
                    || hit.collider == null)
                    {
                    destroyCircle();
                    selected = false;
                    if ((hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.gameObject.tag != "Tower") || (hit.collider == null))
                    {
                        towerManager.clearLabels();
                        towerManager.SelectedTower = null;
                    }
                  
                }

            }
    
        }
	}


    private void OnMouseDown()
    {
         //Ignore towers on Menu
         if (gameObject.tag == "Tower")
            {

                drawCircle(gameObject.GetComponent<Tower>().detectionRadius);

                towerManager.SelectedTower = gameObject;
                towerManager.setLabels(gameObject.GetComponent<Tower>().towerName, gameObject.GetComponent<Tower>().towerCost);
              
                selected = true;
              }
       
    }

    //Display Tower Name if its a menu item and there's no tower currently selected
    private void OnMouseEnter()
    {
      
        // mouseOverTower = true;
        if (towerManager.SelectedTower == null && gameObject.tag == "MenuItems")
        {
            towerManager.towerNamelabel.text = gameObject.GetComponent<Tower>().towerName;
        }
    }

    //Clear tower name
    private void OnMouseExit()
    {
        // mouseOverTower = false;
        if (towerManager.SelectedTower == null && gameObject.tag == "MenuItems")
        {
            towerManager.towerNamelabel.text = "";
        }
    }

    //Draws the Towers radius when it's selected
    public void drawCircle(float radius)
    {
        int segments = 360;
        lineRenderer.positionCount = segments + 1;

        int pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0f);
        }

        lineRenderer.SetPositions(points);
        
    }

    //Erases the circle when tower is "deselected"
    public void destroyCircle()
    {
        lineRenderer.positionCount = 0;
    }

    //Sets the color of the radius Circle
    public void colorCircle(Color col)
    {
        lineRenderer.startColor = col;
        lineRenderer.endColor = col;
    }

}
