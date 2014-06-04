using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	public static List<string> words = new List<string>(new string[] { "Hi", "What's up fellow!", "Dudee", "Dudeeee", "I'm bored.", "?", "!", "Mhm", "Hmmm", "I hate you", "I love you", "Nope Nope Nope", "Gawd", "Ugh", "yaaaaay", "mkay", "sup" });

	public static Vector3 spawnLocation = new Vector3(0F, 2.5F, 0F);
	public static List<EntityWarrior> warriors = new List<EntityWarrior>();
	public static int redwarriors = 0;
	public static int bluewarriors = 0;

	Ray ray;
	RaycastHit hit;

	public static Material redMatoutline;
	public static Material redMat;
	public static Material blueMatoutline;
	public static Material blueMat;

	Entity selected;

	void Start () {
		redMatoutline = Resources.Load("redMaterialoutline", typeof(Material)) as Material;
		redMat = Resources.Load("redMaterial", typeof(Material)) as Material;
		blueMatoutline = Resources.Load("blueMaterialoutline", typeof(Material)) as Material;
		blueMat = Resources.Load("blueMaterial", typeof(Material)) as Material;

		for (int i = 0; i < 10; i++) {
			EntityWarriorRED ew = new EntityWarriorRED ();
			warriors.Add (ew);
			redwarriors++;
		}

		for (int i = 0; i < 10; i++) {
			EntityWarriorBLUE ew = new EntityWarriorBLUE ();
			warriors.Add (ew);
			bluewarriors++;
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
				hit.collider.renderer.material = redMatoutline;
				if(Input.GetMouseButtonDown(0)){
					EntityID e = (EntityID)hit.collider.GetComponent(typeof(EntityID));
					int tempid = e.getID();
                    
					foreach(EntityWarrior ew in warriors){
						if(ew.getEntityID().getID() == tempid){
							// found
							ew.jump();
							selected = ew;
						}
					}
				}
			}else if(hit.collider.name == "EntityWarriorBLUE"){
				hit.collider.renderer.material = blueMatoutline;
				if(Input.GetMouseButtonDown(0)){
					EntityID e = (EntityID)hit.collider.GetComponent(typeof(EntityID));
					int tempid = e.getID();
					
					foreach(EntityWarrior ew in warriors){
						if(ew.getEntityID().getID() == tempid){
							// found
							ew.jump();
							selected = ew;
						}
					}
				}
			}else{
				foreach(EntityWarrior ew in warriors){
					ew.updateMaterial(false);
				}

				if(selected != null){
					EntityWarrior ew_ = (EntityWarrior) selected;
					ew_.setMove(false);
					ew_.moveTo(hit.point, 1.5F);
				}
			}
			//print (hit.collider.name);
		}
	}

	void OnGUI() {
		string re = redwarriors.ToString ();
		string be = bluewarriors.ToString ();
		GUI.Label (new Rect (Screen.width / 2 - 30,5,60,20), "R " + re + " : " + be + " B");
	}

	public static void updateWarriorCounts(int r, int b){
		redwarriors = r;
		bluewarriors = b;
	}

}
