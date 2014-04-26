using UnityEngine;
using System.Collections;

// A warrior moves around very aggressively and fast and is a short ranged entity

public class EntityWarrior : Entity {

	GameObject cube;
	public static float speed = 0.9F;
	public string classifier = "RED";
	EntityID entityid;

	public EntityWarrior(string c){
		this.classifier = c;
		createGraphics ();
	}

	public void createGraphics(){

		// creates the cube
		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.name = "EntityWarrior" + classifier;
		cube.transform.position = new VectorHelper(Main.spawnLocation).add(Random.Range(10, 20), 0, Random.Range (10, 20));
		cube.transform.localScale = new Vector3 (0.5F, 0.5F, 0.5F);
		cube.renderer.material = Main.redMat;	

		// adds components to cube
		cube.AddComponent<Rigidbody>(); 
		BoxCollider cubeCollider = cube.AddComponent<BoxCollider>();
		cubeCollider.size = new Vector3 (1.25F, 1.25F, 1.25F);
		entityid = cube.AddComponent<EntityID> ();
		//Debug.Log (entityid.getID ());

		// testing stuff
		//Rigidbody.Instantiate (cubeRigid, Main.spawnLocation, Quaternion.identity);
		//GameObject.Instantiate(cube, Main.spawnLocation, Quaternion.identity);

	}

	public void saySomething(string msg){

	}

	public void move(){
		// will just randomly move around if no target found
		if (getTarget () == null) {
			cube.transform.Rotate(0, Random.Range(-5, 5), 0);
			cube.transform.Translate(Vector3.forward * Time.deltaTime * speed);
		}
	}

	public Entity getTarget(){
		return null;
	}

	public void update(){
		move ();
	}

	public GameObject getCube(){
		return cube;
	}

	public EntityID getEntityID(){
		return entityid;
	}

	public void jump(){
		cube.rigidbody.AddForce (0, 100, 0);
	}
}
