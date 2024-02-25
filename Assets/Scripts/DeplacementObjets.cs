using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementObjets : MonoBehaviour
{
    public float vitesse;
    public float positionFin;
    public float positionDebut;
    public float deplacementAleatoire;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(vitesse, 0, 0);

        float valeurAleatoireY = Random.Range(-deplacementAleatoire, deplacementAleatoire)
        transform.position = new Vector2(positionDebut, valeurAleatoireY);
        
    }
}
