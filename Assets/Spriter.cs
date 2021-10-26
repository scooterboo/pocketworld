using System.Collections.Generic;
using UnityEngine;
using SFB;

public class Spriter
{
    private byte[] ROMText;


    // Start is called before the first frame update
    public void Start()
    {

        string romfolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\.pocketworld\\roms";
        System.IO.Directory.CreateDirectory(romfolder);
        if (!System.IO.File.Exists(romfolder + "\\BlueNA.gb"))
        {
            try
            {
                string[] test;
                test = StandaloneFileBrowser.OpenFilePanel("Can you go find me a legally acquired ROM file for English Pokemon Blue?", "", "gb", false);
                if (test.Length == 0) Application.Quit();
                System.IO.File.Copy(test[0], romfolder + "\\BlueNA.gb");
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }


        ROMText = System.IO.File.ReadAllBytes(romfolder + "\\BlueNA.gb");

        string texturefolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\.pocketworld\\textures\\BlueNA";
        System.IO.Directory.CreateDirectory(texturefolder);

        if (!System.IO.File.Exists(texturefolder + "\\people.png"))
        {
            System.IO.File.WriteAllBytes(texturefolder + "\\people.png", makePeopleSpriteSheet());
        }

        if (!System.IO.File.Exists(texturefolder + "\\front.png"))
        {
            System.IO.File.WriteAllBytes(texturefolder + "\\front.png", makePokeSpriteSheet(true));
        }

        if (!System.IO.File.Exists(texturefolder + "\\back.png"))
        {
            System.IO.File.WriteAllBytes(texturefolder + "\\back.png", makePokeSpriteSheet(false));
        }

        if (!System.IO.File.Exists(texturefolder + "\\32x32sheet.png"))
        {
            System.IO.File.WriteAllBytes(texturefolder + "\\32x32sheet.png", make32x32SpriteSheet());
        }

        if (!System.IO.File.Exists(texturefolder + "\\16x16sheet.png"))
        {
            System.IO.File.WriteAllBytes(texturefolder + "\\16x16sheet.png", make16x16SpriteSheet());
        }

        if (!System.IO.File.Exists(texturefolder + "\\menusheet.png"))
        {
            System.IO.File.WriteAllBytes(texturefolder + "\\menusheet.png", makemenu());
        }
        if (!System.IO.File.Exists(texturefolder + "\\menusheet2.png"))
        {
            System.IO.File.WriteAllBytes(texturefolder + "\\menusheet2.png", makemenu2());
        }

    }

    byte[] makePeopleSpriteSheet()
    {
        Texture2D tex = new Texture2D(160, 1024, TextureFormat.RGBA32, false);
        Color[] image;
        int stoppa = 0;
        int incramenta = 0x10000;
        for (int j = 1008; j >= 0; j -= 16)
        {
            for (int i = 0; i < 160; i += 16)
            {
                if (++stoppa > 303) break;
                image = shiftup(GBImage8x8Rip(incramenta));
                tex.SetPixels(0 + i, 8 + j, 8, 8, image, 0);
                image = shiftup(GBImage8x8Rip(incramenta + 16));
                tex.SetPixels(8 + i, 8 + j, 8, 8, image, 0);
                image = shiftup(GBImage8x8Rip(incramenta + 32));
                tex.SetPixels(0 + i, 0 + j, 8, 8, image, 0);
                image = shiftup(GBImage8x8Rip(incramenta + 48));
                tex.SetPixels(8 + i, 0 + j, 8, 8, image, 0);
                if (i >= 32)
                {
                    image = tex.GetPixels(i, j, 16, 16);
                    i += 16;
                    tex.SetPixels(i,j,16, 16, reverse16x16(image), 0);
                }
                incramenta += 64;
                if (stoppa == 78)  incramenta = 0x14000;
                if (stoppa == 207)
                {
                    Color[] arr = new Color[256];
                    for (int k = 0; k < arr.Length; ++k)
                    {
                        arr[k].a = 0f;
                    }
                    for (int k = 1; k < 7; ++k)
                    {
                        tex.SetPixels(k * 16 + i, 0 + j, 16, 16, arr, 0);
                    }
                    break;
                }
            }
            if (stoppa > 303) break;
        }
        return tex.EncodeToPNG();
    }

    Color[] shiftup(Color[] arr)
    {
        Color white = new Color(1f, 1f, 1f);
        Color lgrey = new Color(3 / 4f, 3 / 4f, 3 / 4f);
        for (int i = 0; i < arr.Length; ++i)
        {
            if (arr[i].r == 1f) arr[i].a = 0f;
            if (arr[i].r == 3 / 4f) arr[i] = white;
            if (arr[i].r == 2 / 4f) arr[i] = lgrey;

        }
        return arr;

    }

    Color[] reverse16x16(Color[] arr) {
        Color temp;
        for (int i = 0; i< 256; i += 16) {
            for (int j = 0; j < 8; ++j)
            {
                temp = arr[i + j];
                arr[i + j] = arr[i + 15 - j];
                arr[i + 15 - j] = temp;
             }

        }
        return arr;
    }


    byte[] makePokeSpriteSheet(bool front)
    {
        Texture2D tex = new Texture2D(8 * 16 * 7, 8 * 16 * 7, TextureFormat.RGB24, false);
        Color white = new Color(1f, 1f, 1f);
        Color[] blankout = new Color[8 * 16 * 7 * 8 * 16 * 7];
        for (int i = 0; i < blankout.Length; ++i)
        {
            blankout[i] = white;
        }

        tex.SetPixels(blankout, 0);

        int[] bankarray = new int[191];
        int bank = 0;
        for (int i = 1; i < 191; ++i)
        {
            if (i >= 0x0 && i <= 0x1E) bank = 0x9;
            if (i >= 0x1F && i <= 0x49) bank = 0xA;
            if (i >= 0x4A && i <= 0x73) bank = 0xB;
            if (i >= 0x74 && i <= 0x98) bank = 0xC;
            if (i >= 0x99 && i <= 0xFF) bank = 0xD;
            if (i == 0x15) bank = 0x1; //mew
            bankarray[nbit(8 * (0x41023 + i), 8)] = bank;
        }


        int spritepointerlocation = 0;
        if (front) spritepointerlocation = 0x383CE;
        if (!front) spritepointerlocation = 0x383D0;

        int location;
        int pointerloc;
        int counter = 1;
        for (int j = 15; j >= 0; j--)
        {
            for (int i = 0; i < 16; i++)
            {
                Color[] image;
                int width, height;
                pointerloc = 8 * (spritepointerlocation + (counter * 28));
                location = (nbit(pointerloc, 8) << 8) + nbit(pointerloc - 8, 8);
                location = (bankarray[counter] << 14) + (location & 0x3fff);

                (image, width, height) = PokeSpriteGet(location);
                int offset = 8;
                if (width == 7) offset = 0;


                tex.SetPixels(i * 7 * 8 + offset, j * 7 * 8, width * 8, height * 8, image, 0);
                counter++;
                if (counter > 150) break;
            }
            if (counter > 150) break;
        }

        return tex.EncodeToPNG();
    }


    void mergelist(List<Color[]> add, List<Color[]> dest)
    {
        for (int i = 0; i < add.Count; ++i)
        {
            addtile(add[i], dest);
        }
    }


    bool addtile(Color[] tile, List<Color[]> sheet)
    {
        for (int i = 0; i < sheet.Count; ++i)
        {
            if (comparetile(tile, sheet[i])) return false;
        }
        sheet.Add(tile);
        return true;
    }

    bool comparetile(Color[] tile, Color[] tile2)
    {
        if (tile.Length != tile2.Length) return false;
        for (int i = 0; i < tile2.Length; ++i)
        {
            if (tile[i] != tile2[i]) return false;
        }
        return true;
    }



    byte[] make32x32SpriteSheet()
    {

        List<Color[]> sheet = new List<Color[]>();
        List<Color[]> sheet2 = new List<Color[]>();

        sheet = readmapsprites(0x64000, 0x645E0);
        sheet2 = makeblocks(0x645E0, 0x64DE0, sheet);
        sheet = readmapsprites(0x64DE0, 0x65270); // 0x65270 - 0x6527F is junk
        sheet2.AddRange(makeblocks(0x65280, 0x653A0, sheet));
        sheet = readmapsprites(0x653A0, 0x65980); // 0x65980 - 0x6598F is junk
        sheet2.AddRange(makeblocks(0x65990, 0x65BB0, sheet));
        sheet = readmapsprites(0x65BB0, 0x66190);
        sheet2.AddRange(makeblocks(0x66190, 0x66610, sheet));
        sheet = readmapsprites(0x66610, 0x66BF0);
        sheet2.AddRange(makeblocks(0x66BF0, 0x66D60, sheet));
        sheet = readmapsprites(0x66D60, 0x67350);
        sheet2.AddRange(makeblocks(0x67350, 0x676F0, sheet));
        sheet = readmapsprites(0x676F0, 0x67B50);
        sheet2.AddRange(makeblocks(0x67B50, 0x68000, sheet));

        //0x68000-x6806F is some junk then the "blue version" text 
        sheet = readmapsprites(0x6806F, 0x6866F);
        sheet2.AddRange(makeblocks(0x6866F, 0x68DAF, sheet));
        sheet = readmapsprites(0x68DAF, 0x693AF);
        sheet2.AddRange(makeblocks(0x693AF, 0x695FF, sheet));
        sheet = readmapsprites(0x695FF, 0x69BEF);
        sheet2.AddRange(makeblocks(0x69BEF, 0x6A33F, sheet)); //WTF is 0x6A340-0x6A3EF?
                                                              //sheet2.AddRange(makeblocks(0x69BEF, 0x6A3EF, sheet));
        sheet = readmapsprites(0x6A3EF, 0x6A9EF);
        sheet2.AddRange(makeblocks(0x6A9EF, 0x6B1EF, sheet));
        sheet = readmapsprites(0x6B1EF, 0x6B7EF);
        sheet2.AddRange(makeblocks(0x6B7EF, 0x6C000, sheet));

        sheet = readmapsprites(0x6C000, 0x6C5C0);
        sheet2.AddRange(makeblocks(0x6C5C0, 0x6CCA0, sheet));
        sheet = readmapsprites(0x6CCA0, 0x6D0C0);
        sheet2.AddRange(makeblocks(0x6D0C0, 0x6D8C0, sheet));
        sheet = readmapsprites(0x6D8C0, 0x6DEA0);
        sheet2.AddRange(makeblocks(0x6DEA0, 0x6E390, sheet));
        sheet = readmapsprites(0x6E390, 0x6E930);
        sheet2.AddRange(makeblocks(0x6E930, 0x6ED10, sheet));
        sheet = readmapsprites(0x6ED10, 0x6F2D0);
        sheet2.AddRange(makeblocks(0x6F2D0, 0x6F670, sheet));
        sheet = readmapsprites(0x6F670, 0x6FB20);
        sheet2.AddRange(makeblocks(0x6FB20, 0x6FD60, sheet));
        sheet = readmapsprites(0x6FD60, 0x6FEF0);
        sheet2.AddRange(makeblocks(0x6FEF0, 0x70000, sheet));


        Texture2D tex = new Texture2D(1024, 2048, TextureFormat.RGB24, false);
        int counter = 0;
        for (int j = 63; j >= 0; j--)
        {
            for (int i = 0; i < 32; i++)
            {
                tex.SetPixels(i * 32, j * 32, 32, 32, sheet2[counter++], 0);

                if (counter >= sheet2.Count) break;
            }
            if (counter >= sheet2.Count) break;
        }

        return tex.EncodeToPNG();
        
    }

    byte[] make16x16SpriteSheet()
    {
        List<Color[]> sheet = new List<Color[]>();
        List<Color[]> sheet2 = new List<Color[]>();
        sheet = readmapsprites(0x64000, 0x645E0);
        sheet2 = maketiles(0x645E0, 0x64DE0, sheet);
        sheet = readmapsprites(0x64DE0, 0x65270); // 0x65270 - 0x6527F is junk
        mergelist(maketiles(0x65280, 0x653A0, sheet), sheet2);
        sheet = readmapsprites(0x653A0, 0x65980); // 0x65980 - 0x6598F is junk
        mergelist(maketiles(0x65990, 0x65BB0, sheet), sheet2);
        sheet = readmapsprites(0x65BB0, 0x66190);
        mergelist(maketiles(0x66190, 0x66610, sheet), sheet2);
        sheet = readmapsprites(0x66610, 0x66BF0);
        mergelist(maketiles(0x66BF0, 0x66D60, sheet), sheet2);
        sheet = readmapsprites(0x66D60, 0x67350);
        mergelist(maketiles(0x67350, 0x676F0, sheet), sheet2);
        sheet = readmapsprites(0x676F0, 0x67B50);
        mergelist(maketiles(0x67B50, 0x68000, sheet), sheet2);

        //0x68000-x6806F is some junk then the "blue version" text 
        sheet = readmapsprites(0x6806F, 0x6866F);
        mergelist(maketiles(0x6866F, 0x68DAF, sheet), sheet2);
        sheet = readmapsprites(0x68DAF, 0x693AF);
        mergelist(maketiles(0x693AF, 0x695FF, sheet), sheet2);
        sheet = readmapsprites(0x695FF, 0x69BEF);
        mergelist(maketiles(0x69BEF, 0x6A33F, sheet), sheet2); //WTF is 0x6A340-0x6A3EF?
                                                               //sheet2.AddRange(makeblocks(0x69BEF, 0x6A3EF, sheet));
        sheet = readmapsprites(0x6A3EF, 0x6A9EF);
        mergelist(maketiles(0x6A9EF, 0x6B1EF, sheet), sheet2);
        sheet = readmapsprites(0x6B1EF, 0x6B7EF);
        mergelist(maketiles(0x6B7EF, 0x6C000, sheet), sheet2);

        sheet = readmapsprites(0x6C000, 0x6C5C0);
        mergelist(maketiles(0x6C5C0, 0x6CCA0, sheet), sheet2);
        sheet = readmapsprites(0x6CCA0, 0x6D0C0);
        mergelist(maketiles(0x6D0C0, 0x6D8C0, sheet), sheet2);
        sheet = readmapsprites(0x6D8C0, 0x6DEA0);
        mergelist(maketiles(0x6DEA0, 0x6E390, sheet), sheet2);
        sheet = readmapsprites(0x6E390, 0x6E930);
        mergelist(maketiles(0x6E930, 0x6ED10, sheet), sheet2);
        sheet = readmapsprites(0x6ED10, 0x6F2D0);
        mergelist(maketiles(0x6F2D0, 0x6F670, sheet), sheet2);
        sheet = readmapsprites(0x6F670, 0x6FB20);
        mergelist(maketiles(0x6FB20, 0x6FD60, sheet), sheet2);
        sheet = readmapsprites(0x6FD60, 0x6FEF0);
        mergelist(maketiles(0x6FEF0, 0x70000, sheet), sheet2);

        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        int counter = 0;
        for (int j = 31; j >= 0; j--)
        {
            for (int i = 0; i < 32; i++)
            {
                tex.SetPixels(i * 16, j * 16, 16, 16, sheet2[counter++], 0);

                if (counter >= sheet2.Count) break;
            }
            if (counter >= sheet2.Count) break;
        }

        return tex.EncodeToPNG();

    }

    //make a 3x3 menu that we stretch to show the left menu.
    byte[] makemenu()
    {
        Texture2D tex = new Texture2D(24, 24, TextureFormat.RGB24, false);
        List<Color[]> menu = new List<Color[]>();
        int[] tiles = { 0x10 , 0x20 , 0x30 ,
                        0x40 , 0x70 , 0x40 ,
                        0x50 , 0x20 , 0x60 };
        for (int i = 0; i < 9; ++i)
        {
            menu.Add(GBImage8x8Rip(0x12000 + tiles[i]));
        }

        int counter = 0;

        for (int j = 2; j >= 0; j--)
        {
            for (int i = 0; i < 3; i++)
            {
                tex.SetPixels(i * 8, j * 8, 8, 8, menu[counter++], 0);

                if (counter >= menu.Count) break;
            }
            if (counter >= menu.Count) break;
        }
        return tex.EncodeToPNG();
        
    }

    //make the main tile menu.
    byte[] makemenu2()
    {
        Texture2D tex = new Texture2D(34*8, 30*8, TextureFormat.RGB24, false);
        List<Color[]> menu = new List<Color[]>();
        List<Color[]> menupallet = new List<Color[]>();
        menupallet.Add(combine(GBImage8x8Rip(0x12010), GBImage8x8Rip(0x12030)));
        menupallet.Add(combine(GBImage8x8Rip(0x12030), GBImage8x8Rip(0x12060)));
        menupallet.Add(combine(GBImage8x8Rip(0x12060), GBImage8x8Rip(0x12050)));
        menupallet.Add(combine(GBImage8x8Rip(0x12050), GBImage8x8Rip(0x12010)));

        int[] tiles = { 10 , 20 , 20 , 00 , 20 , 20 , 00 , 20 , 20 , 00 , 20 , 20 , 00 , 20 , 20 , 00 , 20 , 20 , 00 , 20 , 20 , 00 , 20 , 20 , 00 , 20 , 20 , 00 , 20 , 20 , 00 , 20 , 20 , 30 ,
                        40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 , 70 , 70 , 40 ,
                        03 , 20 , 20 , 02 , 20 , 20 , 02 , 20 , 20 , 02 , 20 , 20 , 02 , 20 , 20 , 02 , 20 , 20 , 02 , 20 , 20 , 02 , 20 , 20 , 02 , 20 , 20 , 02 , 20 , 20 , 02 , 20 , 20 , 01 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        40 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 70 , 40 ,
                        50 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 20 , 60 };

        for (int i = 0; i < tiles.Length; ++i)
        {
            if (tiles[i] >= 10)
            {
                menu.Add(GBImage8x8Rip(0x12000 + 16*tiles[i]/10));
            }
            else
            {
                menu.Add(menupallet[tiles[i]]);
            }
        }

        int counter = 0;

        for (int j = 29; j >= 0; j--)
        {
            for (int i = 0; i < 34; i++)
            {
                tex.SetPixels(i * 8, j * 8, 8, 8, menu[counter++], 0);

                if (counter >= menu.Count) break;
            }
            if (counter >= menu.Count) break;
        }
        return tex.EncodeToPNG();

    }

    Color[] combine(Color[] image1, Color[] image2)
    {
        
        int limit;
        if (image1.Length > image2.Length) {
            limit = image2.Length;
        } else {
            limit = image1.Length;
        }
        Color[] returnimg = new Color[limit];


        for (int i = 0; i < limit; ++i)
        {
            if (image1[i].r < image2[i].r)
            {
                returnimg[i] = image1[i];
            }
            else
            {
                returnimg[i] = image2[i];
            }
        }
        return returnimg;
    }







    List<Color[]> makeblocks(int start, int end, List<Color[]>sheet)
    {
        List<Color[]> sheet2 = new List<Color[]>();
        Texture2D tex = new Texture2D(32, 32, TextureFormat.RGB24, false);
        Color[] sprite= new Color[64];
        int incramenta = start;
        do
        {
            for (int j = 24; j >= 0; j -= 8)
            {
                for (int i = 0; i < 32; i += 8)
                {
                    
                    sprite = sheet[ROMText[incramenta++]];
                    tex.SetPixels(i, j, 8, 8, sprite, 0);
                }
            }
            sheet2.Add(tex.GetPixels());
        } while (incramenta < end);
        return sheet2;
    }

    List<Color[]> maketiles(int start, int end, List<Color[]> sheet)
    {
        List<Color[]> sheet2 = new List<Color[]>();
        Texture2D tex = new Texture2D(32, 32, TextureFormat.RGB24, false);
        Color[] sprite = new Color[64];
        int incramenta = start;
        do
        {
            for (int j = 24; j >= 0; j -= 8)
            {
                for (int i = 0; i < 32; i += 8)
                {

                    sprite = sheet[ROMText[incramenta++]];
                    tex.SetPixels(i, j, 8, 8, sprite, 0);
                }
            }
            addtile(tex.GetPixels(0, 16, 16, 16), sheet2);
            addtile(tex.GetPixels(16, 16, 16, 16), sheet2);
            addtile(tex.GetPixels(0, 0, 16, 16), sheet2);
            addtile(tex.GetPixels(16, 0, 16, 16), sheet2);
        } while (incramenta < end);
        return sheet2;
    }


    byte[] make8x8SpriteSheet(List<Color[]> sheet)
    {

        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);

        //todo: replace with single for loop
        int counter = 0;
        for (int j = 31; j >= 0; j--)
        {
            for (int i = 0; i < 32; i++)
            {
                tex.SetPixels(i * 8, j * 8, 8, 8, sheet[counter++], 0);

                if (counter >= sheet.Count) break;
            }
            if (counter >= sheet.Count) break;
        }

        return tex.EncodeToPNG();
        
    }


    List<Color[]> readmapsprites(int start, int end)
    {
        List<Color[]> sheet = new List<Color[]>();
        for (int i = start; i < end; i+=16) {
            sheet.Add(GBImage8x8Rip(i));
        }
        return sheet;
    }







    Color[] GBImage8x8Rip(int location) 
    {
        Color[] output = new Color[64];
        bool p1, p2;
        byte b1, b2, b3;
        b3 = 0;
        Color white = new Color(1f, 1f, 1f);
        Color lgrey = new Color(3 / 4f, 3 / 4f, 3 / 4f);
        Color dgrey = new Color(2 / 4f, 2 / 4f, 2 / 4f);
        Color black = new Color(1 / 4f, 1 / 4f, 1 / 4f);

        for (int j = 14; j >= 0; j -= 2)
        {
            b1 = ROMText[location + j];
            b2 = ROMText[location + 1 + j];
            for (int i = 7; i >= 0; --i)
            {
                p1 = (b2 & (1 << i)) > 0;
                p2 = (b1 & (1 << i)) > 0;

                if (p1 && p2) output[b3++] = black;
                if (p1 && !p2) output[b3++] = dgrey;
                if (!p1 && p2) output[b3++] = lgrey;
                if (!p1 && !p2) output[b3++] = white;
            }
        }
        return output;
    }

    (Color[], int, int) PokeSpriteGet(int location) {
        int bitpos = location * 8;

        int width = nbit(bitpos, 4);
        bitpos += 4;
        int height = nbit(bitpos, 4);
        bitpos += 4;



        int size = width * height * 64;
        if (size == 0)
        {
            Debug.Log("this is a zero size sprite, I'm exiting\n");
            return (null,0,0);
        }

        //start outputing!
        bool[] image = new bool[size];
        bool[] image2 = new bool[size];
        bool primary_buffer = onebit(bitpos++) == 1;

        if (primary_buffer)
        {
            bitpos = readimage(bitpos, image2, height, width);
        }
        else
        {
            bitpos = readimage(bitpos, image, height, width);
        }

        int encoding_mode = 0;
        if (onebit(bitpos++) == 1)
        {
            if (onebit(bitpos++) == 1)
            {
                encoding_mode = 3;
            }
            else
            {
                encoding_mode = 2;
            }
        }
        else
        {
            encoding_mode = 1;
        }

        if (primary_buffer)
        {
            bitpos = readimage(bitpos, image, height, width);
        }
        else
        {
            bitpos = readimage(bitpos, image2, height, width);
        }

        if (primary_buffer)
        {
            if (encoding_mode == 1 || encoding_mode == 3)
            {
                deltadecode(image, height, width);
            }
            deltadecode(image2, height, width);

            if (encoding_mode == 2 || encoding_mode == 3)
            {
                for (int i = 0; i < size; i++)
                {
                    image[i] ^= image2[i];
                }
            }
        }
        else
        {
            if (encoding_mode == 1 || encoding_mode == 3)
            {
                deltadecode(image2, height, width);
            }
            deltadecode(image, height, width);

            if (encoding_mode == 2 || encoding_mode == 3)
            {
                for (int i = 0; i < size; i++)
                {
                    image2[i] ^= image[i];
                }
            }
        }




        Color[] imagec = new Color[size];
        Color white = new Color(1f, 1f, 1f);
        Color lgrey = new Color(3 / 4f, 3 / 4f, 3 / 4f);
        Color dgrey = new Color(2 / 4f, 2 / 4f, 2 / 4f);
        Color black = new Color(1 / 4f, 1 / 4f, 1 / 4f);

        bool p1, p2;
        int b3 = 0;
        for (int i = 0; i < size; i++)
        {
            p1 = image2[i];
            p2 = image[i];


            if (p1 && p2) imagec[b3++] = black;
            if (p1 && !p2) imagec[b3++] = dgrey;
            if (!p1 && p2) imagec[b3++] = lgrey;
            if (!p1 && !p2) imagec[b3++] = white;
        }


        Color[] imagecf = new Color[size];
        for (int j = 0; j < (height * 8); j++)
        {
            int offest = (height * 8) - (j + 1);

            for (int i = 0; i < (width * 8); i++)
            {
                imagecf[(offest * width * 8) + i] = imagec[(j * width * 8) + i];
            }

        }

        return (imagecf, width, height);
    }




    int onebit(int bitpos)
    {
        bool output = (ROMText[bitpos / 8] & (1 << (7 - (bitpos % 8)))) > 0;
        if (output) 
        {
            return 1; 
        }else{ 
            return 0; 
        }
    }

    int nbit(int bitpos, int number)
    {
        int output = 0;
        for (int i = 0; i < number; i++)
        {
            output = (output << 1) + onebit(bitpos++);
        }
        return output;
    }


    int readimage(int bitpos,bool[] image, int height, int width)
    {
        int size = height * width * 64;
        int outputpos = 0;
        int outputadd, bitposadd;


        if (onebit(bitpos++) == 0)
        {
            (outputadd,bitposadd) = zeropacket(bitpos, image, outputpos, height, width);
            outputpos = outputadd;
            bitpos = bitposadd;
        }

        while (true)
        {
            (outputadd, bitposadd) = datapacket(bitpos, image, outputpos, height, width);
            outputpos = outputadd;
            bitpos = bitposadd;
            if (outputpos >= size)
            {
                break;
            }
            (outputadd, bitposadd) = zeropacket(bitpos, image, outputpos, height, width);
            outputpos = outputadd;
            bitpos = bitposadd;
            if (outputpos >= size)
            {
                break;
            }
        }
        return bitpos;
    }

    (int,int) zeropacket(int bitpos, bool[] image,int outputpos, int height, int width)
    {
        int lengthn = 1;
        while (onebit(bitpos++)==1)
        {
            lengthn++;
        }
        int number = (1 << lengthn) + nbit(bitpos, lengthn) - 1;
        bitpos += lengthn;
        int h8 = height * 8;
        int w8 = width * 8;
        int size = h8 * w8;
        int ho;
        for (int i = 0; i < number; i++)
        {
            ho = outputpos / 2;
            image[(ho / h8 * 2) + (ho * w8) % size] = false;
            image[(ho / h8 * 2) + 1 + (ho * w8) % size] = false;
            outputpos += 2;

            if (outputpos >= size)
            {
                break;
            }
        }
        return (outputpos,bitpos);
    }


    (int,int) datapacket(int bitpos, bool[] image, int outputpos, int height, int width)
    {
        int data;
        int h8 = height * 8;
        int w8 = width * 8;
        int size = h8 * w8;
        int ho;
        do
        {
            data = nbit(bitpos, 2);
            bitpos += 2;
            ho = outputpos / 2;
            switch (data)
            {
                case 0:
                    break;
                case 1:
                    image[(ho / h8 * 2) + (ho * w8) % size] = false;
                    image[(ho / h8 * 2) + 1 + (ho * w8) % size] = true;
                    outputpos += 2;
                    break;
                case 2:
                    image[(ho / h8 * 2) + (ho * w8) % size] = true;
                    image[(ho / h8 * 2) + 1 + (ho * w8) % size] = false;
                    outputpos += 2;
                    break;
                case 3:
                    image[(ho / h8 * 2) + (ho * w8) % size] = true;
                    image[(ho / h8 * 2) + 1 + (ho * w8) % size] = true;
                    outputpos += 2;
                    break;
            }
            if (outputpos >= size)
            {
                break;
            }

        } while (data != 0);
        return (outputpos,bitpos);
    }



    void deltadecode(bool[] image, int height, int width)
    {
        bool output;
        for (int i = 0; i < (height * 8); i++)
        {
            output = false;
            for (int j = 0; j < (width * 8); j++)
            {
                output ^= image[(i * width * 8) + j];
                image[(i * width * 8) + j] = output;
            }
        }
        return;
    }

}
