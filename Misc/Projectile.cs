using UnityEngine;
using System.Collections;

public class Projectile : RotatableObject
{
    // Constants
    public float velocity;
    public SplashEffect impactEffect;
    public AudioClip impactSound;

    // Public variables
    public Weapon weapon { get; set; }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isCanMove)
        {
            transform.position = transform.position + (Vector3)direction * Time.deltaTime * velocity;

            Vector2 pos = (Vector2)transform.position;
            Vector2 bottomLeft = world.bottomLeft;
            Vector2 topRight = world.topRight;

            if ((pos.x < bottomLeft.x) || (pos.x > topRight.x) || (pos.y < bottomLeft.y) || (pos.y > topRight.y))
            {
                Destroy(gameObject);
                return;
            }

            base.Update();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        bool proceed = false;

        if (coll.gameObject.tag == "Alien") proceed = true;
        if (coll.gameObject.name == "Tank") proceed = true;

        if (proceed)
        {
            if (impactEffect != null)
            {
                SplashEffect impact = Instantiate<SplashEffect>(impactEffect);
                impact.gameObject.SetActive(true);
                impact.transform.position = transform.position;
            }

            world.audioSrc.PlayOneShot(impactSound);
            Destroy(gameObject);
        }
    }
}
