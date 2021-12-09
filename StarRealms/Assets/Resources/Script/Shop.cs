using System.Collections.Generic;
using UnityEngine;

namespace Resources.Script
{
    public class Shop : MonoBehaviour
    {
        public List<Card> explorer;
        public List<Card> display = new List<Card>();
        public static readonly List<Card> GameDeck = new List<Card>();
        public GameObject objectGameDeck;
        
        public void Refill(Card chosenCard)
        {
            if (!explorer.Contains(chosenCard))
            {
                display.Add(GameDeck[0]);
                GameDeck[0].objectToMove = transform.GetChild(0).transform.GetChild(0).transform
                    .GetChild(display.IndexOf(chosenCard)).gameObject;
                GameDeck[0].gameObject.SetActive(true);
                display.Remove(chosenCard);
                GameDeck.Remove(GameDeck[0]);
            }
        }
        public void Startfill()
        {
            for(int i=0 ; i < 5 ; i++)
            {
                display.Add(GameDeck[0]);
                GameDeck[0].gameObject.SetActive(true);
                GameDeck[0].objectToMove = transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject;
                GameDeck.Remove(GameDeck[0]);
            }
        }
        public static void ShuffleGameDeck()
        {
            int nbCard = GameDeck.Count;
            List<Card> tempList = new List<Card>(GameDeck);
            GameDeck.Clear();
            for (int i = 0; i < nbCard; i++)
            {
                int rnd = Random.Range(0, nbCard - i);
                GameDeck.Add(tempList[rnd]);
                tempList.RemoveAt(rnd);
            }
        }
    }
}
