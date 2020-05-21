using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour
{
    protected World world;

    // Use this for initialization
    protected virtual void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
}
