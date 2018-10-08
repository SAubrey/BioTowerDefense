using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour {

    [HideInInspector] public GameObject SelectedTower;

    public Text towerNamelabel;
    public Button sellTowerButton;

    private Game gameManager;

    public bool sell;


    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("Game").GetComponent<Game>();
        sell = false;
    }

    // Update is called once per frame
    void Update () {

        if(SelectedTower != null){
            enableSellButton();
 
        } else {
            disableSellButton();
        }      
	}

    //Destroys the currently selected tower and adds currecncy for it
    public void sellTower()
    {
        gameManager.Currency += (SelectedTower.GetComponent<Tower>().towerCost / 2);
        Destroy(SelectedTower);
        clearLabels();
        SelectedTower = null;
    }

    //Sets the selected towers labels
    public void setLabels(string towerName, int towerCost){
        towerNamelabel.text = towerName;
        sellTowerButton.GetComponent<Text>().text = "Sell for $" + (towerCost / 2);
        sellTowerButton.enabled = true;
    }

    //Clears the labels once tower is deselected or sold
    public void clearLabels(){
        towerNamelabel.text = "";
        sellTowerButton.GetComponent<Text>().text = "";
    }

    void disableSellButton(){
        sellTowerButton.enabled = false;
        SelectedTower = null;

    }

    void enableSellButton(){
        sellTowerButton.enabled = true;
    }

}
