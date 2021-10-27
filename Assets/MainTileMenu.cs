using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class MainTileMenu : MonoBehaviour
{
    public Tilemap WorldTilemap;
    // Start is called before the first frame update
    void Start()
    {
        Texture2D mainmenu = new Texture2D(2, 2);
        mainmenu.LoadImage(File.ReadAllBytes(WorldTilemap.GetComponent<the_world>().texturefolder + "\\BlueNA\\menusheet2.png"));
        mainmenu.filterMode = FilterMode.Point;
        Sprite menusprite = Sprite.Create(mainmenu, new Rect(0, 0, mainmenu.width, mainmenu.height), new Vector2(0.5f, 0.5f), 16.0f, 1, SpriteMeshType.FullRect);
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = menusprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
