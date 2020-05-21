using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    // Constants
    public Texture2D background;
    public Texture2D titleImage;
    public string titleText;
    public float logoScale;
    public GUISkin skin;
    public string[] buttons;
    public Vector2 buttonSize;
    public float buttonPadding;

    // Private variables
    private Camera mainCamera;

    // Use this for initialization
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUI.skin = skin;

        Vector2 bgPlacement = Vector2.zero;
        Vector2 bgSize = mainCamera.pixelRect.size;
        Rect bgRect = new Rect(bgPlacement, bgSize);
        GUI.DrawTexture(bgRect, background);

        Vector2 logoSize = Vector2.zero;
        logoSize.x = logoScale * (float)titleImage.width;
        logoSize.y = logoScale * (float)titleImage.height;

        GUIContent title = new GUIContent(titleText);
        Vector2 titleSize = skin.label.CalcSize(title);

        Vector2 logoPlacement = Vector2.zero;
        logoPlacement.y = logoSize.y < titleSize.y ? (titleSize.y - logoSize.y) / 2.0f : 0.0f;
        Rect logoRect = new Rect(logoPlacement, logoSize);
        GUI.DrawTexture(logoRect, titleImage);

        Vector2 titlePlacement = Vector2.zero;
        titlePlacement.x = logoPlacement.x + logoSize.x + 10.0f;
        titlePlacement.y = titleSize.y < logoSize.y ? (logoSize.y - titleSize.y) / 2.0f : 0.0f;
        Rect titleRect = new Rect(titlePlacement, titleSize);
        GUI.Label(titleRect, title);

        int btnCount = buttons.Length;
        int btnIndex = 0;
        float btnHeight = buttonSize.y + buttonPadding;
        float btnAnchorY = (bgSize.y - (float)btnCount * btnHeight) / 2.0f;
        foreach (string btn in buttons)
        {
            Vector2 btnPlacement = Vector2.zero;
            btnPlacement.x = (bgSize.x - buttonSize.x) / 2.0f;
            btnPlacement.y = btnAnchorY + btnIndex * btnHeight;
            Rect btnRect = new Rect(btnPlacement, buttonSize);
            if (GUI.Button(btnRect, btn)) OnMenuClick(btn);

            btnIndex++;
        }
    }

    void Play(Difficulty difficulty)
    {
        DifficultySelection.difficulty = difficulty;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }

    void OnMenuClick(string text)
    {
        switch (text)
        {
            case "Play (Easy)":
                Play(Difficulty.Easy);
                break;
            case "Play (Medium)":
                Play(Difficulty.Medium);
                break;
            case "Play (Hard)":
                Play(Difficulty.Hard);
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }
}
