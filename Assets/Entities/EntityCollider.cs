using UnityEngine;
using System.Collections;

public class EntityCollider : MonoBehaviour {

	EntityWarrior ew;

	public void init (Entity e){
		if (e is EntityWarrior) {
			ew = (EntityWarrior)e;
		}
	}

	void Start () {
	
	}

	void Update () {
	
	}

	public void OnCollisionEnter(Collision c){
		if (ew != null) {
			ew.checkCollide(c);
		}
	}
}
