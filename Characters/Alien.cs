using UnityEngine;
using System.Collections;

public class Alien : Character
{
    // Constants
    public float maxHealth;
    public float velocity;
    public bool isCanFire;
    public int scoreReward;
    public float impactDamage;
    public SplashEffect bloodEffect;
    public AudioClip deathSound;

    // Public variables
    public float health { get; set; }
    public Weapon poison { get; set; }

    // Private variables
    private Player player;
    private float fireElapsed;

    protected override void Start()
    {
        base.Start();

        // Some constants will be modified regarding the game settings
        maxHealth *= world.settings.alienHealthFactor;

        health = maxHealth;
        poison = GetComponentInChildren<Weapon>();

        player = GameObject.Find("Tank").GetComponent<Player>();
        fireElapsed = Time.time;
    }

    protected override void Update()
    {
        fireElapsed = world.paused ? fireElapsed + Time.deltaTime : fireElapsed;

        if (isCanMove)
        {
            Vector2 thisPos = (Vector2)transform.position;
            Vector2 playerPos = player.transform.position;
            angle = Mathf.Atan2(playerPos.x - thisPos.x, playerPos.y - thisPos.y) * Mathf.Rad2Deg;

            Vector2 dir = Vector2.zero;
            dir.x = Mathf.Sin(angle * Mathf.Deg2Rad);
            dir.y = Mathf.Cos(angle * Mathf.Deg2Rad);
            transform.position = transform.position + (Vector3)dir * velocity * Time.deltaTime;

            if (Time.time >= fireElapsed + world.settings.alienPoisonInterval)
            {
                if (poison) poison.Fire();
                fireElapsed = Time.time;
            }

            if (health < 0.0f)
            {
                int spawnPickup = Random.Range(0, 5);
                if (spawnPickup == 0)
                {
                    int pickupItem = Random.Range(0, world.genericPickups.Length);

                    Pickup pu = Instantiate<Pickup>(world.genericPickups[pickupItem]);
                    pu.gameObject.SetActive(true);
                    pu.transform.position = transform.position;
                }

                world.audioSrc.PlayOneShot(deathSound);
                Destroy(gameObject);
                player.score += player.level * (int)((float)scoreReward * world.settings.scoreIncreaseFactor);
                player.experience += 10.0f * world.settings.xpIncreaseFactor;
            }

            base.Update();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Projectile" || coll.gameObject.tag == "AlienProjectile")
        {
            Projectile proj = coll.gameObject.GetComponent<Projectile>();
            health -= proj.weapon.damage * world.settings.playerDamageFactor;
        }
        if (System.Object.ReferenceEquals(player.gameObject, coll.gameObject))
        {
            SplashEffect blood = Instantiate<SplashEffect>(bloodEffect);
            blood.gameObject.SetActive(true);
            blood.transform.position = transform.position;

            Destroy(gameObject);
        }
    }
}
