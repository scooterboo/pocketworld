using UnityEngine;
using UnityEngine.Tilemaps;


//Don't worry, I got this
public class TeleportHandler : MonoBehaviour
{
    private string savefolder;
    private Vector3Int oldpos = new Vector3Int(0, 0, 0);
    public GameObject movement;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IWannaTeleport()
    {
        if (gameObject.GetComponent<the_world>().world == "loaf-1")
        {
            Vector3Int pos = movement.GetComponent<Movement>().intpos;
            string wolrdstring = "loaf-" + System.Convert.ToBase64String(System.BitConverter.GetBytes(((long)pos.x << 32) + (long)pos.y)).Replace('/', '=');
            TeleportInterior(wolrdstring, new Vector3Int(0, 0, 0));
        }
        else
        {
            TeleportExterior("loaf-1");
        }
    }


    private void TeleportExterior(string worldID)
    {
        //change world
        gameObject.GetComponent<the_world>().world = worldID;

        oldpos.y -= 1;

        //teleport
        movement.GetComponent<Movement>().intpos = oldpos;
        movement.transform.position = oldpos;

        //clear old tiles
        gameObject.GetComponent<Tilemap>().ClearAllTiles();

        //paint new tiles
        gameObject.GetComponent<the_world>().loadchunk3x3(Mathf.FloorToInt(oldpos.x), Mathf.FloorToInt(oldpos.y));


    }
    private void TeleportInterior(string worldID, Vector3Int pos)
    {
        //change world
        gameObject.GetComponent<the_world>().world = worldID;

        //teleport
        oldpos = movement.GetComponent<Movement>().intpos;
        movement.GetComponent<Movement>().intpos = pos;
        movement.transform.position = pos;

        //clear old tiles
        gameObject.GetComponent<Tilemap>().ClearAllTiles();

        //paint new tiles
        gameObject.GetComponent<the_world>().loadchunk3x3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));



        //check if there is a house there!
        Tilemap tilemap = gameObject.GetComponent<Tilemap>();
        if (tilemap.GetTile<Tile>(new Vector3Int(0, 0, 0)).name != "131")
        {
            return;
        }

        //build house
        house house = gameObject.GetComponent<perlin>().generateInterior((int)oldpos.x, (int)oldpos.y);

        //paint house
        gameObject.GetComponent<the_world>().pasteHouse(house, 0 - house.offsetx, -1);



    }

}
