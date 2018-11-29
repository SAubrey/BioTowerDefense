using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour {

    [HideInInspector] public GameObject SelectedTower;
    [HideInInspector] public LineRenderer lineRenderer;

    public Text towerNamelabel;
    public Button sellTowerButton;

    public Button reloadTowerAmmoButton;

    private Game gameManager;
    public float sellPercentage = 0.7f;

    private Color defaultColor;
    private float reloadCostDiv;

    void Start () {
        gameManager = GameObject.Find("Game").GetComponent<Game>();
        defaultColor = sellTowerButton.GetComponent<Image>().color;
        reloadCostDiv = __app.reloadCostDiv;
    }

    void Update () {

        // If a tower is selected
        if (SelectedTower != null) {
            
            // Listen to Click events
            if (Input.GetMouseButtonDown(0)) {
                Vector2 direction = new Vector2(0, 0);
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), direction);

                // Set selected tower to null if a collider was hit that's not of another tower or menu.
                if((hit.collider == null) || (hit.collider.tag != "Tower" && hit.collider.tag != "Menu")) {
                    disableSellButton();
                    clearLabels();
                    destroyCircle();
                    SelectedTower.GetComponent<Tower>().selected = false;
                    SelectedTower = null;
                }
            }
        } 
	}

    // Destroys the currently selected tower and adds currecncy for it
    public void sellTower() {
        gameManager.Currency += (int)Mathf.Round(SelectedTower.GetComponent<Tower>().cost * sellPercentage);
        Destroy(SelectedTower);
        clearLabels();
        SelectedTower = null;
        disableSellButton();
    }

    public void reloadTowerAmmo() {
        if (SelectedTower != null) {
            Tower tScript = SelectedTower.GetComponent<Tower>();
            int cost = calcReloadCost();
            if (cost <= gameManager.Currency) {
                gameManager.Currency -= cost;
                tScript.ammo = tScript.maxAmmo;

                __app appScript = GameObject.Find("__app").GetComponent<__app>();
                appScript.increaseMutationChanceForAntibiotic(tScript.antibioticType, ((float)cost/tScript.cost) * reloadCostDiv);
            }
        }
    }

    private int calcReloadCost() {
        Tower tScript = SelectedTower.GetComponent<Tower>();
        float ammoRatio = (float)tScript.ammo / (float)tScript.maxAmmo;
        return (int)Mathf.Round(((float)tScript.cost / reloadCostDiv) * (1f - ammoRatio));
    }

    public void updateReloadText() {
        reloadTowerAmmoButton.GetComponentInChildren<Text>().text = "Refill Tower for $" + calcReloadCost();
    }

    // Sets the selected towers labels
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

    // Clears the labels once tower is deselected or sold
    public void clearLabels() {
        towerNamelabel.text = "";
        reloadTowerAmmoButton.GetComponentInChildren<Text>().text = "Refill Tower";
    }

    // Draws the Towers radius when it's selected
    public void drawEllipse(float radius) {
        int segments = 200;
        lineRenderer.positionCount = segments;
        lineRenderer.alignment = LineAlignment.TransformZ;

        Vector3[] points = new Vector3[segments];
        float angle = 1f;
        float xradius = radius;
        float yradius = radius * __app.ellipseYMult;

        for (int i = 0; i < segments; i++) {
            var x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
            var y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;
            angle += (360f / segments);

            points[i] = new Vector3(x, y + __app.towerShadowYOffset, 0);
        }
        lineRenderer.SetPositions(points);
    }

    public void enableSellButton() {
        sellTowerButton.enabled = true;
        sellTowerButton.GetComponent<Image>().color = Color.green;
    }
    public void disableSellButton() {
        sellTowerButton.enabled = false;
        sellTowerButton.GetComponent<Image>().color = defaultColor;
    }

    // Erases the circle when tower is "deselected"
    public void destroyCircle() {
        if (lineRenderer != null) {
            lineRenderer.positionCount = 0;
        }
    }

    // Sets the color of the radius Circle
    public void colorCircle(Color col) {
        lineRenderer.startColor = col;
        lineRenderer.endColor = col;
    }
}