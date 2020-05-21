using UnityEngine;
using System.Collections;

public class TankBarrel : RotatableObject
{
    // Public variables
    public Player tank { get; set; }
    public float pointerDistance { get; set; }
    public GameObject pointer { get; set; }

    protected override void Start()
    {
        base.Start();

        tank = GetComponentInParent<Player>();
        pointerDistance = 0.0f;
        pointer = GameObject.Find("Pointer");
    }

    protected override void Update()
    {
        if (pointerDistance < 0.0f)
            pointerDistance = 0.0f;

        pointer.transform.localPosition = new Vector3(0.0f, pointerDistance, 0.0f);

        base.Update();
    }
}
