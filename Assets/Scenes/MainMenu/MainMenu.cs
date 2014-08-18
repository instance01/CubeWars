using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GUISkin skin;
    public Texture texture;

	void OnGUI () {

        GUI.skin = skin;

        GUI.DrawTexture(new Rect(Screen.width / 2 - 150, 10, 300, 50), texture, ScaleMode.StretchToFill, true);

        if (GUI.Button(new Rect(Screen.width / 2 - 100, 80, 200, 30), "Free Play"))
        {
			Application.LoadLevel (1);
		}

        if (GUI.Button(new Rect(Screen.width / 2 - 100, 120, 200, 30), "Campaign"))
        {
			Application.LoadLevel (2);
		}

        if (GUI.Button(new Rect(Screen.width / 2 - 100, 160, 200, 30), "Multiplayer"))
        {
			//Application.LoadLevel (3);
		}
	}
}
