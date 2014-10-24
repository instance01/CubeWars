using UnityEngine;
using System.Collections;

public class EntityCollider : MonoBehaviour
{

    Entity e;

    public void init(Entity e)
    {
        this.e = e;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnCollisionEnter(Collision c)
    {
        if (e != null)
        {
            e.checkCollide(c);
        }
    }
}
