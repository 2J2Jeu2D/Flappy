using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleFlappy : MonoBehaviour

{
    public float vitesseX;
    public float vitesseY;
    public float positionFin;
    public float positionDebut;
    public float deplacementAleatoire;

    public Sprite flappyBlesse;
    public Sprite flappyNormal;
    public GameObject objetPiece;
    public GameObject objetPack;
    public GameObject objetChampignon;


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

    void OnCollisionEnter2D(Collision2D infocollision)
    {
        //si l'objet touché est une colonne. alors on change l'image de Flappy change pour son image blessée.
        if (infocollision.gameObject.name == "Colonne")
        {
            GetComponent<SpriteRenderer>().sprite = flappyBlesse;
        }

        //si l'objet touché est la pièce d'or. alors la pièce disparaît (il faut la désactiver).
        //Après quelques secondes (le temps que la colonne sorte de la scène) elle redevient active.
        //Sa position verticale se modifie un peu aléatoirement. (Indice: Random.Range)
        else if (infocollision.gameObject.name == "PieceOr")
        {
            infocollision.gameObject.SetActive(false); //Pièce disparait
            Invoke("PieceRevient", 3f); //Pièce revient après 3 secondes
        }



        //si l'objet touché est le "pack" de vie. alors l'objet PackVie disparaît pour quelques secondes (comme la pièce).
        //Flappy guérit(il reprend son image normale). Il faut mémoriser cette image dans une variable.   
        else if (infocollision.gameObject.name == "PackVie")
        {
            infocollision.gameObject.SetActive(false); //Pack de vie disparait
            Invoke("PackVieRevient", 3f); //Pack revient après 3 secondes
            GetComponent<SpriteRenderer>().sprite = flappyNormal; //Fappy redevient normal

        }


        //si l'objet touché est le champignon. alors le champignon disparaît pour quelques secondes. Flappy devient gros pour quelques secondes.
        //Pour modifier la taille (Scale)  d'un objet, il faut utiliser le composant transform et modifier la valeur de sa propriété LocalScale
        else if (infocollision.gameObject.name == "Champingon")
        {
            infocollision.gameObject.SetActive(false); //Champignon disparait
            transform.localScale *= 1.5f; //Flappy grossit 
            Invoke("ChampignonRevient", 3f); //Champignon revient après 3 secondes
        }
    }

    void PieceRevient()
    {
        objetPiece.SetActive(true);

        float valeurAleatoireY = Random.Range(-deplacementAleatoire, deplacementAleatoire);

        objetPiece.transform.position = new Vector2(positionDebut, valeurAleatoireY);
    }

    void PackVieRevient()
    {
        objetPack.SetActive(true);
    }

    void ChampignonRevient()
    {
        objetChampignon.SetActive(true);
        transform.localScale /= 1.5f;
    }
}
