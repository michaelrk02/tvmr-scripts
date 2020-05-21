using UnityEngine;
using System.Collections;

public class ParticleEffect : Effect
{
    // Constants
    public float lifetime;
    public float velocity;

    // Public variables
    public Vector2 dir { get; set; }

    protected override void Start()
    {
        // Some constants will be modified
        lifetime = Random.Range(lifetime / 2.0f, lifetime);

        float angle = Random.Range(0.0f, 360.0f);
        float x = Mathf.Sin(angle);
        float y = Mathf.Cos(angle);
        dir = new Vector2(x, y);

        float scale = Random.Range(0.1f, 1.0f);
        transform.localScale = new Vector3(scale, scale, 1.0f);

        Invoke("Disappear", lifetime);

        base.Start();
    }

    protected override void Update()
    {
        transform.position = transform.position + (Vector3)dir * Time.deltaTime * velocity;

        base.Update();
    }
}
