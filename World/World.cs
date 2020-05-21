using UnityEngine;
using System.Collections;

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
}

public class DifficultySelection
{
    public static Difficulty difficulty = Difficulty.Medium;
}

public class World : MonoBehaviour
{
    // Constants
    public GameSettings easySettings;
    public GameSettings mediumSettings;
    public GameSettings hardSettings;
    public float tipsInterval;
    public float tipsTimeout;
    public string[] tips;
    public AudioClip bossSpawnSound;
    public AudioClip pickupSpawnSound;
    // Generic objects
    public Alien[] genericAliens;
    public Pickup[] genericPickups;

    // Variables (public)
    public AudioSource audioSrc { get; set; }
    public Vector2 bottomLeft { get; set; }
    public Vector2 topRight { get; set; }
    public bool paused { get; set; }
    public bool isPlaying { get; set; }

    // Variables (private)
    private Camera mainCamera;
    private Player player;
    private HUD hud;
    private Difficulty _difficulty;
    private int pauseMsgToken;
    private float spawnElapsed;
    private float bossSpawnElapsed;
    private bool backToMainMenu;
    private int backToMainMenuMsgToken;

    // Use this for initialization
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.Find("Tank").GetComponent<Player>();
        hud = GameObject.Find("HUD").GetComponent<HUD>();
        pauseMsgToken = 0;
        backToMainMenuMsgToken = 0;
        spawnElapsed = Time.time;
        bossSpawnElapsed = Time.time;

        difficulty = DifficultySelection.difficulty;
        audioSrc = GetComponent<AudioSource>();
        bottomLeft = (Vector2)mainCamera.ScreenToWorldPoint(mainCamera.pixelRect.position);
        topRight = (Vector2)mainCamera.ScreenToWorldPoint(mainCamera.pixelRect.size);
        paused = false;
        isPlaying = true;

        hud.DisplayMessage(MessagePlacement.Center, "Prepare to fight!", 3.0f);

        InvokeRepeating("ShowTips", 0.0f, tipsInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) BackToMainMenu();
        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }
        }
        if (backToMainMenu)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                hud.RemoveMessage(MessagePlacement.Center, backToMainMenuMsgToken);
                backToMainMenu = false;
                Pause(false);
            }
        }

        spawnElapsed = (paused || !isPlaying) ? spawnElapsed + Time.deltaTime : spawnElapsed;
        bossSpawnElapsed = (paused || !isPlaying) ? bossSpawnElapsed + Time.deltaTime : bossSpawnElapsed;
        if (!paused)
        {
            if (isPlaying)
            {
                if (Time.time >= spawnElapsed + settings.alienSpawnInterval)
                {
                    SpawnMonster(Random.Range(0, 2));
                    spawnElapsed = Time.time;
                }
                if (Time.time >= bossSpawnElapsed + settings.bossSpawnInterval)
                {
                    audioSrc.PlayOneShot(bossSpawnSound);
                    hud.DisplayMessage(MessagePlacement.Center, "ALERT!", 3.0f);
                    hud.DisplayMessage(MessagePlacement.Center, "The boss has spawned", 3.0f);
                    hud.DisplayMessage(MessagePlacement.BottomLeft, "Prepare your mental, commander :)", 5.0f);
                    SpawnMonster(genericAliens.Length - 1);
                    bossSpawnElapsed = Time.time;
                }
            }
        }
    }

    public Difficulty difficulty
    {
        get
        {
            return _difficulty;
        }
        set
        {
            _difficulty = value;

            ParseDifficulty();
        }
    }

    public GameSettings settings
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return easySettings;
                case Difficulty.Medium:
                    return mediumSettings;
                case Difficulty.Hard:
                    return hardSettings;
            }

            return null;
        }
    }

    void ShowTips()
    {
        int tip = Random.Range(0, tips.Length);
        hud.DisplayMessage(MessagePlacement.BottomLeft, tips[tip], tipsTimeout);
    }

    void SpawnMonster(int type)
    {
        Alien alien = Instantiate<Alien>(genericAliens[type]);
        alien.gameObject.SetActive(true);

        Vector2 pos = Vector2.zero;
        Vector2 extent = (Vector2)alien.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // bottom
                pos.x = Random.Range(bottomLeft.x, topRight.x);
                pos.y = bottomLeft.y - extent.y;
                break;
            case 1: // left
                pos.x = topRight.x - extent.x;
                pos.y = Random.Range(bottomLeft.y, topRight.y);
                break;
            case 2: // top
                pos.x = Random.Range(bottomLeft.x, topRight.x);
                pos.y = topRight.y + extent.y;
                break;
            case 3: // right
                pos.x = topRight.x + extent.x;
                pos.y = Random.Range(bottomLeft.y, topRight.y);
                break;                
        }
        alien.transform.position = (Vector3)pos;
    }

    void TogglePause()
    {
        Pause(!paused);
    }

    public void BackToMainMenu()
    {
        if (!backToMainMenu)
        {
            Pause(true);
            backToMainMenuMsgToken = hud.DisplayMessage(MessagePlacement.Center, "Back to main menu? (Y/N)");
            backToMainMenu = true;
        }
    }

    public void GameOver()
    {
        foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            DynamicObject dynobj = obj.GetComponent<DynamicObject>();
            if (dynobj)
            {
                if (dynobj.tag == "Alien")
                {
                    dynobj.isCanMove = false;
                }
                else
                {
                    Destroy(dynobj.gameObject);
                }
            }
        }
        isPlaying = false;
        hud.DisplayMessage(MessagePlacement.Center, "GAME OVER!");
        hud.DisplayMessage(MessagePlacement.BottomLeft, "Press escape to back to main menu");
    }

    public void Pause(bool pause)
    {
        if (paused != pause)
        {
            paused = pause;
            foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                DynamicObject dynobj = obj.GetComponent<DynamicObject>();
                if (dynobj)
                {
                    dynobj.isCanMove = !paused;
                }
            }

            if (paused)
            {
                pauseMsgToken = hud.DisplayMessage(MessagePlacement.Center, "[PAUSED]");
            }
            else
            {
                hud.RemoveMessage(MessagePlacement.Center, pauseMsgToken);
                pauseMsgToken = 0;
            }
        }
    }

    public void ParseDifficulty()
    {
        player.SetInfo(player.maxHealth * settings.playerHealthFactor, player.maxShield * settings.playerHealthFactor);
        player.musicPlayer.clip = settings.music;
        player.musicPlayer.Play();
    }
}
