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
    public GameObject SideMenu;
    private Tile LeftArrowTile;
    private Tile RightArrowTile;
    private Tilemap ThisTilemap;
    private int CurrTabSet = 0;
    private int CurrTab = 1;
    private List<int> toptabs = new List<int>();
    private List<List<int>> BottomTiles = new List<List<int>>();


    // Start is called before the first frame update
    void Start()
    {

        ThisTilemap = gameObject.GetComponent<Tilemap>();
        tiles = WorldTilemap.GetComponent<the_world>().tiles;

        LeftArrowTile = ScriptableObject.CreateInstance<Tile>();
        RightArrowTile = ScriptableObject.CreateInstance<Tile>();
        LeftArrowTile.sprite = LeftArrow;
        RightArrowTile.sprite = RightArrow;
        LeftArrowTile.name = "-1";
        RightArrowTile.name = "-2";

        //-1 is left arrow, -2 is right arrow.
        //the tiles I have sorted through already:
        toptabs.AddRange(new List<int> { -1, 10, 13, 18, 20, 32, 14, 25, 15, 343, -2 });
        toptabs.AddRange(new List<int> { -1, 139, 119, 135, 125, 122, 123, 128, 342, 193, -2 });
        toptabs.AddRange(new List<int> { -1, 132, 222, 255, 445, 456, 274, 805, 827, 6, -2 });

        //TODO: make this a json
        BottomTiles.Add(new List<int>());//-1
        BottomTiles.Add(new List<int> { 1, 2, 3, 4, 7, 8, 9, 10, 11, 12, 23, 24, 26, 27, 28, 29, 30, 31, 33, 34, 35, 36, 37, 38, 47, 48, 49, 50, 69, 71, 72, 73, 74, 75, 78, 79, 80, 81, 82, 83, 101, 102, 103, 104, 105, 106, 109, 110, 111, 112, 331, 332, 335, 333, 334, 336, 311, 312, 314, 318, 326, 316, 325, 317, 319, 324, 320, 321, 489, 490, 491, 492, 493, 494, 495, 496 });//10 house parts
        BottomTiles.Add(new List<int> { 13, 42, 43, 44, 45, 46, 65, 66, 113, 213, 214, 242, 243, 306, 307, 308, 309, 310, 329, 330, 347, 506, 507, 518, 527, 535, 573, 574, 575, 576, 577 });//13 water
        BottomTiles.Add(new List<int> { 16, 17, 18, 56, 57, 58, 59, 63, 64, 76, 77, 84, 296, 297, 298, 299, 301, 303, 304, 327, 328, 508, 528, 509, 529, 511, 512, 513, 514, 515, 516, 517, 536, 510, 537, 594, 595, 596, 597, 599, 600, 601, 530, 538, 531, 602, 603, 604, 605, 606, 607, 608, 609, 610, 615, 616 });//18 cliffs
        BottomTiles.Add(new List<int> { 20, 39, 40, 67, 87, 88, 91, 93, 95, 96 });//20 ledges
        BottomTiles.Add(new List<int> { 480, 481, 32, 366, 70, 305, 485, 486, 6, 6, 482, 483 });//32 trees
        BottomTiles.Add(new List<int> { 14, 22, 41, 244, 217, 218, 487, 488, 614, 747 });//14 man made obsticles
        BottomTiles.Add(new List<int> { 6, 294, 315, 300, 5, 21, 116, 114, 115, 338, 302, 363, 362, 361, 19, 107, 108, 25, 337, 477, 478, 479, 484, 497, 498, 499, 502, 503, 504, 505 });//25 natural ground cover in sparse to full order
        BottomTiles.Add(new List<int> { 15, 51, 52, 53, 54, 55, 60, 61, 62, 68, 85, 86, 89, 90, 92, 94, 97, 98, 99, 100, 523, 524, 525, 526, 519, 521, 520, 522, 212, 215, 216 });//15 unnatural ground cover
        BottomTiles.Add(new List<int> { 295, 343, 532, 542, 598, 618 });//343 rocks
        BottomTiles.Add(new List<int>());//-2
        BottomTiles.Add(new List<int>());//-1
        BottomTiles.Add(new List<int> { 139, 140, 189, 190, 170, 171, 407, 408, 545, 546, 724, 725 });//139 indoor plants
        BottomTiles.Add(new List<int> { 118, 119, 120, 121, 133, 134, 564, 136, 138, 141, 142, 149, 150, 155, 157, 197, 198, 199, 200, 211, 382, 383, 385, 387, 386, 364, 365, 557, 558, 559, 560, 169, 626, 627, 629, 647, 648, 649, 650, 654, 670, 671, 687, 688, 689, 690, 691, 692, 693, 694, 695, 696, 697, 698, 699, 705, 706, 707, 708, 709, 712, 713, 714, 715, 710, 711, 716, 721, 728, 729, 736, 737, 738, 755, 756 });//119 tables
        BottomTiles.Add(new List<int> { 127, 130, 135, 137, 152, 182, 183, 184, 185, 421, 245, 246, 423, 422, 391, 392, 393, 394, 380, 381, 410, 586, 587, 588, 589, 592, 624, 625, 630, 722, 723, 780, 781 });//135 TV Typewriters http://catb.org/~esr/jargon/html/tv-typewriters.html
        BottomTiles.Add(new List<int> { 124, 125, 143, 144, 146, 147, 188, 204, 205, 208, 209, 351, 437, 403, 404, 412, 413, 414, 415, 416, 417, 568, 632, 633, 634, 635, 640, 644, 660, 700, 702, 719 });//125 bookcases
        BottomTiles.Add(new List<int> { 122, 126, 131, 181, 145, 151, 153, 154, 156, 167, 174, 194, 195, 196, 202, 203, 453, 468, 272, 280, 284, 287, 348, 355, 349, 350, 357, 339, 384, 409, 389, 406, 440, 553, 566, 569, 585, 622, 623, 636, 637, 645, 651, 652, 655, 657, 658, 659, 663, 664, 661, 662, 665, 666, 667, 668, 675, 678, 673, 674, 677, 680, 681, 717, 720, 742, 743, 744, 773, 767, 769, 770, 820, 817, 821, 822, 823, 824, 825, 826 });//122 walls
        BottomTiles.Add(new List<int> { 117, 123, 158, 159, 160, 161, 162, 163, 164, 165, 166, 168, 173, 176, 179, 207, 241, 257, 259, 293, 340, 346, 360, 367, 368, 369, 378, 379, 411, 390, 345, 452, 352, 467, 427, 539, 555, 578, 581, 582, 583, 590, 593, 612, 613, 619, 620, 621, 628, 631, 646, 669, 672, 676, 679, 682, 683, 684, 685, 686, 701, 703, 704, 718, 726, 730, 771, 772, 774, 775, 779, 808, 809, 816, 818 });//123 floor
        BottomTiles.Add(new List<int> { 128, 129, 172, 191, 285, 388, 517, 533, 534, 556, 579, 611, 617, 819 });//128 stairs
        BottomTiles.Add(new List<int> { 322, 342, 441, 443, 313, 500, 565, 567, 591, 6, 323, 344, 442, 6, 6, 501 });//342 statues
        BottomTiles.Add(new List<int> { 186, 187, 192, 175, 177, 178, 180, 193, 210, 281, 282, 283, 286, 288, 289, 290, 291, 292, 353, 354, 356, 358, 359, 370, 371, 372, 373, 374, 375, 376, 377, 562, 563, 571, 572, 549, 584, 432, 439, 433, 438, 465, 420, 435, 436, 434, 419, 418, 398, 395, 396, 424, 425, 426, 428, 397, 547, 548, 550, 551, 552, 554, 201, 206, 570, 580, 638, 639, 641, 642, 643, 653, 656, 752, 753, 754, 768, 776, 777, 778 });//193 partitions
        BottomTiles.Add(new List<int>());//-2
        BottomTiles.Add(new List<int>());//-1
        BottomTiles.Add(new List<int> { 132, 148 , 258, 540, 429, 430, 475, 341, 541, 543, 544, 476, 405, 431, 561, 727, 766 });//132 rugs
        BottomTiles.Add(new List<int> { 214, 214, 214, 214, 214, 223, 214, 214, 214, 214, 214, 219, 220, 221, 222, 224, 225, 226, 227, 214, 214, 228, 229, 232, 233, 236, 236, 237, 238, 214, 214, 230, 231, 234, 235, 235, 235, 239, 240, 214 });//222 boat
        BottomTiles.Add(new List<int> { 131, 247, 248, 251, 122, 122, 247, 248, 251, 131, 131, 249, 250, 252, 253, 253, 249, 250, 252, 131, 131, 254, 255, 256, 257, 257, 254, 255, 256, 131, 131, 731, 732, 734, 731, 734, 739, 719, 719, 131, 131, 568, 733, 735, 568, 740, 741, 568, 568, 131 });//255 teleporter/lab equipment
        BottomTiles.Add(new List<int> { 448, 445, 444, 446, 450, 6, 6, 6, 6, 6, 449, 444, 445, 447, 451 });//445 window
        BottomTiles.Add(new List<int> { 459, 461, 462, 469, 454, 455, 456, 748, 749, 6, 460, 463, 464, 470, 457, 458, 466, 750, 751, 6, 471, 472, 473, 474, 757, 758, 758, 758, 758, 759, 460, 463, 464, 470, 745, 199, 199, 199, 199, 746, 6, 6, 6, 6, 760, 762, 762, 762, 762, 764, 6, 6, 6, 6, 761, 763, 763, 763, 743, 765 });//456 museum pieces
        BottomTiles.Add(new List<int> { 260, 261, 263, 264, 267, 268, 271, 6, 6, 6, 197, 262, 265, 266, 269, 270, 273, 6, 6, 6, 274, 199, 199, 277, 399, 400, 6, 6, 6, 6, 275, 276, 278, 279, 401, 402 });//274 misc objects
        BottomTiles.Add(new List<int> { 801, 340, 810, 811, 790, 791, 810, 811, 340, 801, 802, 427, 427, 427, 427, 427, 427, 427, 427, 802, 427, 427, 427, 782, 784, 784, 787, 427, 427, 427, 427, 427, 782, 783, 6, 6, 788, 789, 427, 427, 427, 812, 813, 793, 804, 805, 793, 814, 815, 427, 427, 427, 794, 795, 6, 6, 799, 800, 427, 427, 785, 786, 427, 794, 803, 803, 800, 427, 792, 798, 796, 797, 427, 427, 427, 427, 427, 427, 806, 807 });//805 cable club
        BottomTiles.Add(new List<int> { 827 });//827 current multi-tiles
        //don't need to fill out the rest

        //If I ever want to bring back the full menu
        /*
        BottomTiles.Add(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 });//1
        BottomTiles.Add(new List<int> { 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160 });//81
        BottomTiles.Add(new List<int> { 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240 });//161
        BottomTiles.Add(new List<int> { 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 316, 317, 318, 319, 320 });//241
        BottomTiles.Add(new List<int> { 321, 322, 323, 324, 325, 326, 327, 328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344, 345, 346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 370, 371, 372, 373, 374, 375, 376, 377, 378, 379, 380, 381, 382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 400 });//321
        BottomTiles.Add(new List<int> { 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 428, 429, 430, 431, 432, 433, 434, 435, 436, 437, 438, 439, 440, 441, 442, 443, 444, 445, 446, 447, 448, 449, 450, 451, 452, 453, 454, 455, 456, 457, 458, 459, 460, 461, 462, 463, 464, 465, 466, 467, 468, 469, 470, 471, 472, 473, 474, 475, 476, 477, 478, 479, 480 });//401
        BottomTiles.Add(new List<int> { 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 495, 496, 497, 498, 499, 500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 510, 511, 512, 513, 514, 515, 516, 517, 518, 519, 520, 521, 522, 523, 524, 525, 526, 527, 528, 529, 530, 531, 532, 533, 534, 535, 536, 537, 538, 539, 540, 541, 542, 543, 544, 545, 546, 547, 548, 549, 550, 551, 552, 553, 554, 555, 556, 557, 558, 559, 560 });//481
        BottomTiles.Add(new List<int> { 561, 562, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 574, 575, 576, 577, 578, 579, 580, 581, 582, 583, 584, 585, 586, 587, 588, 589, 590, 591, 592, 593, 594, 595, 596, 597, 598, 599, 600, 601, 602, 603, 604, 605, 606, 607, 608, 609, 610, 611, 612, 613, 614, 615, 616, 617, 618, 619, 620, 621, 622, 623, 624, 625, 626, 627, 628, 629, 630, 631, 632, 633, 634, 635, 636, 637, 638, 639, 640 });//561
        BottomTiles.Add(new List<int> { 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 656, 657, 658, 659, 660, 661, 662, 663, 664, 665, 666, 667, 668, 669, 670, 671, 672, 673, 674, 675, 676, 677, 678, 679, 680, 681, 682, 683, 684, 685, 686, 687, 688, 689, 690, 691, 692, 693, 694, 695, 696, 697, 698, 699, 700, 701, 702, 703, 704, 705, 706, 707, 708, 709, 710, 711, 712, 713, 714, 715, 716, 717, 718, 719, 720 });//641
        BottomTiles.Add(new List<int>());//-2
        BottomTiles.Add(new List<int>());//-1
        BottomTiles.Add(new List<int> { 721, 722, 723, 724, 725, 726, 727, 728, 729, 730, 731, 732, 733, 734, 735, 736, 737, 738, 739, 740, 741, 742, 743, 744, 745, 746, 747, 748, 749, 750, 751, 752, 753, 754, 755, 756, 757, 758, 759, 760, 761, 762, 763, 764, 765, 766, 767, 768, 769, 770, 771, 772, 773, 774, 775, 776, 777, 778, 779, 780, 781, 782, 783, 784, 785, 786, 787, 788, 789, 790, 791, 792, 793, 794, 795, 796, 797, 798, 799, 800 });//721
        BottomTiles.Add(new List<int> { 801, 802, 803, 804, 805, 806, 807, 808, 809, 810, 811, 812, 813, 814, 815, 816, 817, 818, 819, 820, 821, 822, 823, 824, 825, 826 });//801
        */

        UpdateTopTabs();
        UpdateBottomTiles();
    }

    // Update is called once per frame
    void Update()
    {
        //if the mouse gets clicked and the menu is up.
        if (Input.GetMouseButtonDown(0) && gameObject.transform.position.z < 0)
        {
            Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 ppos = gameObject.transform.parent.position;

            //if we are not on the left bar
            if (mpos.x - ppos.x > -0.5f)
            {
                //top menu
                if (mpos.y - ppos.y > 1.25)
                {
                    //find what top menu tab the player is clicking.
                    int x = Mathf.FloorToInt(.1875f + (mpos.x - ppos.x) / 1.5f);
                    if (x > 10) x = 10;
                    if (x < 0) x = 0;
                    int TabTile = int.Parse(ThisTilemap.GetTile<Tile>(new Vector3Int(x, 1, 0)).name);

                    //-1 is left arrow, -2 is right arrow. update if you click an arrow
                    //left arrow
                    if (TabTile == -1)
                    {
                        CurrTabSet -= 11;
                        UpdateTopTabs();
                    }
                    //right arrow
                    else if (TabTile == -2)
                    {
                        CurrTabSet += 11;
                        UpdateTopTabs();
                    }
                    //a blank
                    else if (TabTile == 6)
                    {
                        //do nothing
                    }
                    //an actual tab
                    else
                    {
                        CurrTab = x + CurrTabSet;
                        UpdateBottomTiles();
                    }
                }
                else
                //lower menu
                {
                    int x = Mathf.FloorToInt(.1875f + (mpos.x - ppos.x) / 1.5f);
                    int y = Mathf.FloorToInt(.1875f + (mpos.y - ppos.y) / 1.5f);
                    if (x > 9) x = 9;
                    if (x < 0) x = 0;
                    if (y < -7) x = -7;
                    if (y > 0) y = 0;
                    //move tile to side bar
                    SideMenu.GetComponent<menu>().updatetile(int.Parse(ThisTilemap.GetTile<Tile>(new Vector3Int(x, y, 0)).name));
                }

            }
        }
    }

    private void UpdateTopTabs()
    {
        if (CurrTabSet < 0) CurrTabSet = toptabs.Count - 11;
        if (CurrTabSet >= toptabs.Count) CurrTabSet = 0;


        for (int i = 0; i < 11; ++i)
        {
            if (toptabs[i + CurrTabSet] == -1)
            {
                ThisTilemap.SetTile(new Vector3Int(i, 1, 0), LeftArrowTile);
                continue;
            }
            if (toptabs[i + CurrTabSet] == -2)
            {
                ThisTilemap.SetTile(new Vector3Int(i, 1, 0), RightArrowTile);
                continue;
            }
            ThisTilemap.SetTile(new Vector3Int(i, 1, 0), tiles[toptabs[i + CurrTabSet]]);
        }
    }

    private void UpdateBottomTiles()
    {
        for (int i = 0; i < BottomTiles[CurrTab].Count; ++i)
        {
            ThisTilemap.SetTile(new Vector3Int(i % 10, -(i / 10), 0), tiles[BottomTiles[CurrTab][i]]);
        }
        for (int i = BottomTiles[CurrTab].Count; i < 80; ++i)
        {
            ThisTilemap.SetTile(new Vector3Int(i % 10, -(i / 10), 0), tiles[6]); //blank out the rest
        }
    }
}
