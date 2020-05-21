using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    // Constants
    public float damage;
    public AudioClip fireSound;
    public float fireInterval;
    public RotatableObject[] origins;
    public float fireOffset;
    public ParticleEffect[] particles;
    // Generic objects
    public Projectile genericProjectile;

    // Public variables
    public Character owner { get { return _owner; } }
    public bool canFire { get; set; }

    // Private variables
    private Character _owner;
    private AudioSource audioSrc;

    // Use this for initialization
    void Start()
    {
        canFire = true;

        _owner = GetComponentInParent<Character>();
        audioSrc = GameObject.Find("World").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Open()
    {
        canFire = true;
    }

    public void Fire()
    {
        if (canFire)
        {
            audioSrc.PlayOneShot(fireSound);

            float angle = GetComponentInParent<RotatableObject>().angle;
            for (int i = 0; i < origins.Length; i++)
            {
                angle = angle + origins[i].angle;
            }

            Projectile proj = Instantiate(genericProjectile);
            proj.gameObject.SetActive(true);
            proj.angle = angle;
            proj.weapon = this;
            proj.transform.position = (Vector2)owner.transform.position + proj.direction * fireOffset;
            canFire = false;

            foreach (ParticleEffect particle in particles)
            {
                ParticleEffect part = Instantiate<ParticleEffect>(particle);
                part.gameObject.SetActive(true);
                part.transform.position = (Vector2)owner.transform.position + proj.direction * fireOffset;
            }

            Invoke("Open", fireInterval);
        }
    }
}
