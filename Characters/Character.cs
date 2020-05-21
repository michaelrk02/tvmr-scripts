using UnityEngine;
using System.Collections;

public class Character : RotatableObject
{
    // Public variables
    public Weapon[] weapons { get { return _weapons; } }

    // Private variables
    private Weapon[] _weapons;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        _weapons = GetComponentsInChildren<Weapon>();
    }
}
