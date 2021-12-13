using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saving
{
    private string savefolder;
    // Start is called before the first frame update
    public void Start()
    {
        savefolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\.pocketworld\\save";
        //verify really quickly that we have that folder.
        System.IO.Directory.CreateDirectory(savefolder);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public (Vector3Int position, int[] sprite, int[] tiles) loadplayer()
    {
        string filename = savefolder + "\\player.json";
        if (!System.IO.File.Exists(filename))
        {
            saveplayer(new Vector3Int(0, 0, 0), new int[] { 47, 0 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        }
        Player myplayer = JsonUtility.FromJson<Player>(System.IO.File.ReadAllText(filename));
        
        return (myplayer.position, myplayer.sprite, myplayer.tiles);
    }

    public void saveplayer(Vector3Int position, int[] sprite, int[] tiles)
    {
        Player myplayer = new Player();
        myplayer.position = position;
        myplayer.sprite = sprite;
        myplayer.tiles = tiles;
        string json = JsonUtility.ToJson(myplayer);
        string filename = savefolder + "\\player.json";
        System.IO.File.WriteAllText(filename, json);


    }

    [System.Serializable]
    public class Player
    {
        public Vector3Int position;
        public int[] sprite;
        public int[] tiles;
    }
}
