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
        public int chosenCard; 
        /*public void RefillAll()
        {
            for(int i = 0; i < display.Count; i++)
            {
                gameDeck.Add(display[i]);
            }
            display.Clear();
            for(int i=0;i<5;i++)
            {
                display.Add(gameDeck[0]);
                gameDeck.RemoveAt(0);
            }
        }*/
        public void Refill()
        {
            display.RemoveAt(chosenCard);
            display.Add(gameDeck[0]);
            gameDeck.RemoveAt(0);
        }
    }
}
