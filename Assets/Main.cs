using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	public static Vector3 spawnLocation = new Vector3(0F, 2.5F, 0F);
	public static List<EntityWarrior> warriors = new List<EntityWarrior>();

	Ray ray;
	RaycastHit hit;

	public static Material redMatoutline;
	public static Material redMat;

	void Start () {
		redMatoutline = Resources.Load("redMaterialoutline", typeof(Material)) as Material;
		redMat = Resources.Load("redMaterial", typeof(Material)) as Material;

		for (int i = 0; i < 10; i++) {
			EntityWarriorRED ew = new EntityWarriorRED ();
			warriors.Add (ew);
		}
	}

	void Update () {
		float moveVertical = Input.GetAxis("Vertical") * 4 * Time.deltaTime;
		float moveVertical2 = Input.GetAxis("Vertical2") * 3 * Time.deltaTime;
		float moveHorizontal = Input.GetAxis("Horizontal") * 3 * Time.deltaTime;
		
		transform.Translate(new Vector3(moveHorizontal, moveVertical2, moveVertical));

		foreach(EntityWarrior ew in warriors){
			ew.update();
		}

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			if(hit.collider.name == "EntityWarriorRED"){
				//hit.collider.renderer.material.SetColor("_OutlineColor", Color.cyan);
				hit.collider.renderer.material = redMatoutline;
				if(Input.GetMouseButtonDown(0)){
					EntityID e = (EntityID)hit.collider.GetComponent(typeof(EntityID));
					int tempid = e.getID();

					foreach(EntityWarrior ew in warriors){
						if(ew.getEntityID().getID() == tempid){
							// found
							ew.jump();
						}
					}
				}
			}else{
				foreach(EntityWarrior ew in warriors){
					ew.getCube().transform.renderer.material = redMat;
				}
			}
			//print (hit.collider.name);
		}
	}
}
