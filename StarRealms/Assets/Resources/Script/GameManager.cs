using System.Collections.Generic;
using UnityEngine;

namespace Resources.Script
{
    public class GameManager : MonoBehaviour
    {
        //variable temporaire
        public List<GameObject> display;
        public List<GameObject> gameDeck;
        public List<GameObject> deck;
        public List<GameObject> discardPile;
        public List<GameObject> hand;
        public List<GameObject> board;
        public List<GameObject> boardOpponent;
        public int hp;
        public int hpOpponent;
        public int money;
        public int totalPower;
        public int toDiscard;
        public bool cadOnTop;
        public bool freeShip;

        public static Player player1;
        public static Player player2;
        public static Player currentPlayer;

        [SerializeField]
        private GameObject panelWin;
        private bool firstRound;

        private void Start()
        {
            panelWin.SetActive(false);
            firstRound = true;
            hp = 50;
            hpOpponent = 50;
            BeginTurn();
        }

        //Distribue les carte selon si c'est le premier tour ou non
        private void BeginTurn()
        {
            if(firstRound)
            {
                for(int i = 0;i < 3; i = i+1)
                {
                    Draw();
                }
                firstRound = false;
            }
            else
            {
                for (int i = 0; i < 5; i = i + 1)
                {
                    Draw();
                }
            }

        }

        //l'action de pioche qui peux �tre appeller n'importe quand 
        private void Draw()
        {
            int deckSize = deck.Count;
            int defauceSize = discardPile.Count;
            if ( deckSize <= 0)
            {
                for (int i = 0; i < defauceSize; i = i + 1)
                {
                    ReFillDeck();
                }
                // on rappelle la fonction car la le joueur n'a toujour pas piocher ducoup ;D
                Draw(); 
            }
            else
            {
                GameObject card = deck[deck.Count - 1].gameObject;
                deck.RemoveAt(deck.Count - 1);
                hand.Add(card);
            }
        
        }

        //si il n'y a plus de carte dans le deck du joueur il est refill par la deffauce 
        private void ReFillDeck()
        {
            //le Random fait une sorte de m�lange des carte avant de les deplacer dans le deck
            int id = Random.Range(0, discardPile.Count);
            GameObject card = discardPile[id].gameObject;
            discardPile.RemoveAt(id);
            deck.Add(card);
        }


        private void EndTurn()
        {
            if(hpOpponent <= 0)
            {
                panelWin.SetActive(true);
            }
            else
            {
                int tHp = hp;
                int thpOpponent = hpOpponent;
                money = 0;
                totalPower = 0;
                hp = thpOpponent;
                hpOpponent = tHp;

                //met toute les carte de la main a la deffauce 
                int handSize = hand.Count;
                for (int i = 0; i < handSize; i = i + 1)
                {
                    GameObject card = hand[hand.Count - 1].gameObject;
                    hand.RemoveAt(hand.Count - 1);
                    discardPile.Add(card);
                }


                BeginTurn();
            }
        

        }

    }
}
