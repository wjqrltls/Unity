using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    //public Sprite cardFace;
    public Material cardFace;

    //public Sprite cardBack;
    public Material cardBack;

    //private SpriteRenderer spriteRenderer;
    private Renderer renderer;

    private Selectable selectable;
    private Solitaire solitaire;
    private UserInput userInput;


    void Start()
    {
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
        userInput = FindObjectOfType<UserInput>();

        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = solitaire.cardFaces[i];
                break;
            }
            i++;
        }
        //spriteRenderer = GetComponent<SpriteRenderer>();
        renderer = GetComponent<Renderer>();
        selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectable.faceUp == true)
        {
            //spriteRenderer.sprite = cardFace;
            renderer.material = cardFace;
        }
        else
        {
            //spriteRenderer.sprite = cardBack;
            renderer.material = cardBack;
        }

        if (userInput.slot1)
        {

            if (name == userInput.slot1.name)
            {
                //spriteRenderer.color = Color.yellow;
                renderer.material.color = Color.yellow;
            }
            else
            {
                //spriteRenderer.color = Color.white;
                renderer.material.color = Color.white;
            }
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    List<string> deck = Solitaire.GenerateDeck();
    //    solitaire = FindObjectOfType<Solitaire>();
    //    userInput = FindObjectOfType<UserInput>();

    //    int i = 0;
    //    foreach (string card in deck)
    //    {
    //        if (this.name == card)
    //        {
    //            cardFace = solitaire.cardFaces[i];
    //            break;
    //        }
    //        i++;
    //    }
    //    spriteRenderer = GetComponent<SpriteRenderer>();
    //    selectable = GetComponent<Selectable>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (selectable.faceUp == true)
    //    {
    //        spriteRenderer.sprite = cardFace;
    //    }
    //    else
    //    {
    //        spriteRenderer.sprite = cardBack;
    //    }

    //    if (userInput.slot1)
    //    {

    //        if (name == userInput.slot1.name)
    //        {
    //            spriteRenderer.color = Color.yellow;
    //        }
    //        else
    //        {
    //            spriteRenderer.color = Color.white;
    //        }
    //    }
    //}
}
