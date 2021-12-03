using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace Resources.Script
{
    public class Player : MonoBehaviour
    {    
        //Variables de classe (attributs de la classe)
        public int hp = 50;
        public int money = 0;
        public int totalPower = 0;
        public int toDiscard;
        public List<Card> deck = new List<Card>();
        public List<Card> discardPile = new List<Card>();
        public List<Card> hand = new List<Card>();
        public List<Card> board = new List<Card>();
        public Dictionary<Faction, int> dicoFacThisTurn;
        public bool cardOnTop;
        public bool freeShip;
    
        //Variables utiles pour le code
        public List<Effect> priorityCheck;
        public GameObject shopObject;
        private Shop shop;
        public GameObject enemyObject;
        private Player enemy;
        public GameObject objectDeck;
        public GameObject objectDiscardPile;
        public GameObject objectHand;
        public GameObject objectBoard;

        void Start()
        {
            shop = shopObject.GetComponent<Shop>();
            enemy = enemyObject.GetComponent<Player>();
        }
    
        void Update()
        {
        if(gameObject.name == "Player")
            {
                if (Input.GetKeyUp(KeyCode.D))
                    Draw(1);
                if (Input.GetKeyUp(KeyCode.B))
                    Buy(shop.transform.GetChild(0).GetComponent<Card>());
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
                    RefillDeck();
                }
                deck[0].gameObject.transform.SetParent(objectHand.transform);
                hand.Add(deck[0]);
                deck.RemoveAt(0);
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
            if (hand.Count == 0)
            {
                if (money == 0 && totalPower == 0)
                {
                    int nbCard = board.Count;
                    for (int i = 0; i < nbCard; i++)
                    {
                        if (!board[0].shipOrBase)
                        {
                            board[0].gameObject.transform.SetParent(objectDiscardPile.transform);
                            discardPile.Add(board[0]);
                            board.RemoveAt(0);
                        }
                    }
                    Debug.Log("Changement de tour");
                    //Appel de la fonction de changement de tour dans le GameManeger
                }
                else
                    Debug.Log("Vous avez de l'argent ou des points d'attaque inutilisés");
            }
            else
                Debug.Log("Il vous reste des cartes dans votre main");
        }

        public void Attack(GameObject target)
        {
            if (target.name == enemyObject.name)
            {
                Debug.Log("ennemi");
                if (canAttackPlayer())
                {
                    enemy.hp -= totalPower;
                    totalPower = 0;
                    if (enemy.hp <= 0)
                        Debug.Log("You Win");
                }
                else
                    Debug.Log("Vous devez d'abord détruire la base taunt avant d'attaquer le joueur");
            }
            else
            {
                Card targetCard = target.GetComponent<Card>();
                if (targetCard.isTaunt || canAttackPlayer())
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
        public bool canAttackPlayer()
        {
            foreach (Card card in enemy.board)
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
            shop.Refill(card);
        }
    }
}