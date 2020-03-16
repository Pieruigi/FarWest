using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTableFreeTimeActionController : BaseFreeTimeActionController
{

    [SerializeField]
    GameObject deck;

    [SerializeField]
    List<Transform> cardsPositions;

    [SerializeField]
    GameObject cardPrefab;

    [SerializeField]
    Collider tableCollision;

    [SerializeField]
    Transform castleCardsParent;

    [SerializeField]
    AudioClip shuffleClip;

    [SerializeField]
    AudioClip drawClip;

    [SerializeField]
    List<AudioClip> sighClips;

    [SerializeField]
    List<AudioClip> angryClips;

    Vector3 deckPosDefault;
    Vector3 deckAngDefault;

    Transform handR, handL, deckParentDefault;

    int cardCount = 0;

    List<GameObject> cards = new List<GameObject>();

    List<GameObject> toFlip = new List<GameObject>();

    List<GameObject> castleCards = new List<GameObject>();

    List<Vector3> castleCardsPosDefault = new List<Vector3>();
    List<Vector3> castleCardsAngDefault = new List<Vector3>();

    int castleCardsStep = 0;
    int castleCardsStart = 0;

    AudioSource source;

    protected override void Start()
    {
      
        base.Start();

        deckPosDefault = deck.transform.localPosition;
        deckAngDefault = deck.transform.localEulerAngles;
        deck.transform.localScale = Vector3.zero;

        handR = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(p => "hand.r".Equals(p.name));
        handL = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(p => "hand.l".Equals(p.name));
        deckParentDefault = deck.transform.parent;

        tableCollision.enabled = false;
        for(int i=0; i<castleCardsParent.childCount; i++)
            castleCards.Add(castleCardsParent.GetChild(i).gameObject);

        source = GetComponent<AudioSource>();

        ResetCastle();
    }

    public override void ActionMessage(string message)
    {
        base.ActionMessage(message);

        if ("ShowDeck".Equals(message))
        {
            LeanTween.scale(deck, Vector3.one, 1f).setEaseOutElastic();
            cardCount = 0;
            cards.Clear();
            toFlip.Clear();
        }

        if ("HideDeck".Equals(message))
        {
            LeanTween.scale(deck, Vector3.zero, 1f).setEaseOutElastic();
            StartCoroutine(DropAllCards());
        }

        if ("TakeDeck".Equals(message))
        {
            deck.transform.parent = handL;
        }

        if ("DropDeck".Equals(message))
        {
            deck.transform.parent = deckParentDefault;
            LeanTween.moveLocal(deck, deckPosDefault, 0.1f);
            LeanTween.rotateLocal(deck, deckAngDefault, 0.1f);
        }

        if ("ThrowCard".Equals(message))
        {
            StartCoroutine(ThrowCard());
        }

        if ("FlipCards".Equals(message))
        {
            FlipCards();
        }

        if ("ScatterCards".Equals(message))
        {
            StartCoroutine(ScatterCards());
        }

        if ("CastleCardsCreate".Equals(message))
        {
            StartCoroutine(CastleCardsPopIn());
        }

        if ("ScatterCardsRB".Equals(message))
        {
            StartCoroutine(ScatterCardsRB());
        }

        if ("TakeTwoCards".Equals(message))
        {
            StartCoroutine(TakeTwoCards());
        }

        if ("PlaySteps".Equals(message))
        {
            Player.SendMessage("PlaySteps");
        }

        if ("PlayFailed".Equals(message))
        {
            StartCoroutine(PlayFailed());
            
        }

        if ("ShuffleCards".Equals(message))
        {
            source.clip = shuffleClip;
            source.Play();
        }
    }

    IEnumerator ThrowCard()
    {
        Vector3 pos = new Vector3(0.0168f, 0.0611f, -0.0264f);
        Vector3 ang = new Vector3(-107.415f, 86.765f, 87.089f);
        GameObject card = Utility.ObjectPopIn(cardPrefab, pos, ang, Vector3.one, handR);
        cards.Add(card);
        toFlip.Add(card);

        source.clip = drawClip;
        source.Play();
        yield return new WaitForSeconds(0.25f);

        
        card.transform.parent = null;
        LeanTween.move(card, cardsPositions[cardCount], 0.5f);
        LeanTween.rotate(card, cardsPositions[cardCount].eulerAngles, 0.5f);

        cardCount++;
        
    }

    void FlipCards()
    {
        int count = 1;
        if (toFlip.Count == 10 || toFlip.Count == 2 || toFlip.Count == 6)
            count = 2;

        for(int i=0; i<count; i++)
        {
            GameObject flip = toFlip[Random.Range(0,toFlip.Count)];
            toFlip.Remove(flip);
            LeanTween.rotateAroundLocal(flip, Vector3.right, 180, 0.5f);
        }
        
    }

    IEnumerator DropAllCards()
    {
        GameObject[] array = cards.ToArray();
        cards.Clear();
        toFlip.Clear();
        cardCount = 0;
        for (int i=0; i<array.Length; i++)
        {
            Utility.ObjectPopOut(array[i]);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ScatterCards()
    {
        for(int i=0; i<cards.Count; i++)
        {
            Vector3 newPos = cards[i].transform.position;
            newPos.x += Random.Range(-0.1f, 0.1f);
            newPos.z += Random.Range(-0.1f, 0.1f);
            Vector3 newRot = cards[i].transform.eulerAngles;
            newRot.y += Random.Range(-60f, 60f);
            LeanTween.move(cards[i], newPos, Random.Range(0.1f, 0.15f));
            LeanTween.rotateY(cards[i], newRot.y, Random.Range(0.1f, 0.15f));
            yield return new WaitForSeconds(Random.Range(0.07f, 0.1f));
        }
        yield break;
    }

    IEnumerator CastleCardsPopIn()
    {
        if(!tableCollision.enabled)
            tableCollision.enabled = true;

        int count = 0;
        switch (castleCardsStep)
        {
            case 0:
                count = 4;
                break;
            case 1:
                count = 4;
                break;
            case 2:
                count = 3;
                break;
            case 3:
                count = 6;
                break;
            case 4:
                count = 2;
                break;
            case 5:
                count = 4;
                break;
            case 6:
                count = 1;
                break;
            case 7:
                count = 2;
                break;

        }

        if (castleCardsStep == 7)
        {

            for (int i = 0; i < castleCards.Count; i++) 
            {
                castleCards[i].GetComponent<Rigidbody>().isKinematic = false;
                castleCards[i].GetComponent<Rigidbody>().AddForce(0.8f*new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode.VelocityChange);
                castleCards[i].GetComponent<Rigidbody>().AddTorque(0.8f * new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode.VelocityChange);
            }
            
        }

        for (int i=castleCardsStart; i< castleCardsStart+count; i++)
        {
            castleCards[i].SetActive(true);
            LeanTween.scale(castleCards[i], Vector3.one, 1f).setEaseOutElastic();
           
            yield return new WaitForSeconds(Random.Range(0.1f, 0.15f));
        }
        


        castleCardsStart += count;
        castleCardsStep++;
    }

    IEnumerator ScatterCardsRB()
    {
        for (int i = 0; i < castleCards.Count; i++)
        {
            castleCards[i].GetComponent<Rigidbody>().AddForce(4.3f * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), ForceMode.VelocityChange);
            
            yield return new WaitForSeconds(Random.Range(0.03f, 0.05f));
        }

        yield return new WaitForSeconds(2.5f);

        foreach(GameObject card in castleCards)
        {
            LeanTween.scale(card, Vector3.zero, 1f).setEaseOutElastic();
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3);
        ResetCastle();

        
    }

    IEnumerator TakeTwoCards()
    {
        Vector3 pos = new Vector3(-0.001f, 0.073f, -0.03f);
        Vector3 ang = new Vector3(-96.1f, -104.19f, -81.78f);
        GameObject card1 = Utility.ObjectPopIn(cardPrefab, pos, ang, Vector3.one, handL);

        yield return new WaitForSeconds(1f);
        pos = new Vector3(-0.018f, 0.072f, -0.03f);
        ang = new Vector3(-83.27f, 76.58f, -262.55f);
        GameObject card2 = Utility.ObjectPopIn(cardPrefab, pos, ang, Vector3.one, handR);

        if(castleCardsStep < 7)
            yield return new WaitForSeconds(2f);
        else
            yield return new WaitForSeconds(0.5f);

        Utility.ObjectPopOut(card1);
        Utility.ObjectPopOut(card2);

    }

    IEnumerator PlayFailed()
    {
        ChicoFXController fx = Player.GetComponent<ChicoFXController>();
        fx.Play(sighClips[Random.Range(0, sighClips.Count)], false);

        while (fx.IsPlaying())
            yield return null;

        yield return new WaitForSeconds(0.5f);
        fx.Play(angryClips[Random.Range(0, sighClips.Count)], false);

    }

    void ResetCastleCard(GameObject card)
    {
        card.SetActive(false);
        card.GetComponent<Rigidbody>().isKinematic = true;
        castleCardsPosDefault.Add(card.transform.localPosition);
        castleCardsAngDefault.Add(card.transform.localEulerAngles);
        card.transform.localScale = Vector3.zero;
    }

    void ResetCastle()
    {
        castleCardsStart = 0;
        castleCardsStep = 0;
        for (int i = 0; i < castleCards.Count; i++)
        {
            ResetCastleCard(castleCards[i]);
        }
    }
}
