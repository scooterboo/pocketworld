using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



//handles time, input, and auto-saving
public class ClientController : MonoBehaviour
{
    public float walk_delay = 10f; //rename to tick speed
    private float time = 0;
    public Transform MainTileMenuTrans;
    private int direction = 0;
    public Tilemap SelectorTilemap;
    private Saving saving;
    public Tilemap MenuTilemap;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        saving = new Saving();
        saving.Start(); //I'm doing something wrong here.
    }

    // Update is called once per frame
    void Update()
    {
        //check for if the user wishes to quit
        checkForQuit();

        //Take buffered inputs. if you press an input, we'll go that way on the next turn.
        direction = readPressedInputs(direction);

        //wait until time
        //this is done in a way where the client will take a step as soon as able.
        if (time + walk_delay < Time.time)
        {
            time += walk_delay;
            //take inputs if they are held. holding a direction overrides pressing a direction.
            direction = readHeldInputs(direction);
            //prompt movement
            gameObject.GetComponent<Movement>().attemptMovement(direction);
            //reset direction
            direction = 0;
            //update the selector grid
            SelectorTilemap.GetComponent<SelectorGrid>().UpdateSprite();
            //save
            int[] tiles = new int[8];
            for (int i = 0; i < 8; ++i)
            {
                tiles[i] = int.Parse(MenuTilemap.GetTile(new Vector3Int(0, i, 0)).name);
            }
            Vector3Int pos = gameObject.GetComponent<Movement>().intpos;
            int[] sprite = new int[] { gameObject.GetComponent<Movement>().currsprite, 0 };
            saving.saveplayer(pos, sprite, tiles);
        }
    }

    //TODO: move these three to an input class later. This will make remapping easier
    private int readPressedInputs(int direction)
    {
        //if the menu is up, don't take inputs
        if (MainTileMenuTrans.position.z > 0)
        {
            if (Input.GetKeyDown("up") || Input.GetKeyDown("w")) direction = 1;
            if (Input.GetKeyDown("right") || Input.GetKeyDown("d")) direction = 2;
            if (Input.GetKeyDown("down") || Input.GetKeyDown("s")) direction = 3;
            if (Input.GetKeyDown("left") || Input.GetKeyDown("a")) direction = 4;
        }
        return direction;
    }

    private int readHeldInputs(int direction)
    {
        //if the menu is up, don't take inputs
        if (MainTileMenuTrans.position.z > 0)
        {
            if (Input.GetKey("up") || Input.GetKey("w")) direction = 1;
            if (Input.GetKey("right") || Input.GetKey("d")) direction = 2;
            if (Input.GetKey("down") || Input.GetKey("s")) direction = 3;
            if (Input.GetKey("left") || Input.GetKey("a")) direction = 4;
        }
        return direction;
    }

    private void checkForQuit()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

}
