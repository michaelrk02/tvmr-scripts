using UnityEngine;
using System.Collections;

public class Pickup : WorldObject
{
    // Constants
    public AudioClip pickupSound;

    protected override void Start()
    {
        base.Start();

        world.audioSrc.PlayOneShot(world.pickupSpawnSound);

        Invoke("Disappear", world.settings.pickupTimeout);
    }

    void Disappear()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            TakeAction();
        }
    }

    protected virtual void TakeAction()
    {
        world.audioSrc.PlayOneShot(pickupSound);
        Disappear();
    }
}
