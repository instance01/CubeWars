using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.GamePlay;

// A warrior moves around very aggressively and fast and is a short ranged entity

public class EntityWarrior : Entity {

	BoxCollider cubeCollider;
	public static float speed = 0.9F;
	private string classifier = "RED";
	private string enemyClassifier = "";
	EntityID entityid;

	bool said = false;
	int said_c = 0;
    bool shootcooldown = false;
    int shootcooldown_c = 0;
	private GameObject txt;
	private TextMesh txtMesh;


	Entity target;


	public EntityWarrior(string c){
		this.classifier = c;
		if (classifier == "RED")
		{
			enemyClassifier = "BLUE";
		}
		else if (classifier == "BLUE")
		{
			enemyClassifier = "RED";
		}
		createGraphics ();
	}

	public void createGraphics(){

		// creates the cube
		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.name = "EntityWarrior" + classifier;
        cube.tag = "EntityWarrior" + classifier;
		if (classifier == "RED") {
			cube.transform.position = new VectorHelper(Main.spawnLocation).add(Random.Range(20, 30), 0, Random.Range (20, 30));
		} else if (classifier == "BLUE") {
			cube.transform.position = new VectorHelper(Main.spawnLocation).add(Random.Range(10, 20), 0, Random.Range (10, 20));
		}
		cube.transform.localScale = new Vector3 (0.5F, 0.5F, 0.5F);
		if (classifier == "RED") {
			cube.renderer.material = Materials.redMat;
		} else if (classifier == "BLUE") {
            cube.renderer.material = Materials.blueMat;
		}

		// adds components to cube
		cube.AddComponent<Rigidbody>(); 
		cubeCollider = cube.AddComponent<BoxCollider>();
		cubeCollider.size = new Vector3 (1.25F, 1.25F, 1.25F);
		entityid = cube.AddComponent<EntityID> ();
		cube.AddComponent<EntityCollider> ().init (this);
		//Debug.Log (entityid.getID ());

		// testing stuff
		//Rigidbody.Instantiate (cubeRigid, Main.spawnLocation, Quaternion.identity);
		//GameObject.Instantiate(cube, Main.spawnLocation, Quaternion.identity);


		txt = new GameObject();
		
		txtMesh = (TextMesh)txt.AddComponent(typeof(TextMesh));
		txtMesh.font = (Font)Resources.Load("pf_arma_five", typeof(Font));
		txt.transform.renderer.material = (Material)Resources.Load("pf_arma_five", typeof(Material));
        txt.transform.Rotate(67.5F, 0F, 0F);
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


        base.createHealthbarGraphics();

	}

	public void saySomething(string msg){
		txt.transform.localPosition = cube.transform.localPosition;
		txt.transform.Translate (0, 1, 0);
		txtMesh.text = msg;
		MeshRenderer meshrenderer = (MeshRenderer)txt.GetComponent (typeof(MeshRenderer));
		meshrenderer.enabled = true;
		//ctxt = (GameObject) GameObject.Instantiate (txt);
	}


	public void attack(EntityWarrior ew){
		if (ew.getCube () == null) {
			target = null;
			return;
		}
		GameObject attackcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		attackcube.name = "EntityWarrior" + classifier + "_BULLET";
		attackcube.transform.localScale = new Vector3 (0.1F, 0.1F, 0.1F);
		if (classifier == "RED") {
            attackcube.renderer.material = Materials.redMat;
		} else if (classifier == "BLUE") {
            attackcube.renderer.material = Materials.blueMat;
		}
		attackcube.transform.localRotation = cube.transform.localRotation;
		attackcube.transform.localPosition = new VectorHelper(cube.transform.localPosition).add(0, 1, 0);

		Rigidbody rigid = (Rigidbody)attackcube.AddComponent (typeof(Rigidbody));
		rigid.velocity = new VectorHelper((ew.getCube().transform.position - attackcube.transform.position).normalized * speed * 15).add(0F, 2.5F, 0F);
		//rigid.velocity.Set (rigid.velocity.x, 25, rigid.velocity.z);

        GameObject.Destroy(attackcube, 4);

        if (Vector3.Distance(getCube().transform.position, ew.getCube().transform.position) > 15)
        {
            this.target = null;
        }
	}

    public void findTarget()
    {
        var currentPos = getCube().transform.position;

        GameObject[] gos = GameObject.FindGameObjectsWithTag("EntityWarrior" + enemyClassifier);
        if (gos.Length > 0)
        {
            GameObject closestGameObject = gos
                .Select(go => new { go = go, position = go.transform.position })
                    .Aggregate((current, next) =>
                               (current.position - currentPos).sqrMagnitude <
                               (next.position - currentPos).sqrMagnitude
                               ? current : next).go;

            if (Vector3.Distance(getCube().transform.position, closestGameObject.transform.position) < 10)
            {
                EntityID e = (EntityID)closestGameObject.GetComponent(typeof(EntityID));
                int tempid = e.getID();

                foreach (EntityWarrior ew in Main.warriors)
                {
                    if (ew.getEntityID().getID() == tempid)
                    {
                        target = ew;
                    }
                }
            }
        }
    }

	public void move(){
		// will update text coords
		if (said) {
			txt.transform.localPosition = cube.transform.localPosition;
			txt.transform.Translate (0, 1, 0);
		}

        if (!move_)
        {
            moveTo(moveto, 1.5F);
        }

		// will just randomly move around if no target found
		if (getTarget () == null) {
			if(move_){
				cube.transform.Rotate(0, Random.Range(-5, 5), 0); // they move to the left more often because we never reach 5 here (just -5 to 4).
				cube.transform.Translate(Vector3.forward * Time.deltaTime * speed);
				
				if (Random.Range (0, 1000) < 1) {
					// will jump randomly
					// TODO jump when obstacle in front of entity
					jump (150);
				}
            }

			// try to find a new target
            findTarget();
		}else{
            if (shootcooldown)
            {
                shootcooldown_c++;
                if (shootcooldown_c > 100)
                {
                    shootcooldown = false;
                    shootcooldown_c = 0;
                }
                return;
            }
			if(target is EntityWarriorRED){
				EntityWarriorRED ewr = (EntityWarriorRED) target;
                shootcooldown = true;
				attack (ewr);
			}else if(target is EntityWarriorBLUE){
				EntityWarriorBLUE ewb = (EntityWarriorBLUE) target;
                shootcooldown = true;
				attack (ewb);
			}
		}
	}

	public Entity getTarget(){
		return target;
	}

	public void update(){
		if (dead) {
			return;
		}
		updateHealthbar ();
		move ();
		if (!said) {
			if (Random.Range (0, 1500) < 1) {
				saySomething (Main.words[Random.Range(0, Main.words.Count)]);
				said = true;
			}
		} else {
			said_c ++;
			if(said_c > 200){
				said_c = 0;
				said = false;
				txt.renderer.enabled = false;
			}
		}
	}

	public void checkCollide(Collision c){
		if (c.gameObject.name == ("EntityWarrior" + enemyClassifier + "_BULLET")) {
			this.health -= 1;
			if (this.health < 1){
				// dead
				dead = true;
				Main.warriors.Remove(this);
				said = false;
				txt.renderer.enabled = false;
				GameObject.Destroy(health_green);
				GameObject.Destroy(health_red);
                Vector3 cubepos = cube.transform.localPosition;
				GameObject.Destroy(cube, 0);

                int[,] pos = new int[16, 2] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 0, 3 }, { 1, 0 }, { 1, 1 }, { 1, 2 }, { 1, 3 }, { 2, 0 }, { 2, 1 }, { 2, 2 }, { 2, 3 }, { 3, 0 }, { 3, 1 }, { 3, 2 }, { 3, 3 } };

                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        GameObject cube_ = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube_.renderer.material.color = Color.cyan;
                        cube_.transform.localScale = (new Vector3(0.0625F * 2, 0.0625F * 2, 0.0625F * 2));
                        //cube_.transform.localPosition = (cubepos);
                        cube_.transform.localPosition = (new VectorHelper(cubepos).add(pos[i, 0] * 0.0625F * 2, j * 0.0625F * 2,pos[i, 1] * 0.0625F * 2));

                        cube_.AddComponent<Rigidbody>();
                        cube_.rigidbody.AddExplosionForce((Random.Range(300.0f, 700.0f)), new Vector3(cubepos.x * 1.01F, cubepos.y * 0.9F, cubepos.z * 1.01F), (Random.Range(100.0f, 200.0f)));
                        cube_.rigidbody.SetDensity(cube_.transform.localPosition.x);

                        GameObject.Destroy(cube_, 2);
                    }
                    
                }
				if(classifier == "RED"){
					Main.updateWarriorCounts(Main.redwarriors - 1, Main.bluewarriors);
				}else{
					Main.updateWarriorCounts(Main.redwarriors, Main.bluewarriors - 1);
				}
                return;
			}
		}
	}

	public string getClassifier(){
		return classifier;
	}

	public void updateMaterial(bool jump){
		if (classifier == "RED") {
			if(!jump){
                getCube().transform.renderer.material = Materials.redMat;
			}else{
                getCube().transform.renderer.material = Materials.redMatoutline;
			}
		} else if (classifier == "BLUE") {
			if(!jump){
                getCube().transform.renderer.material = Materials.blueMat;
			}else{
                getCube().transform.renderer.material = Materials.blueMatoutline;
			}
		}
	}

	public EntityID getEntityID(){
		return entityid;
	}

	public void jump(){
		cube.rigidbody.AddForce (0, 100, 0);
	}

	public void jump(int force){
		cube.rigidbody.AddForce (0, force, 0);
	}
}
