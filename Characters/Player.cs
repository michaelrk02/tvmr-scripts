using UnityEngine;
using System.Collections;

public class Player : Character
{
    // Constants
    public float velocity;
    public float maxHealth;
    public float maxShield;
    public SplashEffect explodeEffect;
    public ParticleEffect[] particleEffects;
    public Vector2 particleOffset;
    public ParticleEffect[] shieldBrokenParticles;
    public AudioClip levelUpSound;
    public AudioClip hitSound;
    public AudioClip shieldBrokenSound;
    public AudioClip explodeSound;
    public AudioClip deathSound;
    public Quaternion rotationQuaternion;
    
    // Public variables
    public TankBarrel barrel { get; set; }
    public float health { get; set; }
    public float shield { get; set; }
    public int score { get; set; }
    public int level { get; set; }
    public float experience { get; set; }
    public float experienceNeeded { get; set; }
    public AudioSource musicPlayer { get; set; }

    // Private variables
    private float particleElapsed;
    private HUD hud;

    protected override void Start()
    {
        base.Start();

        barrel = GetComponentInChildren<TankBarrel>();
        level = 1;
        CalcXpNeeded();
        musicPlayer = GetComponent<AudioSource>();

        hud = GameObject.Find("HUD").GetComponent<HUD>();
    }

    protected override void Update()
    {
        rotationQuaternion = transform.rotation;

        if (experience >= experienceNeeded)
        {
            // LEVEL UP!
            level++;
            score += (int)(world.settings.scoreIncreaseFactor * 100.0f);
            experience -= experienceNeeded;
            CalcXpNeeded();
            world.audioSrc.PlayOneShot(levelUpSound);

            hud.DisplayMessage(MessagePlacement.Center, "LEVEL UP!", 3.0f);
            hud.DisplayMessage(MessagePlacement.BottomLeft, "Monsters are getting stronger", 5.0f);

            foreach (Alien alien in world.genericAliens)
            {
                alien.maxHealth += 0.05f * alien.maxHealth;
                alien.velocity += 0.0005f * alien.velocity;
                alien.impactDamage += 0.05f * alien.impactDamage;

                Weapon[] weapons = alien.GetComponentsInChildren<Weapon>();

                if (weapons != null)
                {
                    foreach (Weapon wpn in weapons)
                    {
                        wpn.damage += 0.05f * wpn.damage;
                    }
                }
            }
        }

        if (health > maxHealth) health = maxHealth;
        if (shield > maxShield) shield = maxShield;

        if (health == 0.0f)
        {
            SplashEffect exp = Instantiate<SplashEffect>(explodeEffect);
            exp.gameObject.SetActive(true);
            exp.transform.position = transform.position;

            world.audioSrc.PlayOneShot(explodeSound);
            world.audioSrc.PlayOneShot(deathSound);
            world.GameOver();
            Destroy(gameObject);
        }

        if (health <= 50.0f)
        {
            if (Time.time >= particleElapsed + 0.1f)
            {
                int particleIndex = Random.Range(0, particleEffects.Length);
                ParticleEffect part = Instantiate<ParticleEffect>(particleEffects[particleIndex]);
                part.gameObject.SetActive(true);
                part.transform.position = transform.position + (Vector3)particleOffset;
                particleElapsed = Time.time;
            }
        }

        base.Update();
    }

    void CalcXpNeeded()
    {
        experienceNeeded = 100.0f + 20.0f * (float)level;
    }

    public void SetInfo(float _maxHealth, float _maxShield)
    {
        maxHealth = _maxHealth;
        maxShield = _maxShield;
        health = maxHealth;
        shield = maxShield;
    }

    bool IsCanMove(Vector2 offset)
    {
        Vector2 next = (Vector2)transform.position + offset;
        Vector2 bounds = GetComponent<BoxCollider2D>().bounds.extents;
        Vector2 bottomLeft = world.bottomLeft + bounds;
        Vector2 topRight = world.topRight - bounds;

        return ((next.x > bottomLeft.x) && (next.x < topRight.x) && (next.y > bottomLeft.y) && (next.y < topRight.y));
    }

    public void Move(Vector2 dir)
    {
        dir.Normalize();
        Vector2 normal = dir.y * direction;

        Vector2 offset = normal * Time.deltaTime * velocity * world.settings.playerSpeedFactor;
        if (IsCanMove(offset))
        {
            transform.position += (Vector3)offset;
        }
    }

    public void Turn(Vector2 dir)
    {
        dir.Normalize();
        angle += + dir.x * Time.deltaTime * velocity * world.settings.playerSpeedFactor * 45.0f;
    }

    public void Zoom(Vector2 dir)
    {
        barrel.pointerDistance += dir.y * Time.deltaTime * velocity * world.settings.playerSpeedFactor * 2.0f;
    }

    public void Look(Vector2 dir)
    {
        dir.Normalize();
        barrel.angle += dir.x * Time.deltaTime * velocity * world.settings.playerSpeedFactor * 45.0f;
    }

    public void Unzoom()
    {
        barrel.pointerDistance = 0.0f;
    }

    public void FireWeapon(int wpnID)
    {
        weapons[wpnID].Fire();
    }

    public void TakeDamage(float damage)
    {
        if (shield > 0.0f)
        {
            shield -= damage;
            if (shield <= 0.0f)
            {
                world.audioSrc.PlayOneShot(shieldBrokenSound);
                foreach (ParticleEffect effect in shieldBrokenParticles)
                {
                    ParticleEffect part = Instantiate<ParticleEffect>(effect);
                    part.gameObject.SetActive(true);
                    part.transform.position = transform.position;
                }
                hud.DisplayMessage(MessagePlacement.BottomLeft, "Your shield has been broken. The tank is now vulnerable", 5.0f);
            }
        }
        else
        {
            health -= damage;
        }
        shield = shield < 0.0f ? 0.0f : shield;
        health = health < 0.0f ? 0.0f : health;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Alien")
        {
            TakeDamage(coll.gameObject.GetComponent<Alien>().impactDamage * world.settings.alienDamageFactor);

            world.audioSrc.PlayOneShot(hitSound);
        }
        if (coll.gameObject.tag == "AlienProjectile")
        {
            TakeDamage(coll.gameObject.GetComponent<Projectile>().weapon.damage * world.settings.alienDamageFactor);

            world.audioSrc.PlayOneShot(hitSound);
        }
    }
}
