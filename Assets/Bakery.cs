using UnityEngine;

public class Bakery : MonoBehaviour
{

    private string savefolder;

    // Start is called before the first frame update
    void Start()
    {
        savefolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\.pocketworld\\save";
    }

    // Update is called once per frame
    void Update()
    {

    }

    //loads the tiles from a saved chunk. Right now the code is a simple read, but in the future it could be more complex with, for example, transparent tiles.
    public int[] LoadSlice(string loaf, int chunksize, int FloorposX, int FloorposY)
    {
        string filename = savefolder + "\\" + loaf + "\\slice_" + FloorposX / chunksize + "_" + FloorposY / chunksize + ".json";

        if (!System.IO.File.Exists(filename))
        {
            return null;
        }

        int[] data = new int[chunksize * chunksize];
        chunk chunk = JsonUtility.FromJson<chunk>(System.IO.File.ReadAllText(filename));
        for (int i = 0; i < (chunksize * chunksize); ++i)
        {
            byte[] b64byte = System.Convert.FromBase64String(chunk.data.Substring(2 * i, 2) + "A=");//I DO NOT want to know why this works.
            b64byte[0] = (byte)((b64byte[0] * 0x0202020202 & 0x010884422010) % 1023);
            b64byte[1] = (byte)((b64byte[1] * 0x0202020202 & 0x010884422010) % 1023);
            data[i] = (256 * (b64byte[1] & 0x03)) + b64byte[0];
        }
        return data;

    }

    public void SaveSlice(string loaf, int[] data, int chunksize, int FloorposX, int FloorposY)
    {

        //generate the data string
        string b64string = "";
        for (int i = 0; i < (chunksize * chunksize); ++i)
        {
            byte[] barray = System.BitConverter.GetBytes(data[i]);
            barray[0] = (byte)((barray[0] * 0x0202020202 & 0x010884422010) % 1023);
            barray[1] = (byte)((barray[1] * 0x0202020202 & 0x010884422010) % 1023);
            b64string += System.Convert.ToBase64String(barray).Substring(0, 2);
        }

        //verify we got that folder.
        System.IO.Directory.CreateDirectory(savefolder + "\\" + loaf);

        //write to disk
        chunk mychunk = new chunk();
        mychunk.data = b64string;
        string json = JsonUtility.ToJson(mychunk);
        string filename = savefolder + "\\" + loaf + "\\slice_" + FloorposX / chunksize + "_" + FloorposY / chunksize + ".json";
        System.IO.File.WriteAllText(filename, json);
    }

    public void SaveCrumb(string loaf,int crumb, int chunksize,int posx, int posy)
    {
        //read
        int FloorposX = Mathf.FloorToInt(posx / (float)chunksize);
        int FloorposY = Mathf.FloorToInt(posy / (float)chunksize);
        string filename = savefolder + "\\" + loaf + "\\slice_" + FloorposX + "_" + FloorposY + ".json";

        if (!System.IO.File.Exists(filename))
        {
            gameObject.GetComponent<the_world>().loadchunk(posx, posy);
        }


        string json = System.IO.File.ReadAllText(filename);
        chunk mychunk = JsonUtility.FromJson<chunk>(json);
        //modify
        int insertpoint = 2 * (chunksize * (posy - FloorposY * chunksize) + (posx - FloorposX * chunksize));
        byte[] barray = System.BitConverter.GetBytes(crumb);
        barray[0] = (byte)((barray[0] * 0x0202020202 & 0x010884422010) % 1023);
        barray[1] = (byte)((barray[1] * 0x0202020202 & 0x010884422010) % 1023);
        mychunk.data = mychunk.data.Remove(insertpoint, 2);
        mychunk.data = mychunk.data.Insert(insertpoint, System.Convert.ToBase64String(barray).Substring(0, 2));
        //write
        json = JsonUtility.ToJson(mychunk);
        System.IO.File.WriteAllText(filename, json);
    }


}
