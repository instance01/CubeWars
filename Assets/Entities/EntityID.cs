using UnityEngine;
using System.Collections;

public class EntityID : MonoBehaviour {

	int id = 0;
	bool initialized = false;

	void Start () {
		if (!initialized) {
			id = Random.Range (0, int.MaxValue - 1);
		}
	}

	public int getID(){
		if (id == 0) {
			id = Random.Range (0, int.MaxValue - 1);
			initialized = true;
		}
		return id;
	}

}
