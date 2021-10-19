using UnityEngine;
using UnityEngine.Tilemaps;

public class select : MonoBehaviour
{
    public Tilemap WorldTilemap;
    public Tilemap MenuTilemap;
    public Tilemap SelectorTilemap;
    public Transform menuselector;
    private Vector3 oldpos;
    private Vector3 pos;


    // Start is called before the first frame update
    void Start()
    {
        pos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lpos = gameObject.transform.localPosition;
        Vector3 ppos = gameObject.transform.parent.parent.position;

        //make mouseing over the manu make the world placing slector disappear.
        if (mpos.x - ppos.x > -7.5f)
        {
            lpos.z = 0f;
        }
        else
        {
            lpos.z = 3.5f;
        }
        gameObject.transform.localPosition = lpos;


        float xdif = ppos.x - mpos.x;
        float ydif = ppos.y - mpos.y;
        float squareddist = (xdif * xdif) + (ydif * ydif);

        if (squareddist <= 6.25f)
        {
            pos.x = Mathf.Floor(mpos.x + .5f);
            pos.y = Mathf.Floor(mpos.y + .5f);
        }
        else
        {
            float angle = Mathf.Atan2(ydif, xdif);
            pos.x = Mathf.Floor(ppos.x - (2.5f * Mathf.Cos(angle))+.5f);
            pos.y = Mathf.Floor(ppos.y - (2.5f * Mathf.Sin(angle))+.5f);


            if ((pos.x - ppos.x) == 3.0f) pos.x = ppos.x + 2.0f;
            if ((pos.y - ppos.y) == 3.0f) pos.y = ppos.y + 2.0f;
        }

        gameObject.transform.position = new Vector3(pos.x, pos.y, gameObject.transform.position.z);
        if (oldpos != pos)
        {
            oldpos = pos;
            SelectorTilemap.GetComponent<SelectorGrid>().UpdateSprite();
        }
    }


}
