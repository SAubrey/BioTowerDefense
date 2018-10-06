using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelected : MonoBehaviour {

    private LineRenderer lineRenderer;
    private Game gameManager;
    private bool selected;
    private bool mouseOverTower;

	// Use this for initialization
	void Start () {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        gameManager = GameObject.Find("Game").GetComponent<Game>();
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

                if ((hit.collider != null && hit.collider.gameObject != gameObject) || hit.collider == null)
                {
                    lineRenderer.positionCount = 0;
                    selected = false;
                }
            }
            else if (Input.GetMouseButtonDown(1)){
                sellTower();
            }
        }
	}


    private void OnMouseDown()
    {
         //Ignore towers on Menu
         if (gameObject.tag == "Tower")
            {
                drawCircle(gameObject.GetComponent<Tower>().detectionRadius);
                selected = true;

            }
       
    }

    private void OnMouseEnter()
    {
        mouseOverTower = true;
    }

    private void OnMouseExit()
    {
        mouseOverTower = false;
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

    private void sellTower(){
        if(mouseOverTower){
            Destroy(gameObject);
            gameManager.Currency += (gameObject.GetComponent<Tower>().towerCost / 2);
        }
    }

}
