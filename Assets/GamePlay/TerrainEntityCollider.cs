using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainEntityCollider : MonoBehaviour
{
    public void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.name.EndsWith("_BULLET"))
        {
            var children = new List<GameObject>();
            foreach (Transform child in c.gameObject.transform)
            {
                children.Add(child.gameObject);
            }
            foreach (GameObject child in children)
            {
                Destroy(child);
            }
        }
    }
}
