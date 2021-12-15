using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        public GameObject objectInfo;
        public int handNumber;
        public int boardNumber = 0;

        void Start()
        {
            _shop = shopObject.GetComponent<Shop>();
            _enemy = enemyObject.GetComponent<Player>();
            objectInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = hp + "\nHP";
            objectBoard.transform.GetChild(1).gameObject.SetActive(false);
        }
        /*void Update()
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
                    EndTurn(false);
                if (Input.GetKeyUp(KeyCode.F))
                    Attack(enemyObject);
                if (Input.GetKeyUp(KeyCode.L))
                    LookDeck();
            }
        }*/
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
                        deck[0].objectToMove = Instantiate((GameObject) UnityEngine.Resources.Load("Prefab/Tmp/Image"),objectHand.transform);
                        deck[0].transform.SetParent(deck[0].objectToMove.transform);
                        deck[0].gameObject.SetActive(true);
                        deck[0].transform.localScale = new Vector3(182,121,97);
                        deck[0].handPos = handNumber;
                        handNumber++;
                        hand.Add(deck[0]);
                        objectHand.transform.GetChild(0).GetChild(0).GetComponent<ListNavigation>().AddElement(deck[0]);
                        deck.RemoveAt(0);
                    }
                }
                else
                {
                    deck[0].gameObject.transform.SetParent(objectHand.transform);
                    deck[0].objectToMove = Instantiate((GameObject) UnityEngine.Resources.Load("Prefab/Tmp/Image"),objectHand.transform.GetChild(0).GetChild(0));
                    deck[0].transform.SetParent(deck[0].objectToMove.transform);
                    deck[0].gameObject.SetActive(true);
                    deck[0].transform.localScale = new Vector3(182,121,97);
                    deck[0].handPos = handNumber;
                    handNumber++;
                    hand.Add(deck[0]);
                    objectHand.transform.GetChild(0).GetChild(0).GetComponent<ListNavigation>().AddElement(deck[0]);
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
                objectInfo.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = money+" $";
                card.objectToMove = objectDiscardPile.transform.GetChild(0).gameObject;
            }
            else
                Debug.Log("You do not have enough money");
        }
        public void PlayCard(Card card)
        {
            handNumber--;
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
        public void EndTurn(bool bypass)
        {
            handNumber = 0;
            boardNumber = 0;
            if (!CanPurchase() && totalPower == 0 && hand.Count == 0 || bypass)
            {
                int nbCard = board.Count;
                for (int i = 0; i < nbCard; i++)
                {
                    if (!board[0].shipOrBase)
                    {
                        board[0].gameObject.transform.SetParent(objectDiscardPile.transform);
                        board[0].objectToMove = objectDiscardPile;
                        board[0].gameObject.GetComponent<BoxCollider>().enabled = true;
                        board[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                        board[0].objectToMove = objectDiscardPile.transform.GetChild(0).gameObject;
                        board[0].isUsed = false;
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
                foreach (ShipManager sm in objectBoard.GetComponentsInChildren<ShipManager>())
                {
                    if (discardPile.Contains(sm.hisCard))
                    {
                        Destroy(sm.objectToMove);
                        Destroy(sm.gameObject);
                    }
                }
                GameManager.EndTurn();
            }else if( hand.Count > 0 )
            {
                GameManager.popUpEndTurn.GetComponent<PopUpEndTurn>().Activate("You still have cards in your hand.\nAre you sure you want to skip your turn?");
            }
            else if( CanPurchase() )
            {
                GameManager.popUpEndTurn.GetComponent<PopUpEndTurn>().Activate("You can at least buy one more ship/base.\nAre you sure you want to skip your turn?");
            }
            else if( totalPower!=0 )
            {
                GameManager.popUpEndTurn.GetComponent<PopUpEndTurn>().Activate("You can still attack.\nAre you sure you want to skip your turn?");
            }
                
        }
        public void Attack(GameObject target)
        {
            if (target.name == enemyObject.name)
            {
                if (CanAttackPlayer())
                {
                    _enemy.hp -= totalPower;
                    _enemy.objectInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = _enemy.hp+"\nHP";
                    objectInfo.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = _enemy.hp+"\nHP";
                    totalPower = 0;
                    objectInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = totalPower+" P";
                    if (_enemy.hp <= 0)
                        GameManager.popUpEndTurn.GetComponent<PopUpEndTurn>().Activate("You Win !");
                }
                else
                    GameManager.popUpEndTurn.GetComponent<PopUpEndTurn>().Activate("You must first destroy the taunt base before attacking the player");
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
                    GameManager.popUpEndTurn.GetComponent<PopUpEndTurn>().Activate("You must first destroy the taunt base");
                }
            }
        }
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
            for (int i = 1 ; i < discardPile.Count; i++)
            {
                objectDiscardPile.transform.GetChild(1).SetParent(objectDeck.transform);
            }
            deck = new List<Card>(discardPile);
            foreach (Card c in deck)
            {
                c.objectToMove = objectDeck;
            }
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
        public void ShuffleDeck()
        {
            int nbCard = deck.Count;
            List<Card> tempList = new List<Card>(deck);
            deck.Clear();
            for (int i = 0; i < nbCard; i++)
            {
                int rnd = Random.Range(0, nbCard - i);
                deck.Add(tempList[rnd]);
                tempList.RemoveAt(rnd);
                objectDeck.transform.GetChild(rnd).SetParent(objectDeck.transform);
            }
        }
        public void AttackBase(Card targetCard, GameObject target)
        {
            if(targetCard.baseLife <= totalPower)
            {
                Destroy(target);
                totalPower -= targetCard.baseLife;
                objectInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = totalPower+" P";
            }
            else
                Debug.Log("You don't have enough power");
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
            card.transform.localScale = new Vector3(0.7f,0.7f,1);
            _shop.Refill(card);
        }
        public bool CanPurchase()
        {
            foreach (Card c in _shop.display)
            {
                if (c.cost <= money)
                {
                    return true ;
                }
            }
            if (_shop.explorer.Count>0)
            {
                if (_shop.explorer[0].cost <= money)
                {
                    return true;
                }
            }
            return false;
        }
    }
}