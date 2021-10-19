using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    public float walk_delay = 0.3f;
    public bool choppy_movement = true;
    private float smoothment_factor;
    private string sprite_direction = "down";
    private float time = 0;
    private float step_time = 0;
    public Vector3 pos;
    private int step_number =0;
    private int direction = 0; //NESW is 1234
    private int step_direction = 0; //NESW is 1234
    private Dictionary<string, int> sprstr2inty = new Dictionary<string, int>();
    private bool foot_side_right = true;
    public Tilemap WorldTilemap;
    public Tilemap SelectorTilemap;
    private Texture2D playertex;
    private int currsprite = 47;
    private List<Sprite[]> playersprite = new List<Sprite[]>();


    //public CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        //set up those inital values
        pos = transform.position;
        time = Time.time;
        step_time = time;

        if (choppy_movement) { smoothment_factor = 1.0f / 8.0f; }
        else { smoothment_factor = 1.0f / 16.0f; }

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


        WorldTilemap.GetComponent<the_world>().loadchunk3x3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

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
        //if you press a direction, you go that direction.
        if (Input.GetKeyDown("up") || Input.GetKeyDown("w")) direction = 1;
        if (Input.GetKeyDown("right") || Input.GetKeyDown("d")) direction = 2;
        if (Input.GetKeyDown("down") || Input.GetKeyDown("s")) direction = 3;
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a")) direction = 4;
        if (Input.GetKeyDown("3"))
        {
            --currsprite;
            if (currsprite < 0) currsprite = 50;
            gameObject.GetComponent<SpriteRenderer>().sprite = playersprite[currsprite][sprstr2inty["sprite_" + sprite_direction]];
        }

        if (Input.GetKeyDown("4"))
        {
            ++currsprite;
            if (currsprite >50) currsprite = 0;
            gameObject.GetComponent<SpriteRenderer>().sprite = playersprite[currsprite][sprstr2inty["sprite_" + sprite_direction]];
        }





        //conceptually, think of this as the end of a step/start of a new one.
        if (time < Time.time - walk_delay)
        {

            //read held directions
            if (Input.GetKey("up") || Input.GetKey("w")) direction = 1;
            if (Input.GetKey("right") || Input.GetKey("d")) direction = 2;
            if (Input.GetKey("down") || Input.GetKey("s")) direction = 3;
            if (Input.GetKey("left") || Input.GetKey("a")) direction = 4;

            //draw sprite
            if (direction == 1) sprite_direction = "up";
            if (direction == 2) sprite_direction = "right";
            if (direction == 3) sprite_direction = "down";
            if (direction == 4) sprite_direction = "left";
            gameObject.GetComponent<SpriteRenderer>().sprite = playersprite[currsprite][sprstr2inty["sprite_" + sprite_direction]];


            //value added position snapping
            pos.y = Mathf.Round(pos.y);
            pos.x = Mathf.Round(pos.x);
            transform.position = pos;

            //value added time snapping
            time = Mathf.Round(Time.time / walk_delay) * walk_delay;

            if (WorldTilemap.GetComponent<Tilemap>().GetTile<Tile>(new Vector3Int((int)pos.x, (int)pos.y, 0)).name == "10")
            {
                WorldTilemap.GetComponent<TeleportHandler>().IWannaTeleport();
                //value added halting
                direction = 0;
            }

            //setup the steps
            //we're going
            if (direction != 0)
            {
                //check for unloaded chunks
                WorldTilemap.GetComponent<the_world>().loadchunk3x3(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));


                step_time = time + (walk_delay * smoothment_factor);
                if (step_number > 1) foot_side_right = !foot_side_right;
                step_number = 1;

                Vector3Int wantpos = new Vector3Int((int)pos.x, (int)pos.y, 0);
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


            }
            //we stopped.
            if (direction == 0)
            {
                step_number = 0;
                foot_side_right = true;
            }

            step_direction = direction;
            direction = 0;
            SelectorTilemap.GetComponent<SelectorGrid>().UpdateSprite();
        }

        //a step is sceduled:
        if (step_time < Time.time && step_direction != 0)
        {
            step_number++;
            if (step_direction == 1) pos.y += smoothment_factor;
            if (step_direction == 2) pos.x += smoothment_factor;
            if (step_direction == 3) pos.y -= smoothment_factor;
            if (step_direction == 4) pos.x -= smoothment_factor;

            transform.position = pos;
            step_time = time + step_number * (walk_delay * smoothment_factor);
            string step_foot = "left";
            if (foot_side_right) step_foot = "right";
            if (step_number > (1 / (smoothment_factor * 2.0f))) gameObject.GetComponent<SpriteRenderer>().sprite = playersprite[currsprite][sprstr2inty["sprite_" + sprite_direction + "_walk_" + step_foot]];

        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

}
