﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour {

    [HideInInspector] public GameObject SelectedTower;
    [HideInInspector] public LineRenderer lineRenderer;

    public Text towerNamelabel;
    public Button sellTowerButton;

    private Game gameManager;
    public float sellPercentage = 0.7f;


    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("Game").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update () {

        //IF a tower is selected
        if (SelectedTower != null) {
            
            //Listen to Click events
            if (Input.GetMouseButtonDown(0)) {
                Vector2 direction = new Vector2(0, 0);
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), direction);

                //Set selected tower to null if a collider was hit that's not of another tower or menu.
                if((hit.collider == null) || (hit.collider.tag != "Tower" && hit.collider.tag != "Menu")) {
                        disableSellButton();
                        clearLabels();
                        destroyCircle();
                        SelectedTower = null;
                }
               
            }
        } 
	}

    //Destroys the currently selected tower and adds currecncy for it
    public void sellTower() {
        gameManager.Currency += (int)Mathf.Round(SelectedTower.GetComponent<Tower>().cost * sellPercentage);
        Destroy(SelectedTower);
        clearLabels();
        SelectedTower = null;
        disableSellButton();
    }

    //Sets the selected towers labels
    public void setLabels(string towerName, int towerCost) {
        towerNamelabel.text = towerName;

        //If MenuItem show buy price, if already bought, show sell price
        if (SelectedTower.tag == "Tower") {
           towerNamelabel.text += ("\n Sell for $" + Mathf.Round(towerCost * sellPercentage));
        }
        else {
            towerNamelabel.text += "\n $" + towerCost.ToString();
        }
    }

    //Clears the labels once tower is deselected or sold
    public void clearLabels() {
        towerNamelabel.text = "";
    }

    //Draws the Towers radius when it's selected
    public void drawCircle(float radius) {
        int segments = 360;
        lineRenderer.positionCount = segments + 1;

        int pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++) {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0f);
        }

        lineRenderer.SetPositions(points); 
    }

    public void enableSellButton() {
        sellTowerButton.enabled = true;
        sellTowerButton.GetComponent<Image>().color = Color.green;

    }
    public void disableSellButton() {
        sellTowerButton.enabled = false;
        sellTowerButton.GetComponent<Image>().color = Color.white;
    }

    //Erases the circle when tower is "deselected"
    public void destroyCircle() {
        if (lineRenderer != null) {
            lineRenderer.positionCount = 0;
        }
    }

    //Sets the color of the radius Circle
    public void colorCircle(Color col) {
        lineRenderer.startColor = col;
        lineRenderer.endColor = col;
    }
}