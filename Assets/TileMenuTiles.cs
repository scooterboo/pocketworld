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

    // Start is called before the first frame update
    void Start()
    {

        Tilemap tilemap = gameObject.GetComponent<Tilemap>();
        tiles = WorldTilemap.GetComponent<the_world>().tiles;

        LeftArrowTile = ScriptableObject.CreateInstance<Tile>();
        RightArrowTile = ScriptableObject.CreateInstance<Tile>();
        LeftArrowTile.sprite = LeftArrow;
        RightArrowTile.sprite = RightArrow;
        tilemap.SetTile(new Vector3Int(0, 1, 0), LeftArrowTile);
        tilemap.SetTile(new Vector3Int(10, 1, 0), RightArrowTile);
        for (int i = 1; i < 10; ++i)
        {
            tilemap.SetTile(new Vector3Int(i, 1, 0), tiles[i + 8]);
        }


        for (int i = 0; i < 80; ++i)
        {
            tilemap.SetTile(new Vector3Int(i % 10, -(i / 10), 0), tiles[i + 1]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
