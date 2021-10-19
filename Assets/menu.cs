using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class menu : MonoBehaviour
{
    public Tilemap WorldTilemap;
    public Tilemap MenuTilemap;
    public Tilemap SelectorTilemap;
    public Transform menuselector;
    private int tileloc = 0;

    private int tilemax = 824;

    // Start is called before the first frame update
    void Start()
    {
        Texture2D menu = new Texture2D(2, 2);
        menu.LoadImage(File.ReadAllBytes(WorldTilemap.GetComponent<the_world>().texturefolder + "\\BlueNA\\menusheet.png"));
        //Texture2D menu = Resources.Load<Texture2D>("menusheet");
        menu.filterMode = FilterMode.Point;
        Sprite menusprite = Sprite.Create(menu, new Rect(0, 0, menu.width, menu.height), new Vector2(0.5f, 0.5f), 16.0f, 1, SpriteMeshType.FullRect, new Vector4(8, 8, 8, 8));
        //gameObject.transform.localPosition = new Vector3(0f, -5.5f, 0f);
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = menusprite;
        sr.drawMode = SpriteDrawMode.Sliced;
        sr.size = new Vector2(3f, 15f);

        menuselector.localPosition = new Vector3(0f, -4.5f, -0.5f);


        for (int i = 0; i <= 8; ++i)
        {
            MenuTilemap.SetTile(new Vector3Int(0, i-1, 0), WorldTilemap.GetComponent<the_world>().tiles[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mwheel = Input.GetAxis("Mouse ScrollWheel");

        if (mwheel > 0f && gameObject.transform.position.z < 0)
        {
            Vector3 pos = menuselector.localPosition;
            pos.y += 1.5f;
            if (pos.y > 6.5f) pos.y -= 12f;
            menuselector.localPosition = pos;
            SelectorTilemap.GetComponent<SelectorGrid>().currtile = int.Parse(MenuTilemap.GetTile<Tile>(new Vector3Int((int)menuselector.localPosition.x, (int)(3.5f + menuselector.localPosition.y / 1.5f), 0)).name);
            SelectorTilemap.GetComponent<SelectorGrid>().UpdateSprite();
        }
        if (mwheel < 0f && gameObject.transform.position.z < 0)
        {
            Vector3 pos = menuselector.localPosition;
            pos.y -= 1.5f;
            if (pos.y < -5.5f) pos.y += 12f;
            menuselector.localPosition = pos;
            SelectorTilemap.GetComponent<SelectorGrid>().currtile = int.Parse(MenuTilemap.GetTile<Tile>(new Vector3Int((int)menuselector.localPosition.x, (int)(3.5f + menuselector.localPosition.y / 1.5f), 0)).name);
            SelectorTilemap.GetComponent<SelectorGrid>().UpdateSprite();
        }

        if (Input.GetMouseButtonDown(0) && gameObject.transform.position.z < 0)
        {
            Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 ppos = gameObject.transform.parent.position;
            if ( mpos.x- ppos.x <= -7.5f)
            {
                Vector3 pos = menuselector.localPosition;
                pos.y = Mathf.Floor(.5f+(mpos.y - ppos.y) / 1.5f) * 1.5f;
                if (pos.y > 6) pos.y = 6;
                if (pos.y < -6) pos.y = -6;
                menuselector.localPosition = pos;
                SelectorTilemap.GetComponent<SelectorGrid>().currtile = int.Parse(MenuTilemap.GetTile<Tile>(new Vector3Int((int)menuselector.localPosition.x, (int)(3.5f + menuselector.localPosition.y / 1.5f), 0)).name);
                SelectorTilemap.GetComponent<SelectorGrid>().UpdateSprite();
            }

        }


        if (Input.GetKeyDown("2") && gameObject.transform.position.z < 0)
        {
            tileloc += 8;
            if (tileloc > tilemax)
            {
                tileloc -= (tilemax + 8);
            }
            for (int i = (tileloc + 1); i <= (tileloc + 8); ++i)
            {
                MenuTilemap.SetTile(new Vector3Int(0, i - (tileloc + 1), 0), WorldTilemap.GetComponent<the_world>().tiles[i]);
            }
            SelectorTilemap.GetComponent<SelectorGrid>().currtile = int.Parse(MenuTilemap.GetTile<Tile>(new Vector3Int((int)menuselector.localPosition.x, (int)(3.5f + menuselector.localPosition.y / 1.5f), 0)).name);
            SelectorTilemap.GetComponent<SelectorGrid>().UpdateSprite();
        }
        if (Input.GetKeyDown("1") && gameObject.transform.position.z < 0)
        {
            tileloc -= 8;
            if (tileloc < 0)
            {
                tileloc += (tilemax + 8);
            }
            for (int i = (tileloc + 1); i <= (tileloc + 8); ++i)
            {
                MenuTilemap.SetTile(new Vector3Int(0, i - (tileloc + 1), 0), WorldTilemap.GetComponent<the_world>().tiles[i]);
            }
            SelectorTilemap.GetComponent<SelectorGrid>().currtile = int.Parse(MenuTilemap.GetTile<Tile>(new Vector3Int((int)menuselector.localPosition.x, (int)(3.5f + menuselector.localPosition.y / 1.5f), 0)).name);
            SelectorTilemap.GetComponent<SelectorGrid>().UpdateSprite();
        }

        if (Input.GetKeyDown("q"))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -gameObject.transform.position.z);
        }

    }
}
