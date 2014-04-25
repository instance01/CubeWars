using UnityEngine;
using System.Collections;

public class VectorHelper {

	Vector3 t;

	public VectorHelper(Vector3 temp){
		t = temp;
	}

	public Vector3 add(float x, float y, float z){
		return new Vector3 (t.x + x, t.y + y, t.z + z);
	}
}
