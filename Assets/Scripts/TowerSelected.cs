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
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //If empty area is clicked
                if (!Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    lineRenderer.positionCount = 0;
                    selected = false;
                } else {

                    //If another gameobject is clicked
                    if (hit.collider.gameObject != gameObject)
                    {
                        lineRenderer.positionCount = 0;
                        selected = false;

                    }
                }
            }
        }
	}

    private void OnMouseDown()
    {
        if (gameObject.tag == "Tower")
        {
            drawCircle();
            selected = true;
        }
    }

    //Draws the Towers radius when it's selected
    public void drawCircle()
    {
        Debug.Log("HITTING DRAW CIRCLE");
        int segments = 360;
        lineRenderer.positionCount = segments + 1;

        int pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * gameObject.GetComponent<SphereCollider>().radius, Mathf.Cos(rad) * gameObject.GetComponent<SphereCollider>().radius, 0);
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
