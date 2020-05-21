using UnityEngine;
using System.Collections;

public class Effect : WorldObject
{
    public void Disappear()
    {
        Destroy(gameObject);
    }
}
