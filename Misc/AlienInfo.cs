using UnityEngine;
using System.Collections;

public class AlienInfo : MonoBehaviour
{
    // Public variables
    public Alien parent { get; set; }

    // Use this for initialization
    void Start()
    {
        parent = GetComponentInParent<Alien>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.transform.position + new Vector3(0.0f, -0.5f, 0.0f);

        transform.eulerAngles = Vector3.zero;

        float scale = (parent.health / parent.maxHealth) * 100.0f;
        transform.localScale = new Vector3(scale, 1.0f, 1.0f);
    }
}
