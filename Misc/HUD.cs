using UnityEngine;
using System.Collections;

struct HUDMessage
{
    public string text;
    public bool permanent;
    public float target;

    public HUDMessage(string _text, float _duration)
    {
        text = _text;
        permanent = false;
        target = Time.time + _duration;
    }

    public HUDMessage(string _text)
    {
        text = _text;
        permanent = true;
        target = 0.0f;
    }
}

struct PlayerInfo
{
    public string text;
    public Texture2D image;
    public float value;
    public float maxValue;

    public PlayerInfo(string _text, Texture2D _image, float _value, float _maxValue)
    {
        text = _text;
        image = _image;
        value = _value;
        maxValue = _maxValue;
    }
}

public enum MessagePlacement
{
    Center,
    BottomLeft,
}

public class HUD : MonoBehaviour
{
    // Constants
    public GUIStyle centerMsgStyle;
    public float centerMsgPadding;
    public GUIStyle bottomLeftMsgStyle;
    public float bottomLeftMsgPadding;
    public GUIStyle playerInfoStyle;
    public float playerInfoPadding;
    public Texture2D playerHealthBarImage;
    public Texture2D playerShieldBarImage;
    public Texture2D experienceBarImage;

    // Private variables
    private System.Collections.Generic.Dictionary<int, HUDMessage> centerMsgQueue;
    private System.Collections.Generic.Dictionary<int, HUDMessage> bottomLeftMsgQueue;
    private Camera mainCamera;
    private Player player;
    private PlayerInfo[] playerInfo;
    private World world;

    // Use this for initialization
    void Start()
    {
        centerMsgQueue = new System.Collections.Generic.Dictionary<int, HUDMessage>();
        bottomLeftMsgQueue = new System.Collections.Generic.Dictionary<int, HUDMessage>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.Find("Tank").GetComponent<Player>();

        playerInfo = new PlayerInfo[3];
        playerInfo[0] = new PlayerInfo("Health", playerHealthBarImage, 0.0f, player.maxHealth);
        playerInfo[1] = new PlayerInfo("Shield", playerShieldBarImage, 0.0f, player.maxShield);
        playerInfo[2] = new PlayerInfo("Experience", experienceBarImage, 0.0f, player.experienceNeeded);

        world = GameObject.Find("World").GetComponent<World>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInfo[0].value = player.health;
        playerInfo[0].maxValue = player.maxHealth;
        playerInfo[1].value = player.shield;
        playerInfo[1].maxValue = player.maxShield;
        playerInfo[2].value = player.experience;
        playerInfo[2].maxValue = player.experienceNeeded;

        foreach (int key in centerMsgQueue.Keys)
        {
            HUDMessage msg = centerMsgQueue[key];
            if (!msg.permanent)
            {
                if (Time.time >= msg.target)
                {
                    centerMsgQueue.Remove(key);
                    break;
                }
            }
        }

        foreach (int key in bottomLeftMsgQueue.Keys)
        {
            HUDMessage msg = bottomLeftMsgQueue[key];
            if (!msg.permanent)
            {
                if (Time.time >= msg.target)
                {
                    bottomLeftMsgQueue.Remove(key);
                    break;
                }
            }
        }
    }

    void OnGUI()
    {
        { // Draw level, score & difficulty
            GUIContent score = new GUIContent(string.Format("[ Level: {0} | Score: {1} | Difficulty: {2} ]", player.level, player.score, world.difficulty.ToString()));
            Vector2 size = playerInfoStyle.CalcSize(score);
            Vector2 placement = Vector2.zero;

            placement.x = ((float)mainCamera.pixelWidth - size.x) / 2.0f;

            Rect rect = new Rect(placement, size);
            GUI.Label(rect, score, playerInfoStyle);
        }

        { // Draw player info
            Vector2 placement = Vector2.zero;

            foreach (PlayerInfo info in playerInfo)
            {
                string infoText = string.Format("{0}: {1} / {2}", info.text, info.value, info.maxValue);

                GUIContent text = new GUIContent(infoText);
                Vector2 textSize = playerInfoStyle.CalcSize(text);

                float maxWidth = (float)mainCamera.pixelWidth / 5.0f;

                Vector2 imgSize = Vector2.zero;
                imgSize.x = info.value / info.maxValue * maxWidth;
                imgSize.y = (float)info.image.height;

                float deltaY = Mathf.Max(textSize.y, imgSize.y) + playerInfoPadding;

                Vector2 imgPlacement = Vector2.zero;
                imgPlacement.x = placement.x;
                imgPlacement.y = placement.y + (imgSize.y < textSize.y ? (textSize.y - imgSize.y) / 2.0f : 0.0f);
                Rect imgRect = new Rect(imgPlacement, imgSize);
                GUI.DrawTexture(imgRect, info.image);

                Vector2 textPlacement = Vector2.zero;
                textPlacement.x = placement.x;
                textPlacement.y = placement.y + (textSize.y < imgSize.y ? (imgSize.y - textSize.y) / 2.0f : 0.0f);
                Rect textRect = new Rect(textPlacement, textSize);
                GUI.Label(textRect, text, playerInfoStyle);

                placement.y += deltaY;
            }
        }

        { // Draw hud messages
            int centerCount = centerMsgQueue.Count;
            int centerIndex = 0;
            float centerHeight = (float)centerMsgStyle.font.lineHeight + centerMsgPadding;
            float centerAnchorY = ((float)mainCamera.pixelHeight - (float)centerCount * centerHeight) / 2.0f;

            foreach (int key in centerMsgQueue.Keys)
            {
                HUDMessage msg = centerMsgQueue[key];

                GUIContent content = new GUIContent(msg.text);
                Vector2 size = centerMsgStyle.CalcSize(content);

                Vector2 placement = Vector2.zero;
                placement.x = ((float)mainCamera.pixelWidth - size.x) / 2.0f;
                placement.y = centerAnchorY + (float)centerIndex * centerHeight;

                Rect rect = new Rect(placement, size);
                GUI.Label(rect, content, centerMsgStyle);

                centerIndex++;
            }

            int bottomLeftCount = bottomLeftMsgQueue.Count;
            int bottomLeftIndex = 0;
            float bottomLeftHeight = (float)bottomLeftMsgStyle.font.lineHeight + bottomLeftMsgPadding;
            float bottomLeftAnchorY = (float)mainCamera.pixelHeight - (float)bottomLeftCount * bottomLeftHeight;

            foreach (int key in bottomLeftMsgQueue.Keys)
            {
                HUDMessage msg = bottomLeftMsgQueue[key];

                GUIContent content = new GUIContent(msg.text);
                Vector2 size = bottomLeftMsgStyle.CalcSize(content);

                Vector2 placement = Vector2.zero;
                placement.x = 0.0f;
                placement.y = bottomLeftAnchorY + (float)bottomLeftIndex * bottomLeftHeight;

                Rect rect = new Rect(placement, size);
                GUI.Label(rect, content, bottomLeftMsgStyle);

                bottomLeftIndex++;
            }
        }
    }

    public int DisplayMessage(MessagePlacement placement, string text, float duration)
    {
        int token = Random.Range(0, int.MaxValue);

        switch (placement)
        {
            case MessagePlacement.Center:
                centerMsgQueue.Add(token, new HUDMessage(text, duration));
                break;
            case MessagePlacement.BottomLeft:
                bottomLeftMsgQueue.Add(token, new HUDMessage(text, duration));
                break;
        }

        return token;
    }

    public int DisplayMessage(MessagePlacement placement, string text)
    {
        int token = Random.Range(0, int.MaxValue);

        switch (placement)
        {
            case MessagePlacement.Center:
                centerMsgQueue.Add(token, new HUDMessage(text));
                break;
            case MessagePlacement.BottomLeft:
                bottomLeftMsgQueue.Add(token, new HUDMessage(text));
                break;
        }

        return token;
    }

    public void RemoveMessage(MessagePlacement placement, int token)
    {
        switch (placement)
        {
            case MessagePlacement.Center:
                centerMsgQueue.Remove(token);
                break;
            case MessagePlacement.BottomLeft:
                bottomLeftMsgQueue.Remove(token);
                break;
        }
    }
}
