using UnityEngine;
using System.Collections;

// A warrior moves around very aggressively and fast and is a short ranged entity

public class EntityWarrior : Entity {

	GameObject cube;
	public static float speed = 0.9F;
	public string classifier = "RED";
	EntityID entityid;

	bool said = false;
	int said_c = 0;
	private GameObject ctxt;
	private GameObject txt;
	private TextMesh txtMesh;

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


		txt = new GameObject();
		
		txtMesh = (TextMesh)txt.AddComponent(typeof(TextMesh));
		MeshFilter meshfilter = (MeshFilter)txt.AddComponent(typeof(MeshFilter));
		txtMesh.font = (Font)Resources.Load("pf_arma_five", typeof(Font));
		txt.transform.renderer.material = (Material)Resources.Load("pf_arma_five", typeof(Material));
		//MeshRenderer meshRenderer = (MeshRenderer)txt.AddComponent(typeof(MeshRenderer));
		/*Mesh mesh = txt.GetComponent<MeshFilter> ().mesh;
		meshfilter.mesh = mesh;
		mesh.RecalculateNormals ();*/
		txtMesh.text = ".";
		txtMesh.color = Color.black;
		txtMesh.richText = true;
		txtMesh.fontSize = 45;
		txt.transform.localScale = new Vector3 (0.2F, 0.2F, 0.2F);
		MeshRenderer meshrenderer = (MeshRenderer)txt.GetComponent (typeof(MeshRenderer));
		meshrenderer.enabled = false;
	}

	public void saySomething(string msg){
		txt.transform.localPosition = cube.transform.localPosition;
		txt.transform.Translate (0, 1, 0);
		txtMesh.text = msg;
		MeshRenderer meshrenderer = (MeshRenderer)txt.GetComponent (typeof(MeshRenderer));
		meshrenderer.enabled = true;
		//ctxt = (GameObject) GameObject.Instantiate (txt);
	}

	public void move(){
		// will just randomly move around if no target found
		if (getTarget () == null) {
			cube.transform.Rotate(0, Random.Range(-5, 5), 0); // they move to the left more often because we never reach 5 here (just -5 to 4).
			cube.transform.Translate(Vector3.forward * Time.deltaTime * speed);
		}
	}

	public Entity getTarget(){
		return null;
	}

	public void update(){
		move ();
		if (!said) {
			if (Random.Range (0, 1000) < 1) {
				saySomething (Main.words[Random.Range(0, Main.words.Count)]);
				said = true;
			}
		} else {
			said_c ++;
			if(said_c > 200){
				said_c = 0;
				said = false;
				txt.renderer.enabled = false;
				if(ctxt != null){
					GameObject.Destroy(ctxt);
				}
			}
		}
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
