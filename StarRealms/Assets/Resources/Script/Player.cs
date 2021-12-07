using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace Resources.Script
{
    public class Player : MonoBehaviour
    {    
        //Variables de classe (attributs de la classe)
        public int hp = 50;
        public int money;
        public int totalPower;
        public int toDiscard;
        public List<Card> deck = new List<Card>();
        public List<Card> discardPile = new List<Card>();
        public List<Card> hand = new List<Card>();
        public List<Card> board = new List<Card>();
        public Dictionary<Faction, int> DicoFacThisTurn;
        public bool cardOnTop;
        public bool freeShip;
    
        //Variables utiles pour le code
        public List<Effect> priorityCheck;
        public GameObject shopObject;
        private Shop _shop;
        public GameObject enemyObject;
        private Player _enemy;
        public GameObject objectDeck;
        public GameObject objectDiscardPile;
        public GameObject objectHand;
        public GameObject objectBoard;
        public static  int HandNumber;

        void Start()
        {
            _shop = shopObject.GetComponent<Shop>();
            _enemy = enemyObject.GetComponent<Player>();
        }
    
        void Update()
        {
            if(gameObject.name == "Player")
            {
                if (Input.GetKeyUp(KeyCode.D))
                    Draw(1);
                if (Input.GetKeyUp(KeyCode.B))
                    Buy(_shop.transform.GetChild(0).GetComponent<Card>());
                if (Input.GetKeyUp(KeyCode.P))
                    PlayCard(GameObject.Find("Hand").transform.GetChild(0).GetComponent<Card>());
                if (Input.GetKeyUp(KeyCode.A))
                    PlayAll();
                if (Input.GetKeyUp(KeyCode.E))
                    EndTurn();
                if (Input.GetKeyUp(KeyCode.F))
                    Attack(enemyObject);
                if (Input.GetKeyUp(KeyCode.L))
                    LookDeck();
            }
        }

    
        public void Draw(int nb)
        {
            for (int i = 0; i < nb; i++)
            {
                if (deck.Count == 0)
                {
                    if (discardPile.Count != 0)
                    {
                        RefillDeck();
                        deck[0].gameObject.transform.SetParent(objectHand.transform);
                        deck[0].objectToMove = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(HandNumber).gameObject;
                        deck[0].objectToMove.SetActive(true);
                        deck[0].gameObject.SetActive(true);
                        HandNumber++;
                        hand.Add(deck[0]);
                        deck.RemoveAt(0);
                    }
                }
                else
                {
                    deck[0].gameObject.transform.SetParent(objectHand.transform);
                    deck[0].objectToMove = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(HandNumber).gameObject;
                    deck[0].objectToMove.SetActive(true);
                    deck[0].gameObject.SetActive(true);
                    HandNumber++;
                    hand.Add(deck[0]);
                    deck.RemoveAt(0);
                }
            }
        }

        public void Buy(Card card)
        {
            if (freeShip)
                TakeCardFromShop(card);
            else if (money >= card.cost)
            {
                TakeCardFromShop(card);
                money -= card.cost;
            }
            else
                Debug.Log("Vous n'avez pas assez d'argent");
        }

        public void PlayCard(Card card)
        {
            card.gameObject.transform.SetParent(objectBoard.transform);
            hand.Remove(card);
            board.Add(card);
        }

        public void PlayAll()
        {
            bool canPlay = true;
            foreach (var card in hand)
            {
                if (card.needPlayer)
                {
                    canPlay = false;
                }
            }

            if (canPlay)
            {
                int nbCards = hand.Count;
                for (int i = 0; i < nbCards; i++)
                {
                    PlayCard(hand[0]);
                }
            }
        }

        public void EndTurn()
        {
            if (money == 0 && totalPower == 0)
            {
                int nbCard = board.Count;
                for (int i = 0; i < nbCard; i++)
                {
                    if (!board[0].shipOrBase)
                    {
                        board[0].gameObject.transform.SetParent(objectDiscardPile.transform);
                        board[0].objectToMove.SetActive(false);
                        board[0].objectToMove = objectDiscardPile.transform.GetChild(0).gameObject;
                        discardPile.Add(board[0]);
                        board.RemoveAt(0);
                    }
                }
                nbCard = hand.Count;
                for (int i = 0; i < nbCard; i++)
                {
                    hand[0].gameObject.transform.SetParent(objectDiscardPile.transform);
                    hand[0].objectToMove.SetActive(false);
                    hand[0].objectToMove = objectDiscardPile.transform.GetChild(0).gameObject;
                    discardPile.Add(hand[0]);
                    hand.RemoveAt(0);
                }
                GameManager.EndTurn();
                
                Debug.Log("Changement de tour");
                //Appel de la fonction de changement de tour dans le GameManeger
            }
            else
                Debug.Log("Vous avez de l'argent ou des points d'attaque inutilisés");
            // LANCER LA POPUP
            
        }

        public void Attack(GameObject target)
        {
            if (target.name == enemyObject.name)
            {
                Debug.Log("ennemi");
                if (CanAttackPlayer())
                {
                    _enemy.hp -= totalPower;
                    totalPower = 0;
                    if (_enemy.hp <= 0)
                        Debug.Log("You Win");
                }
                else
                    Debug.Log("Vous devez d'abord détruire la base taunt avant d'attaquer le joueur");
            }
            else
            {
                Card targetCard = target.GetComponent<Card>();
                if (targetCard.isTaunt || CanAttackPlayer())
                {
                    AttackBase(targetCard, target);
                }
                else
                {
                    Debug.Log("Vous devez d'abord détruire la base taunt");
                }
            }
        }

        //public void Discard(int nb){ } //A faire une fois qu'on aura l'UI

        public void LookDiscard()
        {
            foreach (Card card in discardPile)
            {
                Debug.Log(card);
            }
        }

        public void LookDeck()
        {
            foreach (Card card in deck)
            {
                Debug.Log(card);
            }
        }

        public void RefillDeck()
        {
            Shuffle();
            for (int i = 0; i < discardPile.Count; i++)
            {
                objectDiscardPile.transform.GetChild(0).SetParent(objectDeck.transform);
            }
            deck = new List<Card>(discardPile);
            discardPile.Clear();
        }
    
        public void Shuffle()
        {
            int nbCard = discardPile.Count;
            List<Card> tempList = new List<Card>(discardPile);
            discardPile.Clear();
            for (int i = 0; i < nbCard; i++)
            {
                int rnd = Random.Range(0, nbCard - i);
                discardPile.Add(tempList[rnd]);
                tempList.RemoveAt(rnd);
                objectDiscardPile.transform.GetChild(rnd).SetParent(objectDiscardPile.transform);
            }
        }
    
        //Méthodes utiles pour la classe
    
        public void AttackBase(Card targetCard, GameObject target)
        {
            if(targetCard.baseLife <= totalPower)
            {
                Destroy(target);
                totalPower -= targetCard.baseLife;
            }
            else
                Debug.Log("Vous n'avez pas assez d'attaque");
        }
        public bool CanAttackPlayer()
        {
            foreach (Card card in _enemy.board)
            {
                if (card.isTaunt)
                    return false;
            }
            return true;
        }

        public void TakeCardFromShop(Card card)
        {
            if (cardOnTop)
            {
                card.gameObject.transform.SetParent(objectDeck.transform);
                deck.Insert(0, card);
            }
            else
            {
                card.gameObject.transform.SetParent(objectDiscardPile.transform);
                discardPile.Add(card);
            }
            _shop.Refill(card);
        }
    }
}