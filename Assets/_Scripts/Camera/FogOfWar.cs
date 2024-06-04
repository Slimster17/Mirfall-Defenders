using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogOfWar : MonoBehaviour
{
    public Transform player;
    public float revealRadius = 5f;
    public Texture2D fogTexture;

    private Image fogImage;
    private Texture2D fogMask;

    void Start()
    {
        fogImage = GetComponent<Image>();
        fogMask = new Texture2D(fogTexture.width, fogTexture.height);
        ClearFog();
    }

    void Update()
    {
        UpdateFog();
    }

    void ClearFog()
    {
        for (int y = 0; y < fogMask.height; y++)
        {
            for (int x = 0; x < fogMask.width; x++)
            {
                fogMask.SetPixel(x, y, Color.black);
            }
        }
        fogMask.Apply();
        fogImage.sprite = Sprite.Create(fogMask, new Rect(0, 0, fogMask.width, fogMask.height), new Vector2(0.5f, 0.5f));
    }

    void UpdateFog()
    {
        Vector2 playerPos = new Vector2(player.position.x, player.position.z);
        Vector2Int maskPos = new Vector2Int(
            Mathf.FloorToInt((playerPos.x / fogTexture.width) * fogMask.width),
            Mathf.FloorToInt((playerPos.y / fogTexture.height) * fogMask.height)
        );

        for (int y = -Mathf.FloorToInt(revealRadius); y <= Mathf.FloorToInt(revealRadius); y++)
        {
            for (int x = -Mathf.FloorToInt(revealRadius); x <= Mathf.FloorToInt(revealRadius); x++)
            {
                int posX = maskPos.x + x;
                int posY = maskPos.y + y;

                if (posX >= 0 && posX < fogMask.width && posY >= 0 && posY < fogMask.height)
                {
                    float distance = Vector2.Distance(maskPos, new Vector2Int(posX, posY));
                    if (distance <= revealRadius)
                    {
                        fogMask.SetPixel(posX, posY, Color.white);
                    }
                }
            }
        }
        fogMask.Apply();
        fogImage.sprite = Sprite.Create(fogMask, new Rect(0, 0, fogMask.width, fogMask.height), new Vector2(0.5f, 0.5f));
    }
}
