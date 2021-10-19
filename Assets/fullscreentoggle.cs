using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fullscreentoggle : MonoBehaviour
{
    public bool fulltoggle = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //fullscreen toggle
        if (Input.GetKeyDown("f"))
        {
            if (!fulltoggle)
            {
                gameObject.GetComponent<UnityEngine.U2D.PixelPerfectCamera>().refResolutionX = 960;
                gameObject.GetComponent<UnityEngine.U2D.PixelPerfectCamera>().refResolutionY = 720;
            }
            else
            {
                gameObject.GetComponent<UnityEngine.U2D.PixelPerfectCamera>().refResolutionX = 320;
                gameObject.GetComponent<UnityEngine.U2D.PixelPerfectCamera>().refResolutionY = 240;
            }
            fulltoggle = !fulltoggle;
        }
    }
}
