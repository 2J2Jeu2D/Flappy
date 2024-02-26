using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleFlappy : MonoBehaviour

{
    public float vitesseX;
    public float vitesseY;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            vitesseX = 1;

        }
        else if (Input.GetKey("left") || Input.GetKey("a"))
        {
            vitesseX = -1;
        }
        else
        {
            vitesseX = GetComponent<Rigidbody2D>().velocity.x; //vitesse horizontale actuelle
        }


        if (Input.GetKeyDown("up") || Input.GetKey("w"))
        {
            vitesseY = 4;
        }
        else
        {
            vitesseY = GetComponent<Rigidbody2D>().velocity.y; //vitesse verticale actuelle
        }


        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);
    }
}
