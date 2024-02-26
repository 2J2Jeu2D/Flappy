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
        //si l'objet touch� est une colonne. alors on change l'image de Flappy change pour son image bless�e.
        if (infocollision.gameObject.name == "Colonne")
        {
            GetComponent<SpriteRenderer>().sprite = flappyBlesse;
        }

        //si l'objet touch� est la pi�ce d'or. alors la pi�ce dispara�t (il faut la d�sactiver).
        //Apr�s quelques secondes (le temps que la colonne sorte de la sc�ne) elle redevient active.
        //Sa position verticale se modifie un peu al�atoirement. (Indice: Random.Range)
        else if (infocollision.gameObject.name == "PieceOr")
        {
            infocollision.gameObject.SetActive(false); //Pi�ce disparait
            Invoke("PieceRevient", 3f); //Pi�ce revient apr�s 3 secondes
        }



        //si l'objet touch� est le "pack" de vie. alors l'objet PackVie dispara�t pour quelques secondes (comme la pi�ce).
        //Flappy gu�rit(il reprend son image normale). Il faut m�moriser cette image dans une variable.   
        else if (infocollision.gameObject.name == "PackVie")
        {
            infocollision.gameObject.SetActive(false); //Pack de vie disparait
            Invoke("PackVieRevient", 3f); //Pack revient apr�s 3 secondes
            GetComponent<SpriteRenderer>().sprite = flappyNormal; //Fappy redevient normal

        }


        //si l'objet touch� est le champignon. alors le champignon dispara�t pour quelques secondes. Flappy devient gros pour quelques secondes.
        //Pour modifier la taille (Scale)  d'un objet, il faut utiliser le composant transform et modifier la valeur de sa propri�t� LocalScale
        else if (infocollision.gameObject.name == "Champingon")
        {
            infocollision.gameObject.SetActive(false); //Champignon disparait
            transform.localScale *= 1.5f; //Flappy grossit 
            Invoke("ChampignonRevient", 3f); //Champignon revient apr�s 3 secondes
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
