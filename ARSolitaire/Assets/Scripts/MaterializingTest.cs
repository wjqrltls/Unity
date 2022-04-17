using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterializingTest : MonoBehaviour
{
    public Material[] CardFaces;
    public Sprite[] SpritesSs;

    int cardFaceIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = CardFaces[0];
        StartCoroutine(CardFacesChanger());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator CardFacesChanger()
    {
        
        GetComponent<Renderer>().material = CardFaces[cardFaceIndex];
        cardFaceIndex++;
        Debug.Log("CardFacesChanger / cardFaceIndex = "+cardFaceIndex);
        
        yield return new WaitForSeconds(1);
        StartCoroutine(CardFacesChanger());
    }
}
