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
    private TowerManager towerManager;
    private LoadTowers loadTowers;
    private Game gameManager;
    private Tower myTower;

    private TowerPlacement shadow;
    
    // Use this for initialization
    void Start() {
        gameManager = GameObject.Find("Game").GetComponent<Game>();
        towerManager = GameObject.Find("Game").GetComponent<TowerManager>();
        loadTowers = gameObject.GetComponentInParent<LoadTowers>();
        myTower = gameObject.GetComponent<Tower>();
        shadow = gameObject.GetComponentInChildren<TowerPlacement>();
    }

    void Update() {
        //Handles updating tower movement when being dragged
        if (dragging) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(distance);
            //Debug.Log(rayPoint);
            gameObject.transform.position = new Vector3(rayPoint.x, rayPoint.y, rayPoint.y);

            if (validSpot()) {
                towerManager.colorCircle(validColor);
            }
            else {
                towerManager.colorCircle(invalidColor);
            }
        }
    }

    //Invoked when towers clicked
    void OnMouseDown() {
		    towerManager.destroyCircle();
            towerManager.SelectedTower = gameObject;
            towerManager.disableSellButton();
            towerManager.setLabels(myTower.towerName, myTower.cost);
            gameObject.layer = 2;

        //if user has enough money to buy tower
        if (gameManager.Currency >= myTower.cost) {

            towerManager.lineRenderer = gameObject.GetComponent<LineRenderer>();
            originalPosition = gameObject.transform.position;
            distance = Vector2.Distance(transform.position, Camera.main.transform.position);
            dragging = true;

            towerManager.drawCircle(myTower.detectionRadius);
        }
    }

    //Invoked when towers released
    void OnMouseUp() {
        if (gameObject.tag == "MenuItems" && gameManager.Currency >= myTower.cost) {
            validateDropPosition();
        }
        dragging = false;
        gameObject.layer = 0;
    }

    //Moves tower back to menu if dropped on an invalid area
    //Dissattaches sideMenu as parent if towers dropped on valid area
    void validateDropPosition() {
        if (validSpot()) {
            
            detachFromMenu();
            updateTheMenu();

            gameManager.Currency -= myTower.cost;
            towerManager.SelectedTower = null;

            Destroy(this);
        }
        //If invalid, move tower back to original spot, destroy radius circle, but tower remains "selected"
        else {
            gameObject.transform.position = originalPosition;
            towerManager.destroyCircle();
        }
    }

    //Upon successful purchase of tower, detach it from the menu.
    void detachFromMenu() {
        gameObject.tag = "Tower";
        gameObject.transform.SetParent(null);

        // Increase mutation probability.
		__app appScript = GameObject.Find("__app").GetComponent<__app>();
		appScript.increaseMutationChanceForAntibiotic(myTower.antibioticType);
    }

    //Upon successful drop, reload the an instance of the tower to the sidemenu
    void updateTheMenu() {
        loadTowers.reloadTower(gameObject.GetComponent<Tower>().towerName, originalPosition);
    }

    //returns false if the current position's overlapping any other colliders
    bool validSpot() {
        if (shadow.hitting == 0) {
            return true;
        }
        return false;
    }
}