using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
	bool cd = false;
	float cdTime = 0f;
	public float coolDown = 0f;
	public int targetType = 0;//0=first, 1=last, 2=lowestHP, 3=highestHP, 4=self(aoe), 5=all in radius
	public int projectileType = 0;//0 = Pellet, 1 = Laser, 2 = AOE
	public float projectileSize = 0f;
	public float projectileSpeed;
	public float projectileAOE = 0f;
	public Sprite projectileSprite;
	public bool projectilePierce;	
	public float detectionRadius;
	public int specialEffect = 0;//0 = none, 1 = slow, 2 = increase damage taken, etc.
	private GameObject target = null;
	private GameObject projectile;
    public int towerCost;
	// Use this for initialization
	void Start () {
		GameObject projectile  = Resources.Load("Prefabs/Projectile") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(tag=="MenuItems"){
			return;
		}
		if (Game.paused || !Game.game) {
			return;
		}
		if(!cd){
			var objects = GameObject.FindGameObjectsWithTag("Enemy");
			var objectCount = objects.Length;
			//Numbers used for targetType
			var minHP = 100000f;
			var maxHP = 0f;
			//This is not a if/else statement because it must be checked after the previous if statement
			if(target == null){
				foreach (var obj in objects) {
					//Get Distance
					var myDist = Mathf.Sqrt(Mathf.Pow(obj.transform.position.x - transform.position.x,2f) + Mathf.Pow(obj.transform.position.y - transform.position.y,2f));
					if(myDist <= detectionRadius){
						//Based on TargetType
						if(targetType==0){
							target = obj;
							break;
						}
						else if(targetType==4){
							
						}
						else if(targetType==5){
							shoot(obj);
						}
					}
				}
			}
			else{
				//Check if target is out of bounds
				var targetDist = Mathf.Sqrt(Mathf.Pow(target.transform.position.x - transform.position.x,2f) + Mathf.Pow(target.transform.position.y - transform.position.y,2f));
				if(targetDist > detectionRadius){
					target = null;
				}
				if(targetType==0){
					shoot(target);
				}
			}
		}
		else{
			if(cdTime<=0f){
				cd = false;
			}
			cdTime-=1f;
		}
	}
	
	private void shoot(GameObject obj){
		GameObject projectile  = Resources.Load("Prefabs/Projectile") as GameObject;
		if(projectileType == 0){
			GameObject myProjectile = Instantiate(projectile);
			myProjectile.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
			var run = obj.transform.position.x - transform.position.x;
			var rise = obj.transform.position.y - transform.position.y;
			var myDist = Mathf.Sqrt(Mathf.Pow(run,2f) + Mathf.Pow(rise,2f));
			var myXsp = (run / myDist) * projectileSpeed;
			var myYsp = (rise / myDist) * projectileSpeed;
			myProjectile.GetComponent<Projectile>().setVals(projectileType,projectileSize,projectileSpeed,projectileAOE,projectilePierce,specialEffect,gameObject,myXsp,myYsp);
		}
		else{
			
		}
		cd = true;
		cdTime = coolDown;
	}
}
