using UnityEngine;
using System.Collections;

public class RotatableObject : DynamicObject
{
    // Private variables
    private float _angle;

    // Update is called once per frame
    protected override void Update()
    {
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, -angle);

        base.Update();
    }

    public float angle
    {
        get
        {
            return _angle;
        }
        set
        {
            _angle = value % 360.0f;
        }
    }

    public Vector2 direction
    {
        get
        {
            Vector2 vec = Vector2.zero;
            vec.x = Mathf.Sin(angle * Mathf.Deg2Rad);
            vec.y = Mathf.Cos(angle * Mathf.Deg2Rad);
            return vec;
        }
    }
}
