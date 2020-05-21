using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Constants
    public KeyCode moveForward, moveBackward, turnLeft, turnRight;
    public KeyCode zoomForward, zoomBackward, lookLeft, lookRight;
    public KeyCode unzoom;
    public KeyCode fireTank, fireMachineGun;

    // Public variables
    public Player player { get; set; }

    // Private variables
    private World world;

    // Use this for initialization
    void Start()
    {
        player = GetComponentInParent<Player>();

        world = GameObject.Find("World").GetComponent<World>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!world.paused)
        {
            if (Input.GetKey(moveForward)) player.Move(Vector2.up);
            if (Input.GetKey(moveBackward)) player.Move(Vector2.down);
            if (Input.GetKey(turnLeft)) player.Turn(Vector2.left);
            if (Input.GetKey(turnRight)) player.Turn(Vector2.right);
            if (Input.GetKey(zoomForward)) player.Zoom(Vector2.up);
            if (Input.GetKey(zoomBackward)) player.Zoom(Vector2.down);
            if (Input.GetKey(lookLeft)) player.Look(Vector2.left);
            if (Input.GetKey(lookRight)) player.Look(Vector2.right);
            if (Input.GetKey(unzoom)) player.Unzoom();
            if (Input.GetKey(fireTank)) player.FireWeapon(0);
            if (Input.GetKey(fireMachineGun)) player.FireWeapon(1);
        }
    }
}
