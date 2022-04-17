using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solitaire : MonoBehaviour
{
    //public Sprite[] cardFaces;
    UIButtons uibuttons;
    public Material[] cardFaces;

    public GameObject cardPrefab;
    public GameObject deckButton;
    public GameObject[] bottomPos;
    public GameObject[] topPos;

    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public List<string>[] bottoms;
    public List<string>[] tops;
    public List<string> tripsOnDisplay = new List<string>();
    public List<List<string>> deckTrips = new List<List<string>>();

    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();


    public List<string> deck;
    public List<string> discardPile = new List<string>();
    private int deckLocation;
    private int trips;
    private int tripsRemainder;
    private int difficulty;
    private CardAnimation cardAnimation;


    private void Awake()
    {
        uibuttons = FindObjectOfType<UIButtons>();

    }

    // Start is called before the first frame update
    void Start()
    {
        cardAnimation = cardAnimation = FindObjectOfType<CardAnimation>();
        bottoms = new List<string>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCards()
    {
        foreach (List<string> list in bottoms)
        {
            list.Clear();
        }

        deck = GenerateDeck();
        Shuffle(deck);

        //test the cards in the deck:
        /*foreach (string card in deck)
        {
            print(card);
        }*/
        SolitaireSort();
        StartCoroutine(SolitaireDeal());
        SortDeckIntoTrips();

    }


    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                newDeck.Add(s + v);
            }
        }
        return newDeck;
    }

    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    IEnumerator SolitaireDeal()
    {
        for (int i = 0; i < 7; i++)
        {

            float yOffset = 0;
            float zOffset = 0.03f;
            foreach (string card in bottoms[i])
            {
                yield return new WaitForSeconds(0.05f);
                //GameObject newCard = Instantiate(cardPrefab, new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity, bottomPos[i].transform);
                 GameObject newCard = Instantiate(cardPrefab, deckButton.transform.position, Quaternion.identity, bottomPos[i].transform);
                cardAnimation.MovingAnimation(newCard, bottomPos[i].transform, new Vector3(0.0f, -yOffset, -zOffset));
                newCard.transform.localScale = new Vector3(1.0f, 0.9f, 1.0f);
                newCard.name = card;
                Debug.Log("[Solitaire] newCard.name = "+newCard.name+" / i = "+i);
                newCard.GetComponent<Selectable>().row = i;
                if (card == bottoms[i][bottoms[i].Count -1])
                {
                    newCard.GetComponent<Selectable>().faceUp = true;
                }
                

                yOffset = yOffset + 0.3f;
                zOffset = zOffset + 0.1f;
                discardPile.Add(card);
            }
        }

        foreach (string card in discardPile)
        {
            if (deck.Contains(card))
            {
                deck.Remove(card);
            }
        }
        discardPile.Clear();

    }

    void SolitaireSort()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = i; j < 7; j++)
            {
                bottoms[j].Add(deck.Last<string>());
                deck.RemoveAt(deck.Count - 1);
            }

        }

    }

    public void SortDeckIntoTrips()
    {
        difficulty = uibuttons.change;
        trips = deck.Count / difficulty;
        tripsRemainder = deck.Count % difficulty;
        deckTrips.Clear();

        int modifier = 0;
        for (int i = 0; i < trips; i++)
        {
            List<string> myTrips = new List<string>();
            for (int j = 0; j < difficulty; j++)
            {
                myTrips.Add(deck[j + modifier]);
            }
            deckTrips.Add(myTrips);
            modifier = modifier + difficulty;
        }
        if (tripsRemainder != 0)
        {
            List<string> myRemainders = new List<string>();
            modifier = 0;
            for (int k = 0; k < tripsRemainder; k++)
            {
                myRemainders.Add(deck[deck.Count - tripsRemainder + modifier]);
                modifier++;
            }
            deckTrips.Add(myRemainders);
            trips++;
        }
        deckLocation = 0;

    }

    public void DealFromDeck()
    {
        // add remaining cards to discard pile

        foreach (Transform child in deckButton.transform)
        {
            if (child.CompareTag("Card"))
            {
                deck.Remove(child.name);
                discardPile.Add(child.name);
                Destroy(child.gameObject);
            }
        }


        if (deckLocation < trips)
        {
            // draw 3 new cards
            tripsOnDisplay.Clear();
            float xOffset = 2.5f;
            float zOffset = -0.2f;

            foreach (string card in deckTrips[deckLocation])
            {
                //GameObject newTopCard = Instantiate(cardPrefab, new Vector3(deckButton.transform.position.x + xOffset, deckButton.transform.position.y, deckButton.transform.position.z + zOffset), Quaternion.identity, deckButton.transform);
                GameObject newTopCard = Instantiate(cardPrefab, deckButton.transform.position, Quaternion.identity, deckButton.transform);
                cardAnimation.MovingAnimation(newTopCard, deckButton.transform, new Vector3(xOffset, 0.0f, zOffset));
                newTopCard.transform.localScale = new Vector3(1.0f, 0.9f, 1.0f);
                xOffset = xOffset + 0.5f;
                zOffset = zOffset - 0.002f;
                newTopCard.name = card;
                tripsOnDisplay.Add(card);
                newTopCard.GetComponent<Selectable>().faceUp = true;
                newTopCard.GetComponent<Selectable>().inDeckPile = true;
            }
            deckLocation++;

        }
        else
        {
            //Restack the top deck
            RestackTopDeck();
        }
    }

    void RestackTopDeck()
    {
        deck.Clear();
        foreach (string card in discardPile)
        {
            deck.Add(card);
        }
        discardPile.Clear();
        Shuffle(deck);
        SortDeckIntoTrips();
    }
}
