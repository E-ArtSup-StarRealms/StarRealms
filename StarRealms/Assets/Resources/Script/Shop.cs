using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources.Script
{
    public class Shop : MonoBehaviour
    {
        public Card Default;
        public int nbDefault = 10;
        public List<Card> display = new List<Card>();
        public List<Card> gameDeck;
        
        public void Refill(Card chosenCard)
        {
            display.Remove(chosenCard);
            display.Add(gameDeck[0]);
            gameDeck.RemoveAt(0);
        }
    }
}
