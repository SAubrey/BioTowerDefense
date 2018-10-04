using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelected : MonoBehaviour {

    private LineRenderer lineRenderer;
    private bool selected;

	// Use this for initialization
	void Start () {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
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
                //Raycast to see colliders that were hit
                //TODO: If we programmatically detect radius, the Circle Collider 2D on tower prefabs wont be needed
                //And Raycasting won't be as necessary
                RaycastHit2D[] hit;
                Vector2 direction = new Vector2(0, 0);
                hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), direction);

                //If empty area is clicked
                if (hit.Length > 0)
                {
                    foreach (RaycastHit2D h in hit)
                    {
                        //If a detected collider is another game object, hide this towers radius
                        if (h.collider.gameObject != gameObject && h.collider is BoxCollider2D)
                        {
                            lineRenderer.positionCount = 0;
                            selected = false;
                        }
                    }
                }
                else
                {
                    lineRenderer.positionCount = 0;
                    selected = false;
                }
            }
        }
	}


    private void OnMouseDown()
    {
         //Ignore towers on Menu
         if (gameObject.tag == "Tower")
            {
                drawCircle();
                selected = true;
            }
       
    }

    //Draws the Towers radius when it's selected
    public void drawCircle()
    {
        int segments = 360;
        lineRenderer.positionCount = segments + 1;

        int pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * gameObject.GetComponent<CircleCollider2D>().radius, Mathf.Cos(rad) * gameObject.GetComponent<CircleCollider2D>().radius, 0f);
        }

        lineRenderer.SetPositions(points);
        
    }

    //Erases the circle when tower is "deselected"
    public void destroyCircle()
    {
        lineRenderer.positionCount = 0;
    }

    //Sets the color of the Radius Circle
    public void colorCircle(Color col)
    {
        lineRenderer.startColor = col;
        lineRenderer.endColor = col;
    }

}
