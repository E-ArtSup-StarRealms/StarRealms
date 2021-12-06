using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources.Script
{
    public class Shop : MonoBehaviour
    {
        public List<Card> Default;
        public List<Card> display = new List<Card>();
        
        public static List<Card> gameDeck = new List<Card>();
        
        public void Refill(Card chosenCard)
        {
            display.Remove(chosenCard);
            display.Add(gameDeck[0]);
            gameDeck.RemoveAt(0);
        }
        public void Startfill()
        {
           
            for(int i=0 ; i < 5 ; i++)
            {
                display.Add(gameDeck[0]);
                //gameDeck[0].objectToMove = transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject;
                gameDeck.Remove(gameDeck[0]);
                

            }
        
        }
        public static void ShuffleGameDeck()
        {
            int nbCard = gameDeck.Count;
            List<Card> tempList = new List<Card>(gameDeck);
            gameDeck.Clear();
            for (int i = 0; i < nbCard; i++)
            {
                int rnd = Random.Range(0, nbCard - i);
                gameDeck.Add(tempList[rnd]);
                tempList.RemoveAt(rnd);
            }
        }
        
    }
}
