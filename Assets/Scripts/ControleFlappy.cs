using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class ControleFlappy : MonoBehaviour

{
    public float vitesseX;
    public float vitesseY;
    public float positionFin;
    public float positionDebut;
    public float deplacementAleatoire;
    public float deplacementAleatoireY; // déplacement aléatoire en Y

    public Sprite flappyBlesse;
    public Sprite flappyNormal;
    public Sprite flappyBlesseFerme; // image de Flappy blessé volant
    public Sprite flappyNormalFerme; // image de Flappy normal volant
    public GameObject objetPiece;
    public GameObject objetPack;
    public GameObject objetChampignon;
    public GameObject elementGrille; // objet grille

    // Déclaration de la variable public de l’objet son
    public AudioClip sonColonne;
    public AudioClip sonOr;
    public AudioClip sonPack;
    public AudioClip sonChampignon;
    public AudioClip sonFinPartie;

    //Déclaration des variables pour le texte
    public TextMeshProUGUI textFinDuJeu; 
    public TextMeshProUGUI textPointage; // texte du pointage
    float compteur;

    AudioSource sourceAudio; //Audio

    public bool partieTerminee; // variable pour la fin de la partie
    public bool flappyEtatBlesse; // variable pour l'état de Flappy


    // Start is called before the first frame update
    void Start()
    {
        sourceAudio = GetComponent<AudioSource>(); //va chercher le son
        partieTerminee = false;
        textFinDuJeu.text = "Recommencer la partie?";
        textFinDuJeu.GetComponent<TextMeshProUGUI>().fontSize = 0; //va chercher le texte
    }

    // Update is called once per frame
    void Update()
    {
        if (partieTerminee == false) {
        
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

        if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
        {
            if (flappyEtatBlesse == false)
            {
                // On change l'image de Flappy
                GetComponent<SpriteRenderer>().sprite = flappyNormalFerme;
            }
            else
            {
                // On change l'image de Flappy
                GetComponent<SpriteRenderer>().sprite = flappyBlesseFerme;
            }

            vitesseY = 5;

        }
        else if (Input.GetKeyUp("w") || Input.GetKeyUp("up"))
        {
            if (flappyEtatBlesse == false)
            {
                // On change l'image de Flappy
                GetComponent<SpriteRenderer>().sprite = flappyNormal;
            }
            else
            {
                // On change l'image de Flappy
                GetComponent<SpriteRenderer>().sprite = flappyBlesse;
            }
        }
        // Si aucune touche n'est enfoncée, on récupère la vélocité y actuelle
        else
        {

            vitesseY = GetComponent<Rigidbody2D>().velocity.y;

        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);
        }
        else { 
        
        }

    } 

    void OnCollisionEnter2D(Collision2D infocollision)
    {
        //si l'objet touché est une colonne. alors on change l'image de Flappy change pour son image blessée.
        if (infocollision.gameObject.name == "Colonne")
        {
            sourceAudio.PlayOneShot(sonColonne, 1f); //Joue le clip qui se trouve dans la variable sonCol
            
            // On change l'état de Flappy à blessé
            if (infocollision.gameObject.name == "Colonne" && flappyEtatBlesse == false)
            {
                // On change l'image de Flappy
                GetComponent<SpriteRenderer>().sprite = flappyBlesse;
                flappyEtatBlesse = true;

            } else {

                //Fin de partie est activée
                partieTerminee = true;
                textFinDuJeu.GetComponent<TextMeshProUGUI>().fontSize = 100f; //va chercher le texte

                //Flappy peut maintenant tourner
                GetComponent<Rigidbody2D>().freezeRotation = false;
                //Vitesse angulaire
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 5);
                //Désactiver Collider2D
                GetComponent<Collider2D>().enabled = false;

                // Déclencher le son de la fin de la partie
                sourceAudio.PlayOneShot(sonFinPartie, 1f);

                //Relancer le jeu
                Invoke("FinPartie", 3f);
            }
            if (infocollision.gameObject.name == "Colonne")
            {
                compteur = compteur - 5f;
                UpdatePointage();
            }
        }

        //si l'objet touché est la pièce d'or. alors la pièce disparaît (il faut la désactiver).
        //Après quelques secondes (le temps que la colonne sorte de la scène) elle redevient active.
        //Sa position verticale se modifie un peu aléatoirement. (Indice: Random.Range)
        else if (infocollision.gameObject.name == "PieceOr")
        {
            infocollision.gameObject.SetActive(false); //Pièce disparait
            Invoke("PieceRevient", 3f); //Pièce revient après 3 secondes
            sourceAudio.PlayOneShot(sonOr, 1f); //Joue le clip qui se trouve dans la variable sonor
            //Pointage
            compteur = compteur + 5f;
            UpdatePointage();
            //Animation
            elementGrille.GetComponent<Animator>().enabled = true;    //permet d'activer l'Animator
        }

       



        //si l'objet touché est le "pack" de vie. alors l'objet PackVie disparaît pour quelques secondes (comme la pièce).
        //Flappy guérit(il reprend son image normale). Il faut mémoriser cette image dans une variable.   
        else if (infocollision.gameObject.name == "PackVie")
        {
            infocollision.gameObject.SetActive(false); //Pack de vie disparait
            Invoke("PackVieRevient", 3f); //Pack revient après 3 secondes
            GetComponent<SpriteRenderer>().sprite = flappyNormal; //Fappy redevient normal
            sourceAudio.PlayOneShot(sonPack, 1f); //Joue le clip qui se trouve dans la variable sonPack
            //Pointage
            compteur = compteur + 5f;
            UpdatePointage();
        }


        //si l'objet touché est le champignon. alors le champignon disparaît pour quelques secondes. Flappy devient gros pour quelques secondes.
        //Pour modifier la taille (Scale)  d'un objet, il faut utiliser le composant transform et modifier la valeur de sa propriété LocalScale
        else if (infocollision.gameObject.name == "Champingon")
        {
            infocollision.gameObject.SetActive(false); //Champignon disparait
            transform.localScale *= 1.5f; //Flappy grossit 
            Invoke("ChampignonRevient", 3f); //Champignon revient après 3 secondes
            sourceAudio.PlayOneShot(sonChampignon, 1f); //Joue le clip qui se trouve dans la variable sonChampignon
            //Pointage
            compteur = compteur + 10f;
            UpdatePointage();
        }

        //Collision avec le décor
        else if (infocollision.gameObject.name == "Decor")
        {
            compteur = compteur - 5f;
            UpdatePointage();
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

    void FinPartie()
    {
        SceneManager.LoadScene("Flappy");
    }

    void UpdatePointage()
    {
        textPointage.text = "Pointage: " + compteur.ToString();
    }

    // Fonction pour l'activation d'une pièce d'or à une position aléatoire
    void pieceOrAleatoire()
    {
        // On active l'objet pièce d'or
        objetPiece.SetActive(true);

        // On génère une valeur aléatoire pour le déplacement vertical
        float valeurAleatoireY = Random.Range(-deplacementAleatoireY, deplacementAleatoireY);
        objetPiece.transform.position = new Vector2(objetPiece.transform.position.x, valeurAleatoireY);


        elementGrille.GetComponent<Animator>().enabled = false;


    }
}
