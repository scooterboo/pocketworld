using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class the_world : MonoBehaviour
{
    public int chunksize=16;
    public List<Tile> tiles = new List<Tile>();
    private perlin noise;
    private Bakery bakery;
    private string savefolder;
    public string texturefolder;
    public string world;

    public struct tiledata
    {
        public bool water;
        public bool bump;
    }
    public Dictionary<Tile, tiledata> properties;

    // Start is called before the first frame update
    public void Start()
    {


        savefolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\.pocketworld\\save";
        texturefolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\.pocketworld\\textures";
        world = "loaf-1";

        //GUI.Label(new Rect(Screen.width / 2, Screen.height / 20, 100, 20), "Can you go find me a ROM file for pokemon blue?");
        //bool answer = EditorUtility.DisplayDialog("Find me the pokemon ROM", "Can you go find me a ROM file for pokemon blue?", "Yes, open file", "No, quit please");



        Spriter spriter = new Spriter();
        spriter.Start();

        tiles.Add(null);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(File.ReadAllBytes(texturefolder+"\\BlueNA\\16x16sheet.png"));
        int counter = 1;
        for (int i = 31; i >= 0; --i)
        {
            for (int j = 0; j < 32; ++j)
            {
                Tile can = ScriptableObject.CreateInstance<Tile>();
                can.sprite = Sprite.Create(texture, new Rect(j * 16f, i * 16f, 16f, 16f), new Vector2(0.5f, 0.5f), 16.0f);
                can.sprite.texture.filterMode = FilterMode.Point;
                can.name = counter++ + "";
                tiles.Add(can);
            }
        }
        //add the house tile
        {
            texture = Resources.Load<Texture2D>("4x2house");
            Tile can = ScriptableObject.CreateInstance<Tile>();
            can.sprite = Sprite.Create(texture, new Rect(0f, 0f, 16f, 16f), new Vector2(0.5f, 0.5f), 16.0f);
            can.sprite.texture.filterMode = FilterMode.Point;
            can.name = "827";
            tiles[827] = can;
        }

        properties = new Dictionary<Tile, tiledata>();
        tiledata tileproperties;
        tileproperties.water = false;
        tileproperties.bump = false;
        for (int i = 1; i < tiles.Count; ++i)
        {
            properties.Add(tiles[i], tileproperties);
        }
        //properties, please fill this out soon:


        //water
        tileproperties.water = true;
        {
            int[] temparr = { 13, 42, 43, 44, 45, 46, 65, 66, 113 };
            for (int i = 0; i < temparr.Length; ++i)
            {
                properties[tiles[temparr[i]]] = tileproperties;
            }
        }
        tileproperties.water = false;

        //things that go bump
        tileproperties.bump = true;
        //houses
        {
            int[] temparr = { 1, 2, 3, 4, 7, 8, 9, 11, 12, 23, 24, 26, 27, 28, 29, 30, 31, 33, 34, 35, 36, 37, 38, 47, 48,
                49, 50, 69, 71, 72, 73, 74, 75, 78, 79, 80, 81, 82, 83, 101, 102, 103 , 104, 105, 106, 109, 110, 111, 112 };
            //10 is door, which is "house", but is walkable.
            for (int i = 0; i < temparr.Length; ++i)
            {
                properties[tiles[temparr[i]]] = tileproperties;
            }
        }
        //cliff
        {
            int[] temparr = { 16, 18, 56, 57, 58, 59, 63, 64, 76, 77, 84 };
            //17 is cliff door, which is "cliff", but is walkable.
            for (int i = 0; i < temparr.Length; ++i)
            {
                properties[tiles[temparr[i]]] = tileproperties;
            }
        }

        //ledge - make these "hop" later
        {
            int[] temparr = { 20, 39, 40, 67, 87, 88, 91, 93, 95, 96 };
            for (int i = 0; i < temparr.Length; ++i)
            {
                properties[tiles[temparr[i]]] = tileproperties;
            }
        }


        //trees
        properties[tiles[32]] = tileproperties; //large tree
        properties[tiles[70]] = tileproperties; //cuttable tree

        //other

        properties[tiles[14]] = tileproperties; //pylon
        properties[tiles[22]] = tileproperties; //sign
        properties[tiles[41]] = tileproperties; //fence



        //interior
        properties[tiles[122]] = tileproperties;
        properties[tiles[131]] = tileproperties;





        noise = gameObject.GetComponent<perlin>();
        bakery = gameObject.GetComponent <Bakery>();

    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void loadchunk(int posX, int posY)
    {

        int FloorposX = chunksize * Mathf.FloorToInt(posX / (float)chunksize);
        int FloorposY = chunksize * Mathf.FloorToInt(posY / (float)chunksize);

        Tilemap tilemap = gameObject.GetComponent<Tilemap>();
        int[] data;

        //attempt to read the chunk from the file
        data = bakery.LoadSlice(world, chunksize, FloorposX, FloorposY);
        //if we can't read it off of the disk, make a new chunk instead.
        if (data==null)
        {
            data = noise.mapit(world, chunksize, FloorposX - 1, FloorposY - 1);
            //write to disk
            bakery.SaveSlice(world, data, chunksize, FloorposX, FloorposY);
        }

        //write to world
        for (int i = 0; i < (chunksize * chunksize); ++i)
        {
            tilemap.SetTile(new Vector3Int((i % chunksize) + FloorposX, (i / chunksize) + FloorposY, 0), tiles[data[i]]);
        }

    }

    public void loadchunk3x3(int intxpos, int intypos)
    {
        System.IO.Directory.CreateDirectory(savefolder + "\\" + world);
        Tilemap tilemap = gameObject.GetComponent<Tilemap>();
        //8 and 11 are where the borders of the screen are
        for (int i = intypos - 8; i <= (intypos + 8); i += 8)
        {
            for (int j = (intxpos - 11); j <= (intxpos + 11); j += 11)
            {
                if (tilemap.GetTile<Tile>(new Vector3Int(j, i, 0)) == null)
                {
                    loadchunk(j, i);
                }
            }
        }
    }

    public void pasteHouse(house house,int posx, int posy)
    {
        Tilemap tilemap = gameObject.GetComponent<Tilemap>();
        for (int i = 0; i < house.height; ++i)
        {
            for (int j = 0; j < house.width; ++j)
            {
                Tile tile = tiles[house.data[(i * house.width) + j]];
                //save tile to file.
                gameObject.GetComponent<Bakery>().SaveCrumb(world, int.Parse(tile.name), chunksize, posx + j, posy + i);
                //set the tile in the world
                tilemap.SetTile(new Vector3Int(posx + j, posy + i, 0), tile);
            }
        }
    }
}

[System.Serializable]
public class chunk
{
    public string data;
}