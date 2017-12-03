using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDataCacher : MonoBehaviour
{

    public Sprite[] sprites;
    public Dictionary<string, Sprite> spriteImg;
    public Dictionary<string, string> spriteDesc;
    public Dictionary<string, int> spriteCost;

    void Awake()
    {
        spriteImg = new Dictionary<string, Sprite>();
        spriteDesc = new Dictionary<string, string>();
        spriteCost = new Dictionary<string, int>();

        spriteDesc.Add("shield", "普通的盾牌");
        spriteDesc.Add("tools", "恢复部分城墙的血量。");

        spriteCost.Add("shield", 200);
        spriteCost.Add("tools", 300);

        for (int i = 0; i < sprites.Length; i++)
        {
            spriteImg.Add(sprites[i].name, sprites[i]);
        }

        DontDestroyOnLoad(gameObject);
    }
}
