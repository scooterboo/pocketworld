using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public struct house
{
    public int width;
    public int height;
    public int offsetx;
    public int offsety;
    public int[] data;

    public house(int width, int height, int offsetx, int offsety, int[] data)
    {
        this.width = width;
        this.height = height;
        this.offsetx = offsetx;
        this.offsety = offsety;
        this.data = data;
    }
}


public class SelectorGrid : MonoBehaviour
{
    public Tilemap WorldTilemap;
    public Transform player;
    public Dictionary<int,house> HDir = new Dictionary<int, house>();
    public int currtile;

    // Start is called before the first frame update
    void Start()
    {
        
        house temphouse;
        for (int i = 1; i < 833; ++i)
        {
            temphouse = new house(1, 1, 0, 0, new int[] { i });
            HDir.Add(i, temphouse);
        }
        temphouse = new house(4, 2, 1, 0, new int[] { 9, 10, 3, 12, 7, 8, 8, 11 });
        HDir[827]= temphouse;//4x2house

        currtile = 1;

        //InvokeRepeating("MakeVisible", .3f, .3f);
        //InvokeRepeating("MakeInvisible", .2f, .3f);
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)&& gameObject.transform.position.z<0)
        {
            WorldTilemap.GetComponent<the_world>().pasteHouse(HDir[currtile], (int)(gameObject.transform.position.x+.5f), (int)(gameObject.transform.position.y + .5f));
        }

    }


    public void UpdateSprite()
    {
        Tilemap tilemap = gameObject.GetComponent<Tilemap>();
        tilemap.ClearAllTiles();
        Vector3 pos = gameObject.transform.localPosition;
        pos.x = -HDir[currtile].offsetx;
        pos.y = -HDir[currtile].offsety;
        gameObject.transform.localPosition = pos;
        List<Tile> tiles = WorldTilemap.GetComponent<the_world>().tiles;

        for (int i = 0; i < HDir[currtile].height; ++i)
        {
            for (int j = 0; j < HDir[currtile].width; ++j)
            {
                Vector3Int tpos = new Vector3Int(j + (int)gameObject.transform.position.x, i + (int)gameObject.transform.position.y, 0);
                Tile tile = WorldTilemap.GetTile<Tile>(tpos);
                if (tile == null) continue;
                if (WorldTilemap.GetComponent<the_world>().properties[tile].water || WorldTilemap.GetComponent<the_world>().properties[tile].bump || player.position == tpos)
                {
                    tilemap.SetTile(new Vector3Int(j, i, 0), tiles[816]);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(j, i, 0), tiles[HDir[currtile].data[i * HDir[currtile].width + j]]);
                }
            }
        }
    }

}
