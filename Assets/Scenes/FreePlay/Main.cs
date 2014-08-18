using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.GamePlay;
using Assets.Blocks;

public class Main : MonoBehaviour {

	public static List<string> words = new List<string>(new string[] { "Hi", "What's up fella!", "Dudee", "Dudeeee", "I'm bored.", "?", "!", "Mhm", "Hmmm", "I hate you", "I love you", "Nope Nope Nope", "Gawd", "Ugh", "yaaaaay", "mkay", "sup", "rude", "Not cool man", "so", "Fu" });

    CameraHandler camhandler;
    MouseClickSceneHandler mouseclickhandler;

	public Vector3 spawnLocation = new Vector3(0F, 2.5F, 0F);
	public List<EntityWarrior> warriors = new List<EntityWarrior>();
	public int redwarriors = 0;
	public int bluewarriors = 0;

    public List<Block> blocks = new List<Block>();

    public GameObject cursorcone;
    bool conedown = true;

    static Main main;

	void Start () {
        main = this;
        Materials.init();

        camhandler = new CameraHandler(transform.gameObject);
        mouseclickhandler = new MouseClickSceneHandler();

        cursorcone = GameObject.Find("cone");
        cursorcone.transform.renderer.enabled = false;

		for (int i = 0; i < 10; i++) {
			EntityWarriorRED ew = new EntityWarriorRED (new VectorHelper(Main.getMain().spawnLocation).add(Random.Range(20, 30), 0, Random.Range (20, 30)));
			warriors.Add (ew);
			redwarriors++;
		}

		for (int i = 0; i < 10; i++) {
            EntityWarriorBLUE ew = new EntityWarriorBLUE(new VectorHelper(Main.getMain().spawnLocation).add(Random.Range(10, 20), 0, Random.Range(10, 20)));
			warriors.Add (ew);
			bluewarriors++;
		}

        BlockSpawner spawner = new BlockSpawner(10, 0, 14, "BLUE");
        BlockSpawner spawner2 = new BlockSpawner(28, 0, 24, "RED");
        blocks.Add(spawner);
        blocks.Add(spawner2);
	}


	void Update () {
        camhandler.update();

        // animate cursorcone
        if (!conedown)
        {
            cursorcone.transform.Translate(new Vector3(0F, 0F, -0.03F));
            if (cursorcone.transform.position.y > 1.1)
            {
                conedown = true;
            }
        }
        else if(conedown)
        {
            cursorcone.transform.Translate(new Vector3(0F, 0F, 0.03F));
            if (cursorcone.transform.position.y < 0.7)
            {
                conedown = false;
            }
        }
        
        
        // update all warriors
		foreach(EntityWarrior ew in warriors){
            // TODO this function lags very hard, fix
			ew.update();
		}

        mouseclickhandler.update(warriors);
        foreach(Block b in blocks){
            b.update();
        }

        if (Input.GetKeyDown (KeyCode.Escape)){
            Application.LoadLevel(0);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            IngameGUI.setOpen(!IngameGUI.isOpen());
        }
	}

	void OnGUI() {
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.UpperCenter;
		string re = redwarriors.ToString ();
		string be = bluewarriors.ToString ();
		style.normal.textColor = Color.red;
		GUI.Label (new Rect (Screen.width / 2 - 40,5,10,20), "R");
		style.normal.textColor = Color.white;
		GUI.Label (new Rect (Screen.width / 2 - 30,5,60,20), re + " : " + be);
		style.normal.textColor = Color.blue;
		GUI.Label (new Rect (Screen.width / 2 + 30,5,10,20), "B");
	}

	public void updateWarriorCounts(int r, int b){
		redwarriors = r;
		bluewarriors = b;
	}

    public void addWarriorCount(bool r, bool b)
    {
        if (r)
        {
            redwarriors += 1;
        }
        else if (b)
        {
            bluewarriors += 1;
        }
    }

    public static Main getMain()
    {
        return main;
    }

}
