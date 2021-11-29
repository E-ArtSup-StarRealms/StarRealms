/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    class Card
    {
        int id;
        string name;
        int cost;
        bool shipOrBase;
        bool isTaunt;
    }
    Card Default;
    int nbDefault = 10;
    List<Card> display;
    List<Card> gameDeck;
    int chosenCard;
    /*void RefillAll()
    {
        for(int i = 0; i < display.Count; i++)
        {
            gameDesk.Add(display[i]);
        }
        display.Clear();
        for(int i=0;i<5;i++)
        {
            display.Add(gameDesk[0]);
            gameDesk.RemoveAt(0);
        }
    }*/
    /*void Refill()
    {
        display.RemoveAt(chosenCard);
        display.Add(gameDeck[0]);
        gameDeck.RemoveAt(0);
    }
}
*/