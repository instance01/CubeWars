using UnityEngine;
using System.Collections;

public class levelsMenu : MonoBehaviour {

    public GUISkin skin;

	void OnGUI () {

        GUI.skin = skin;

        if (GUI.Button(new Rect(Screen.width / 2 - 400, 100, 100, 100), "1\nInit"))
        {
			
		}

        GUI.enabled = false;
        if (GUI.Button(new Rect(Screen.width / 2 - 300, 100, 100, 100), "2\nFirst grief"))
        {
			
		}

        if (GUI.Button(new Rect(Screen.width / 2 - 200, 100, 100, 100), "3\nOver-\nwhelming"))
        {
			
		}
        GUI.enabled = true;
	}
}
