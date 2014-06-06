using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GUISkin skin;

	void OnGUI () {

        GUI.skin = skin;

        if (GUI.Button(new Rect(Screen.width / 2 - 100, 40, 200, 30), "Free Play"))
        {
			Application.LoadLevel (1);
		}

        if (GUI.Button(new Rect(Screen.width / 2 - 100, 80, 200, 30), "Campaign"))
        {
			//Application.LoadLevel (2);
		}

        if (GUI.Button(new Rect(Screen.width / 2 - 100, 120, 200, 30), "Multiplayer"))
        {
			//Application.LoadLevel (3);
		}
	}
}
