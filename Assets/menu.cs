using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class menu : MonoBehaviour
{
    public Tilemap WorldTilemap;
    public Tilemap MenuTilemap;
    public Tilemap SelectorTilemap;
    public Transform menuselector;
    public Transform MainTileMenuTrans;


    // Start is called before the first frame update
    void Start()
    {
        Texture2D sidemenu = new Texture2D(2, 2);
        sidemenu.LoadImage(File.ReadAllBytes(WorldTilemap.GetComponent<the_world>().texturefolder + "\\BlueNA\\menusheet.png"));
        sidemenu.filterMode = FilterMode.Point;
        Sprite menusprite = Sprite.Create(sidemenu, new Rect(0, 0, sidemenu.width, sidemenu.height), new Vector2(0.5f, 0.5f), 16.0f, 1, SpriteMeshType.FullRect, new Vector4(8, 8, 8, 8));
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = menusprite;
        sr.drawMode = SpriteDrawMode.Sliced;
        sr.size = new Vector2(3f, 15f);

        menuselector.localPosition = new Vector3(0f, -4.5f, -0.5f);


        Saving saving = new Saving();
        saving.Start(); //I'm doing something wrong here.
        (Vector3Int position, int[] sprite, int[] tiles) = saving.loadplayer();
        MenuTilemap.SetTile(new Vector3Int(0, -1, 0), WorldTilemap.GetComponent<the_world>().tiles[0]);
        for (int i = 0; i < 8; ++i)
        {
            MenuTilemap.SetTile(new Vector3Int(0, i, 0), WorldTilemap.GetComponent<the_world>().tiles[tiles[i]]);
        }

        Texture2D menubutton = Resources.Load<Texture2D>("menu");
        Tile menutile = ScriptableObject.CreateInstance<Tile>();
        menutile.sprite = Sprite.Create(menubutton, new Rect(0f, 0f, 16f, 16f), new Vector2(0.5f, 0.5f), 16.0f);
        menutile.sprite.texture.filterMode = FilterMode.Point;
        menutile.name = "0";
        MenuTilemap.SetTile(new Vector3Int(0, -1, 0), menutile);

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
                if (pos.y > 6) pos.y = 6f;
                if (pos.y < -6) pos.y = -6f;
                if (pos.y == -6f)
                {
                    MainTileMenuTrans.position = new Vector3(MainTileMenuTrans.position.x, MainTileMenuTrans.position.y, -MainTileMenuTrans.position.z);
                }
                else
                {
                    menuselector.localPosition = pos;
                    SelectorTilemap.GetComponent<SelectorGrid>().currtile = int.Parse(MenuTilemap.GetTile<Tile>(new Vector3Int((int)menuselector.localPosition.x, (int)(3.5f + menuselector.localPosition.y / 1.5f), 0)).name);
                    SelectorTilemap.GetComponent<SelectorGrid>().UpdateSprite();
                }
            }

        }

        if (Input.GetKeyDown("q"))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -gameObject.transform.position.z);
            MainTileMenuTrans.position = new Vector3(MainTileMenuTrans.position.x, MainTileMenuTrans.position.y, Mathf.Abs(MainTileMenuTrans.position.z));

        }

    }

    //Changes the current tile selected on the menu to the given tile number
    public void updatetile(int tile)
    {
        Vector3 pos = menuselector.localPosition;
        MenuTilemap.SetTile(new Vector3Int((int)menuselector.localPosition.x, (int)(3.5f + menuselector.localPosition.y / 1.5f), 0), WorldTilemap.GetComponent<the_world>().tiles[tile]);
        SelectorTilemap.GetComponent<SelectorGrid>().currtile = tile;
        SelectorTilemap.GetComponent<SelectorGrid>().UpdateSprite();
    }
}
