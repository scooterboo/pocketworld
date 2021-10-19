using UnityEngine;


public class perlin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public int[] mapit(string worldID, int chunksize, int posX, int posY)
    {
        int[,] map = new int[chunksize + 2, chunksize + 2];
        int[] map2 = new int[chunksize * chunksize];
        if (worldID == "loaf-1")
        {


            for (int i = 0; i < (chunksize + 2); ++i)
            {
                for (int j = 0; j < (chunksize + 2); ++j)
                {


                    float fsample = fivevalueperlin(j + (float)posX - 1, i + (float)posY - 1, 20f, 2f, 5f, 8.5f, 4.5f, 4f);
                    //land
                    if (fsample > .692f)
                    {
                        map[j, i] = 6;
                    }
                    else if (fsample > .612f)
                    {
                        map[j, i] = 5;
                    }
                    else if (fsample > .553f)
                    {
                        map[j, i] = 21;
                    }
                    else if (fsample > .5f)
                    {
                        map[j, i] = 116;
                    }
                    else if (fsample > .447f)
                    {
                        map[j, i] = 114;
                    }
                    else if (fsample > .388f)
                    {
                        map[j, i] = 115;
                    }
                    else if (fsample > .308f)
                    {
                        map[j, i] = 19;
                    }
                    else
                    {
                        if (Random.value > .5f)
                        {
                            map[j, i] = 107;
                        }
                        else
                        {
                            map[j, i] = 108;
                        }
                    }


                    fsample = fivevalueperlin(j + (float)posX + 1000f - 1, i + (float)posY + 1000f - 1, 20f, 2f, 3f, 8.5f, 11f, 8f);
                    //trees
                    if (fsample > .53)
                    {
                        map[j, i] = 32;
                    }
                    else if (fsample > .525)
                    {
                        map[j, i] = 70;
                    }

                    //water
                    fsample = fivevalueperlin(j + (float)posX + 2000f - 1, i + (float)posY + 2000f - 1, 20f, 1.8f, 6.3f, 20f, 20f, 20f);
                    if (fsample > .58)
                    {
                        map[j, i] = 13; //water
                    }
                    //cliffs
                    fsample = fivevalueperlin(j + (float)posX - 2000f - 1, i + (float)posY - 2000f - 1, 20f, 1.6f, 6f, 25f, 25f, 25f);
                    if (fsample > .58)
                    {
                        map[j, i] = 16; //cliff top
                    }

                    //sprite[j + (i * 100)] = new Color(fsample, fsample, fsample);
                }
            }


            for (int i = 0; i < (chunksize * chunksize); ++i)
            {
                processtiletransform(map, (i % chunksize) + 1, (i / chunksize) + 1);
            }

            
            for (int i = 0; i < (chunksize * chunksize); ++i)
            {
                map2[i] = processtilevisual(map, (i % chunksize) + 1, (i / chunksize) + 1);
            }
            return map2;
        }


        for (int i = 0; i < (chunksize + 2); ++i)
        {
            for (int j = 0; j < (chunksize + 2); ++j)
            {
                map[j, i] = 131;

            } 
        }

        for (int i = 0; i < (chunksize * chunksize); ++i)
        {
            map2[i] = map[(i % chunksize) + 1, (i / chunksize) + 1];
        }
        return map2;


    }



    float fivevalueperlin(float x, float y, float s, float a1, float a2, float a3, float a4, float a5)
    {
        float multiplier = (1 / a1 + 1 / a2 + 1 / a3 + 1 / a4 + 1 / a5);
        float output = Mathf.PerlinNoise(x / s, y / s) / a1;
        output += Mathf.PerlinNoise(50f + 2f * x / s, 70f + 2f * y / s) / a2;
        output += Mathf.PerlinNoise(90f + 4f * x / s, 30f + 4f * y / s) / a3;
        output += Mathf.PerlinNoise(20f + 8f * x / s, 10f + 8f * y / s) / a4;
        output += Mathf.PerlinNoise(40f + 16f * x / s, 60f + 16f * y / s) / a5;


        return output / multiplier;
    }

    //each tile needs processing to look pretty before going onto the tilemap. 
    //Need to look in a 3x3 area around the tile to figure out which tile goes on the map.
    void processtiletransform(int[,] data, int posX, int posY)
    {
        //cliff sides.
        if (data[posX, posY] != 16)
        {
            bool left = (data[posX - 1, posY] == 16);
            bool up = (data[posX, posY + 1] == 16);
            bool right = (data[posX + 1, posY] == 16);
            if (!left && !up && !right)
            {
                //0x0x0
                bool upleft = data[posX - 1, posY + 1] == 16;
                bool upright = data[posX + 1, posY + 1] == 16;
                if (!upleft && !upright)
                {
                    //00000
                    //nothing. I do this here so nothing passes through fast.

                }
                else if (upleft && !upright)
                {
                    //01000
                    data[posX, posY] = 59;
                }
                else if (!upleft && upright)
                {
                    //00010
                    data[posX, posY] = 57;
                }
                else
                {
                    //01010 = unknown
                    //data[posX, posY] = xx;
                    data[posX, posY] = 18;

                }

            }
            else if (left && !up && !right)
            {
                //1x0x0
                bool upleft = data[posX - 1, posY + 1] == 16;
                bool upright = data[posX + 1, posY + 1] == 16;
                if (!upleft && !upright)
                {
                    //10000
                    data[posX, posY] = 77;
                }
                else if (upleft && !upright)
                {
                    //11000
                    data[posX, posY] = 58;
                }
                else if (!upleft && upright)
                {
                    //10010 = unknown
                    //data[posX, posY] = xx;
                    data[posX, posY] = 18;

                }
                else
                {
                    //11010 = unknown
                    //data[posX, posY] = xx;
                    data[posX, posY] = 18;

                }
            }
            else if (!left && up && !right)
            {
                //0x1x0
                data[posX, posY] = 18;
            }
            else if (left && up && !right)
            {
                //1x1x0
                data[posX, posY] = 64;
            }
            else if (!left && !up && right)
            {
                //0x0x1
                bool upleft = data[posX - 1, posY + 1] == 16;
                bool upright = data[posX + 1, posY + 1] == 16;
                if (!upleft && !upright)
                {
                    //00001
                    data[posX, posY] = 84;
                }
                else if (upleft && !upright)
                {
                    //01001 = unknown
                    //data[posX, posY] = xx;
                    data[posX, posY] = 18;

                }
                else if (!upleft && upright)
                {
                    //00011
                    data[posX, posY] = 56;
                }
                else
                {
                    //01011 = unknown
                    //data[posX, posY] = xx;
                    data[posX, posY] = 18;

                }
            }
            else if (!left && up && right)
            {
                //0x1x1
                data[posX, posY] = 63;
            }
            else
            {
                //many wierd ones here.
                //data[posX, posY] = xx;
                data[posX, posY] = 18;
            }
        }
        return;
    }

    //each tile needs processing to look pretty before going onto the tilemap. 
    //Need to look in a 3x3 area around the tile to figure out which tile goes on the map.
    int processtilevisual(int[,] data, int posX, int posY)
    {
        int output = data[posX, posY];

        //cliff tops
        if (output == 16)
        {
            if (data[posX, posY + 1] != 16)
            {
                output = 76;
            }
        }
        //water
        else if (output == 13)
        {

            //water is based on the 4 blocks immidiately adjacent.
            bool left = (data[posX - 1, posY] == 13);
            bool up = (data[posX, posY + 1] == 13);
            bool right = (data[posX + 1, posY] == 13);


            if (left && up && right)
            {
                bool upleft = data[posX - 1, posY + 1] == 13;
                bool upright = data[posX + 1, posY + 1] == 13;
                if (upleft && upright)
                {
                    //do nothing
                    return output;
                }
                else if (!upleft && upright)
                {
                    output = 66;
                }
                else if (upleft && !upright)
                {
                    output = 65;
                }
                else if (!upleft && !upright)
                {
                    output = 43;
                }

            }
            else if (!left && !up && !right)
            {
                output = 64;
            }
            else if (!left && up && !right)
            {
                output = 58;
            }
            else if (!left && !up && right)
            {
                output = 42;
            }
            else if (!left && up && right)
            {
                bool upright = data[posX + 1, posY + 1] == 13;
                if (upright)
                {
                    output = 44;
                }
                else
                {
                    output = 42;
                }
            }
            else if (left && !up && !right)
            {
                output = 45;
            }
            else if (left && up && !right)
            {
                bool upleft = data[posX - 1, posY + 1] == 13;
                if (upleft)
                {
                    output = 46;
                }
                else
                {
                    output = 45;
                }
            }
            else if (left && !up && right)
            {
                output = 43;
            }
        }
        return output;
    }


    public house generateInterior(int posx, int posy)
    {
        //we need to find a unique seed for each house's coordinates.
        //so first we turn nagative integers into positive integers:
        long lposx = posx;
        long lposy = posy;


        if (lposx < 0)
        {
            lposx = -2 * lposx - 1;
        }
        else
        {
            lposx *= 2;
        }
        if (lposy < 0)
        {
            lposy = -2 * lposy - 1;
        }
        else
        {
            lposy *= 2;
        }

        //then we use the Cantor pairing function
        int seed = (int)((lposx + lposy) * (lposx + lposy + 1) / 2 + lposy);


        //now this seed is unique. Someday, I'll replace this with a hash function
        System.Random rand = new System.Random(seed);
        int height = rand.Next(5, 12)+1;
        int width = rand.Next(5, 12);
        int doorx = width / 2;

        int[] data = new int[height* width];
        //make the door
        for (int j = 0; j < width; ++j)
        {
            if (j == doorx)
            {
                data[j] = 10;
            }
            else
            {
                data[j] = 131;
            }
        }

        for (int i = 1; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                int input = 123;
                if (i == height -1)
                {
                    input = 122;
                }
                data[i * width + j] = input;
            }
        }

        house house = new house(width,height,doorx,0,data);
        return house;
    }

}
