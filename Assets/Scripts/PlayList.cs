using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using UnityEngine;

public class PlayList : MonoBehaviour
{
    //add a int for the play card limit 
    List <Sprite> cardPic = new List<Sprite>();
    List <Card> playedCards = new List<Card>();


    // Start is called before the first frame update
    void Start()
    {
        //show an empty list of played cards at the start, for the limit 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
