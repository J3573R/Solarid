using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour {

    static public bool center = true;
    static public bool cursorOff = false;


    public float cursorAdjustY;
    public float cursorAdjustX;
    public Texture crosshairTexture;
    public int W;
    public int H;
    public bool on = true;

    private void Awake()
    {
        //Cursor.visible = false;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x + cursorAdjustX, Event.current.mousePosition.y + cursorAdjustY, W, H), crosshairTexture, ScaleMode.ScaleToFit);
    }


    void Update()
    {
        Cursor.visible = false;        
    }

}
     

