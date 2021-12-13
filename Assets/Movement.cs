using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    public bool choppy_movement = true;
    private float smoothment_factor;
    private string sprite_direction = "down";
    private float step_time = 0;
    public Vector3Int intpos;
    private int step_number = 0;
    private int step_direction = 0; //NESW is 1234
    private Dictionary<string, int> sprstr2inty = new Dictionary<string, int>();
    private bool foot_side_right = true;
    public Tilemap WorldTilemap;
    private Texture2D playertex;
    public int currsprite;
    private List<Sprite[]> playersprite = new List<Sprite[]>();
    public Transform MainTileMenuTrans;


    //public CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        //set up those inital values
        Saving saving = new Saving();
        saving.Start(); //I'm doing something wrong here.
        (Vector3Int position, int[] sprite, int[] tiles) = saving.loadplayer();
        intpos = position;
        currsprite = sprite[0];
        gameObject.transform.position = new Vector3(position.x, position.y, 0);
        step_time = 0;

        if (choppy_movement) { smoothment_factor = 8.0f; }
        else { smoothment_factor = 16.0f; }

        //build the ditionary. This lets me manipulate variable names like strings.
        sprstr2inty.Add("sprite_down", 0);
        sprstr2inty.Add("sprite_up", 1);
        sprstr2inty.Add("sprite_left", 2);
        sprstr2inty.Add("sprite_right", 3);
        sprstr2inty.Add("sprite_down_walk_right", 4);
        sprstr2inty.Add("sprite_down_walk_left", 5);
        sprstr2inty.Add("sprite_up_walk_right", 6);
        sprstr2inty.Add("sprite_up_walk_left", 7);
        sprstr2inty.Add("sprite_left_walk_right", 8);
        sprstr2inty.Add("sprite_left_walk_left", 8);
        sprstr2inty.Add("sprite_right_walk_right", 9);
        sprstr2inty.Add("sprite_right_walk_left", 9);

        WorldTilemap.GetComponent<the_world>().loadchunk3x3(intpos.x, intpos.y);

        playertex = new Texture2D(2, 2);
        playertex.LoadImage(File.ReadAllBytes(WorldTilemap.GetComponent<the_world>().texturefolder + "\\BlueNA\\people.png"));
        playertex.filterMode = FilterMode.Point;

        for (int i = 0; i < 51; ++i)
        {
            playersprite.Add(new Sprite[10]);
            for (int j = 0; j < 10; ++j)
            {
                playersprite[i][j] = Sprite.Create(playertex, new Rect(j * 16, playertex.height - (16 + i * 16), 16, 16), new Vector2(0.5f, 0.5f), 16.0f); ;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        //temp character slection. This will be replaced by a menu later. DOn't character select wile the menu is open.
        if (MainTileMenuTrans.position.z > 0)
        {
            if (Input.GetKeyDown("3"))
            {
                --currsprite;
                if (currsprite < 0) currsprite = 50;
                gameObject.GetComponent<SpriteRenderer>().sprite = playersprite[currsprite][sprstr2inty["sprite_" + sprite_direction]];
            }

            if (Input.GetKeyDown("4"))
            {
                ++currsprite;
                if (currsprite > 50) currsprite = 0;
                gameObject.GetComponent<SpriteRenderer>().sprite = playersprite[currsprite][sprstr2inty["sprite_" + sprite_direction]];
            }
        }

        //a step is sceduled:
        if (step_number > 0 && step_time < Time.time)
        {
            step_number = takeStep();
        }

    }

    public void attemptMovement(int direction)
    {
        //draw sprite. if diection is 0, keep current sprite direction.
        if (direction == 1) sprite_direction = "up";
        if (direction == 2) sprite_direction = "right";
        if (direction == 3) sprite_direction = "down";
        if (direction == 4) sprite_direction = "left";
        gameObject.GetComponent<SpriteRenderer>().sprite = playersprite[currsprite][sprstr2inty["sprite_" + sprite_direction]];


        //if we are on a door, do a teleport.
        //value added void loading.
        if (WorldTilemap.GetTile<Tile>(intpos) == null)
        {
            WorldTilemap.GetComponent<the_world>().loadchunk3x3(intpos.x, intpos.y);
        }
        if (WorldTilemap.GetTile<Tile>(intpos).name == "10")
        {
            WorldTilemap.GetComponent<TeleportHandler>().IWannaTeleport();
            //don't move after teleporting.
            direction = 0;
        }

        //setup the steps
        //we're going places
        if (direction != 0)
        {
            //check for unloaded chunks
            WorldTilemap.GetComponent<the_world>().loadchunk3x3(intpos.x, intpos.y);

            //schedule a step
            step_number = 1;
            step_time = Time.time;

            //check if we can walk where we want to go. 5 means we walk, but don't move
            Vector3Int wantpos = intpos;
            if (direction == 1) wantpos.y += 1;
            if (direction == 2) wantpos.x += 1;
            if (direction == 3) wantpos.y -= 1;
            if (direction == 4) wantpos.x -= 1;
            Tile movementtile = WorldTilemap.GetTile<Tile>(wantpos);
            if (movementtile != null)
            {
                the_world.tiledata tiledata = WorldTilemap.GetComponent<the_world>().properties[movementtile];
                if (tiledata.water == true) direction = 5;
                if (tiledata.bump == true) direction = 5;
            }
            else
            {
                direction = 5;
            }

            //move the position where will be.
            if (direction == 1) intpos.y += 1;
            if (direction == 2) intpos.x += 1;
            if (direction == 3) intpos.y -= 1;
            if (direction == 4) intpos.x -= 1;

        }

        //we are not moving.
        if (direction == 0)
        {
            //reset feet
            foot_side_right = true;
        }


        step_direction = direction;
    }

    private int takeStep() {

        //schedule next step.
        step_time += gameObject.GetComponent<ClientController>().walk_delay / smoothment_factor;
        //move
        if (step_direction == 1) transform.position = new Vector3(intpos.x, -1 + intpos.y + step_number / smoothment_factor, 0);
        if (step_direction == 2) transform.position = new Vector3(-1 + intpos.x + step_number / smoothment_factor, intpos.y, 0);
        if (step_direction == 3) transform.position = new Vector3(intpos.x, 1 + intpos.y - step_number / smoothment_factor, 0);
        if (step_direction == 4) transform.position = new Vector3(1 + intpos.x - step_number / smoothment_factor, intpos.y, 0);
        

        //on the second half of the steps, change the sprite to the stepping sprite.
        if (step_number > (smoothment_factor / 2.0f))
        {
            string step_foot = "left";
            if (foot_side_right) step_foot = "right";
            gameObject.GetComponent<SpriteRenderer>().sprite = playersprite[currsprite][sprstr2inty["sprite_" + sprite_direction + "_walk_" + step_foot]];
        }

        //increment the step number or stop when we reach the end.
        if (step_number >= smoothment_factor)
        {
            foot_side_right = !foot_side_right;
            return 0;
        }
        else
        {
            return ++step_number;
        }
    }


}
