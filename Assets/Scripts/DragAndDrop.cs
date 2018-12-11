using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {
    private Color validColor = Color.green;
    private Color invalidColor = Color.red;

    private bool dragging = false;
    private float distance;

    private Vector3 originalPosition;
    private TowerManager towerManager;
    private LoadTowers loadTowers;
    private Game gameManager;
    private Tower myTower;

    private bool offMenu = false;

    private TowerPlacement shadow;
    
    void Start() {
        gameManager = GameObject.Find("Game").GetComponent<Game>();
        towerManager = GameObject.Find("Game").GetComponent<TowerManager>();
        loadTowers = GameObject.Find("TowerPortion").GetComponent<LoadTowers>();
        myTower = gameObject.GetComponent<Tower>();
        shadow = gameObject.GetComponentInChildren<TowerPlacement>();
    }

    void Update() {
        //Handles updating tower movement when being dragged
        if (dragging) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(distance);
            RaycastHit2D hit = Physics2D.Raycast(rayPoint, new Vector2(0,0));
            Debug.Log("OFFMENU IS " + offMenu);
            if(hit.collider == null || (hit.collider.gameObject.tag != "Menu" && hit.collider.gameObject.tag != "MenuItems")){
                offMenu = true;
                
                gameObject.transform.position = new Vector3(rayPoint.x, rayPoint.y, rayPoint.y);
                if (validSpot() ) {
                    
                    towerManager.colorCircle(validColor);
                }
                else {
                    towerManager.colorCircle(invalidColor);
                }   

                  if(offMenu == true){
                    myTower.towerNickname.enabled = false;
                    towerManager.drawEllipse(myTower.detectionRadius);
                }
            } else{
                gameObject.transform.position = originalPosition;
                myTower.towerNickname.enabled = true;
                towerManager.destroyCircle();

            }
            
        }
    }

    // Invoked when towers clicked
    void OnMouseDown() {
        if (!Game.paused) {
		    towerManager.destroyCircle();
            towerManager.SelectedTower = gameObject;
            towerManager.disableSellButton();
            towerManager.setLabels(myTower.towerName, myTower.cost);
            towerManager.lineRenderer = gameObject.GetComponent<LineRenderer>();

            gameObject.layer = 2;
           

            // If user has enough money to buy tower
            if (gameManager.Currency >= myTower.cost) {
                originalPosition = gameObject.transform.position;
                distance = Vector2.Distance(transform.position, Camera.main.transform.position);
                dragging = true;
                //Debug.Log(""WHY ARE YOU NOT ENTERING"");

              
            }
        }
    }

    // Invoked when towers released
    void OnMouseUp() {
        if(!Game.paused) {
            if (gameObject.tag == "MenuItems" && gameManager.Currency >= myTower.cost) {
                validateDropPosition();
            }
            dragging = false;
            gameObject.layer = 0;
            offMenu = false;
        }
    }

    //Moves tower back to menu if dropped on an invalid area
    // Detaches sideMenu as parent if towers dropped on valid area
    void validateDropPosition() {
        if (validSpot()) {
            detachFromMenu();
            updateTheMenu();
            towerManager.clearLabels();
            towerManager.destroyCircle();

            gameManager.Currency -= myTower.cost;
            towerManager.SelectedTower = null;

            Destroy(this);
        }
        //If invalid, move tower back to original spot, destroy radius circle, but tower remains "selected"
        else {
            gameObject.transform.position = originalPosition;
            towerManager.destroyCircle();
            myTower.towerNickname.enabled = true;
        }
    }

    //Upon successful purchase of tower, detach it from the menu.
    void detachFromMenu() {
        gameObject.tag = "Tower";
        shadow.tag = "TowerShadow";
        myTower.ammoBarWhole.SetActive(true);
        gameObject.transform.SetParent(null);

        // Increase mutation probability.
		__app appScript = GameObject.Find("__app").GetComponent<__app>();
        
		appScript.increaseMutationChanceForAntibiotic(myTower.antibioticType);
    }

    //Upon successful drop, reload an instance of the tower to the sidemenu
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