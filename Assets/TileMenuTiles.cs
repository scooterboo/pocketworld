using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMenuTiles : MonoBehaviour
{
    public Tilemap WorldTilemap;
    public List<Tile> tiles;
    public Sprite LeftArrow;
    public Sprite RightArrow;
    private Tile LeftArrowTile;
    private Tile RightArrowTile;
    private Tilemap ThisTilemap;
    private int CurrTab = 0;
    private int[] toptabs = { -1, 10, 20, 30, 40, 50, 60, 70, 80, 90, -2, -1, 100, 110, 6, 6, 6, 6, 6, 6, 6, -2 };

    // Start is called before the first frame update
    void Start()
    {

        ThisTilemap = gameObject.GetComponent<Tilemap>();
        tiles = WorldTilemap.GetComponent<the_world>().tiles;

        LeftArrowTile = ScriptableObject.CreateInstance<Tile>();
        RightArrowTile = ScriptableObject.CreateInstance<Tile>();
        LeftArrowTile.sprite = LeftArrow;
        RightArrowTile.sprite = RightArrow;
        LeftArrowTile.name = "-1";
        RightArrowTile.name = "-2";

        UpdateTopTabs();

        for (int i = 0; i < 80; ++i)
        {
            ThisTilemap.SetTile(new Vector3Int(i % 10, -(i / 10), 0), tiles[i + 1]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if the mouse gets clicked and the menu is up.
        if (Input.GetMouseButtonDown(0) && gameObject.transform.position.z < 0)
        {
            Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 ppos = gameObject.transform.parent.position;

            //if we are not on the left bar
            if (mpos.x - ppos.x > -0.5f)
            {
                //top menu
                if (mpos.y - ppos.y > 1.25)
                {
                    //find what top menu tab the player is clicking.
                    int x = Mathf.FloorToInt(.1875f + (mpos.x - ppos.x) / 1.5f);
                    if (x > 10) x = 10;
                    if (x < 0) x = 0;
                    int TabTile = int.Parse(ThisTilemap.GetTile<Tile>(new Vector3Int(x, 1, 0)).name);

                    //-1 is left arrow, -2 is right arrow. update if you click an arrow
                    //left arrow
                    if (TabTile == -1)
                    {
                        CurrTab -= 11;
                        UpdateTopTabs();
                    }
                    //right arrow
                    else if (TabTile == -2)
                    {
                        CurrTab += 11;
                        UpdateTopTabs();
                    }
                    //an actual tab
                    else
                    {
                        //tab selected
                        //update the menu
                    }

                }
                else
                //lower menu
                {
                    //move tile to side bar
                }

            }
        }
    }

    private void UpdateTopTabs()
    {
        if (CurrTab < 0) CurrTab = toptabs.Length - 11;
        if (CurrTab >= toptabs.Length) CurrTab = 0;


        for (int i = 0; i < 11; ++i)
        {
            if (toptabs[i + CurrTab] == -1)
            {
                ThisTilemap.SetTile(new Vector3Int(i, 1, 0), LeftArrowTile);
                continue;
            }
            if (toptabs[i + CurrTab] == -2)
            {
                ThisTilemap.SetTile(new Vector3Int(i, 1, 0), RightArrowTile);
                continue;
            }
            ThisTilemap.SetTile(new Vector3Int(i, 1, 0), tiles[toptabs[i + CurrTab]]);
        }
    }
}
