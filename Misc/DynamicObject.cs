using UnityEngine;
using System.Collections;

public class DynamicObject : WorldObject
{
    // Public variables
    public bool isCanMove { get; set; }

    protected override void Start()
    {
        isCanMove = true;

        base.Start();
    }
}
