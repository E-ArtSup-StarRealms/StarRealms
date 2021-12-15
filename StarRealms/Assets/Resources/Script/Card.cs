using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Cache = UnityEngine.Cache;

namespace Resources.Script
{
    public class Card : MonoBehaviour
    {
        private static int _nbCard;
        public int id;
        public new string name = "";
        public int cost;
        public Faction faction = Faction.Bleu;
        public bool shipOrBase;
        public bool isTaunt;
        public Sprite icon;
        public Mesh image;
        public Dictionary<List<Condition>, Dictionary<Effect, int>> Actions = new Dictionary<List<Condition>,
            Dictionary<Effect, int>>();
        public int baseLife;
        public bool isUsed;
        public bool needPlayer;
        public Card mySelf;
        public float timer;
        public GameObject objectToMove;
        public bool draged;
        public Object model3D;
        public Object vaisseauBoard;
        public int handPos;
        
        private int _rankCond;
        private float _currentTime;
        private bool _overBoard;
        
        void Update()
        {
            if (! draged && gameObject.activeSelf)
            {
                if (objectToMove.transform.position != transform.position || objectToMove.transform.rotation != transform.rotation)
                {
                    _currentTime += Time.deltaTime;
                    float percent = _currentTime / timer;
                    transform.position = Vector3.Lerp(transform.position, objectToMove.transform.position, percent);
                    transform.rotation = Quaternion.Lerp(transform.rotation, objectToMove.transform.rotation, percent);
                }
                else
                {
                    _currentTime = 0;
                }
            }
        }
        private void OnMouseDown()
        {
            if ((ContainsWithId(GameManager.CurrentPlayer.shopObject.GetComponent<Shop>().display) ||
                ContainsWithId(GameManager.CurrentPlayer.shopObject.GetComponent<Shop>().explorer)) &&
                !GameManager.IsPopUpActivated())
            { 
                GameManager.CurrentPlayer.Buy(this);
            }
        }
        private void OnMouseUp()
        {
            if (draged)
            {
                if (_overBoard)
                {
                    vaisseauBoard = Instantiate(model3D, new Vector3(),new Quaternion());
                    vaisseauBoard.GetComponent<ShipManager>().objectToMove = 
                        GameManager.CurrentPlayer.objectBoard.transform.GetChild(0).transform.GetChild(0)
                            .transform.GetChild(GameManager.CurrentPlayer.boardNumber).gameObject;
                    vaisseauBoard.GetComponent<ShipManager>().objectToMove.SetActive(true);
                    vaisseauBoard.GetComponent<Transform>().SetParent(vaisseauBoard.GetComponent<ShipManager>().objectToMove.transform);
                    GameManager.CurrentPlayer.boardNumber++;
                    vaisseauBoard.GetComponent<ShipManager>().hisCard = this;
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                    transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    transform.SetParent(vaisseauBoard.GetComponent<ShipManager>().objectToMove.transform);
                    GameManager.CurrentPlayer.objectHand.transform.GetChild(0).transform.GetChild(0).GetComponent<ListNavigation>().Actualisation(objectToMove,this);
                    GameManager.CurrentPlayer.PlayCard(this);
                    PlaySelf();
                }
                draged = false;
            }
            GameManager.CurrentPlayer.objectBoard.transform.GetChild(1).gameObject.SetActive(false);
          
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("board"))
            {
                _overBoard = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("board"))
            {
                _overBoard = false;
            }
        }
        private void OnMouseDrag()
        {
            GameManager.CurrentPlayer.objectBoard.transform.GetChild(1).gameObject.SetActive(true);
            if (!GameManager.PopUpPlayerChoice.activeSelf && !isUsed && GameManager.CurrentPlayer.hand.Contains(this))
            {
                draged = true;
                Vector3 position = transform.position;
                int signe = 1;
                if (GameManager.CurrentPlayer == GameManager.Player1)
                {
                    signe = 1;
                }
                else
                {
                    signe = -1;
                }
                position = new Vector3(position.x + signe * Input.GetAxis("Mouse X") / 4, position.y + Input.GetAxis("Mouse Y") / 4.5f, position.z);
                transform.position = position;
            }
        }
        public List<Condition> GetNextCond(bool clicked)
        {
            List<Condition> firstCond = new List<Condition>();
            int thisRank = 0;
            if (!isUsed)
            {
                foreach (KeyValuePair<List<Condition>, Dictionary<Effect, int>> conds in Actions)
                {
                    if (thisRank == _rankCond)
                    {
                        if (!conds.Key.Contains(Condition.Or) && !conds.Key.Contains(Condition.AutoScrap))
                        {
                            if (!Actions[conds.Key].ContainsKey(Effect.Discard) &&
                                !Actions[conds.Key].ContainsKey(Effect.Hinder) &&
                                !Actions[conds.Key].ContainsKey(Effect.Scrap) &&
                                !Actions[conds.Key].ContainsKey(Effect.BaseDestruction) &&
                                !Actions[conds.Key].ContainsKey(Effect.DiscardToDraw))
                            {
                                firstCond = conds.Key;
                                _rankCond++;
                            } else if (clicked)
                            {
                                firstCond = conds.Key;
                                _rankCond++;
                            }
                        }
                        else if (clicked)
                        {
                            firstCond = conds.Key;
                            _rankCond++;
                        }
                        
                        return firstCond;
                    }
                    else
                    {
                        thisRank++;
                    }
                }
            }
            return firstCond;
        }
        public void SetId()
        {
            _nbCard++;
            id = _nbCard;
        }
        public void PlaySelf()
        {
            _rankCond = 0;
            CheckCondition(GetNextCond(false));
        }
        public void CancelPlay()
        {
            /*gameObject.transform.SetParent(GameManager.CurrentPlayer.objectHand.transform);
            ShipManager ship = FindMyShip();
            ship.objectToMove.SetActive(false);
            ship.Destruction();
            objectToMove = GameManager.CurrentPlayer.objectHand.transform.GetChild(0).transform.GetChild(0).transform.GetChild(handPos).gameObject;
            objectToMove.SetActive(true);
            gameObject.GetComponent<BoxCollider>().enabled = true;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            GameManager.CurrentPlayer.handNumber++;
            GameManager.CurrentPlayer.hand.Add(this);
            GameManager.CurrentPlayer.board.Remove(this);*/
        }
        public void CheckCondition(List<Condition> conds)
        {
            List<bool> allCheck = new List<bool>();
            foreach (Condition cond in conds)
            {
                switch (cond)
                {
                    case Condition.Nothing:
                        allCheck.Add(true);
                        break;
                    case Condition.Synergie:
                        if (CheckSynergie())
                        {
                            allCheck.Add(true);
                        }
                        break;
                    case Condition.ForEachSameFaction:
                        for (int i = 0; i < CheckNbSameFactionOnBoard(); i++)
                        {
                            DoEffect(Actions[conds]);
                        }
                        break;
                    case Condition.TwoBaseOrMore:
                        if (CheckNbBaseOnBoard() >= 2)
                        {
                            allCheck.Add(true);
                        }
                        break;
                }
            }
            if (!allCheck.Contains(false) && allCheck.Contains(true))
            {
                DoEffect(Actions[conds]);
            }
        }
        public void DoEffect(Dictionary<Effect, int> lesEffets)
        {
            foreach (KeyValuePair<Effect, int> e in lesEffets)
            {
                switch (e.Key)
                {
                    case Effect.Copy:
                        GameManager.PopUpPlayerChoice.GetComponent<PopUpManager>().Activate(this,Zone.Board,e.Key,e.Value);
                        break;
                    case Effect.D:
                        AddValue(Effect.D,e.Value);
                        GameManager.CurrentPlayer.objectInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>()
                            .text = GameManager.CurrentPlayer.totalPower + " P";
                        break;
                    case Effect.Discard:
                        GameManager.PopUpPlayerChoice.GetComponent<PopUpManager>().Activate(this,Zone.Hand,e.Key,e.Value);
                        break;
                    case Effect.Draw:
                        GameManager.CurrentPlayer.Draw(e.Value);
                        break;
                    case Effect.G:
                        AddValue(Effect.G,e.Value);
                        GameManager.CurrentPlayer.objectInfo.transform.GetChild(2).GetChild(0).GetComponent<Text>()
                            .text = GameManager.CurrentPlayer.money + " $";
                        break;
                    case Effect.H:
                        AddValue(Effect.H,e.Value);
                        GameManager.CurrentPlayer.objectInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>()
                            .text = GameManager.CurrentPlayer.hp + "\nHP";
                        if(GameManager.CurrentPlayer == GameManager.Player1)
                            GameManager.Player2.objectInfo.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = GameManager.Player1.hp+"\nHP";
                        else
                            GameManager.Player1.objectInfo.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = GameManager.Player2.hp+"\nHP";
                        break;
                    case Effect.Hinder:
                        GameManager.PopUpPlayerChoice.GetComponent<PopUpManager>().Activate(this,Zone.Display,e.Key,e.Value);
                        break;
                    case Effect.Requisition:
                        Requisition();
                        break;
                    case Effect.Sabotage:
                        TargetDiscard();
                        break;
                    case Effect.Scrap:
                        GameManager.PopUpPlayerChoice.GetComponent<PopUpManager>().Activate(this,Zone.HandAndDiscardPile,e.Key,e.Value);
                        break;
                    case Effect.Wormhole:
                        GameManager.CurrentPlayer.cardOnTop = true;
                        break;
                    case Effect.BaseDestruction:
                        GameManager.PopUpPlayerChoice.GetComponent<PopUpManager>().Activate(this,Zone.EnnemyBoardBase,e.Key,e.Value);
                        break;
                    case Effect.DiscardToDraw:
                        GameManager.PopUpPlayerChoice.GetComponent<PopUpManager>().Activate(this,Zone.Hand,e.Key,e.Value);
                        break;
                    case Effect.AllShipOneMoreDamage:
                        foreach (Card card in GameManager.CurrentPlayer.board)
                        {
                            if (!card.shipOrBase && HaveIDamage(card))
                            {
                                card.AddValue(Effect.D,1);
                            } 
                        }
                        break;
                    case Effect.MultiFaction:
                        faction = Faction.All;
                        break;
                }
            }
            if (_rankCond == Actions.Keys.Count)
            {
                isUsed = true;
            }
            else
            {
                CheckCondition(GetNextCond(false));
            }
        }
        public void DiscardToDraw(int nbDiscard)
        {
            GameManager.CurrentPlayer.Draw(nbDiscard);
        }
        public bool CheckSynergie()
        {
            foreach (Card c in GameManager.CurrentPlayer.board)
            {
                if ((c.faction == faction || c.faction == Faction.All) && c != this)
                {
                    return true;
                }
            }
            return false;
        }
        public int CheckNbSameFactionOnBoard()
        {
            int nbSameFaction = 0;
            foreach (Card c in GameManager.CurrentPlayer.board)
            {
                if (c.faction == faction || c.faction == Faction.All)
                {
                    nbSameFaction++;
                }
            }
            return nbSameFaction;
        }
        public void AddValue(Effect type, int value)
        {
            switch (type)
            {
                case Effect.H:
                    GameManager.CurrentPlayer.hp += value;
                    break;
                case Effect.D:
                    GameManager.CurrentPlayer.totalPower += value;
                    break;
                case Effect.G:
                    GameManager.CurrentPlayer.money += value;
                    break;
            }
        }
        public void ScrapOrDiscard(bool scrapOrDiscard, List<Card> lesCartes)
        {
            if (!scrapOrDiscard)
            {
                foreach (Card c in lesCartes)
                {
                    if (GameManager.CurrentPlayer.hand.Contains(c))
                    {
                        GameManager.CurrentPlayer.hand.Remove(c);
                    }else if (GameManager.CurrentPlayer.discardPile.Contains(c))
                    {
                        GameManager.CurrentPlayer.discardPile.Remove(c);
                    } else if (GameManager.CurrentPlayer.shopObject.GetComponent<Shop>().display.Contains(c))
                    {
                        GameManager.CurrentPlayer.shopObject.GetComponent<Shop>().Refill(c);
                        GameManager.CurrentPlayer.shopObject.GetComponent<Shop>().display.Remove(c);
                    }
                    Destroy(c.gameObject);
                }
            }
            else
            {
                foreach (Card c in lesCartes)
                {
                    GameManager.CurrentPlayer.hand.Remove(c);
                    GameManager.CurrentPlayer.discardPile.Add(c);
                }
            }
        }
        public void TargetDiscard()
        {
            if (GameManager.CurrentPlayer == GameManager.Player1)
            {
                GameManager.Player2.toDiscard += 1;
            } else if (GameManager.CurrentPlayer == GameManager.Player2)
            {
                GameManager.Player1.toDiscard += 1;
            }
        }
        public void DestroyTargetBase(Card card)
        {
            Destroy(card);
        }
        public void Requisition()
        {
            GameManager.CurrentPlayer.freeShip = true;
        }
        public int CheckNbBaseOnBoard()
        {
            int nbBase = 0;
            foreach (Card c in GameManager.CurrentPlayer.board)
            {
                if (c.shipOrBase)
                {
                    nbBase++;
                }
            }
            return nbBase;
        }
        public void Copy(Card card)
        {
            name = card.name;
            cost = card.cost;
            faction = card.faction;
            shipOrBase = card.shipOrBase;
            isTaunt = card.isTaunt;
            icon = card.icon;
            image = card.image;
            baseLife = card.baseLife;
            isUsed = card.isUsed;
            needPlayer = card.needPlayer;

            List<Condition> cond = FindConditionsOfEffect(Effect.Copy);
            Actions.Remove(cond);
            foreach (KeyValuePair<List<Condition>, Dictionary<Effect, int>> k in card.Actions)
            {
                Actions.Add(k.Key,k.Value);
            }
            CheckCondition(GetNextCond(false));
        }
        public void UnCopy()
        {
            name = mySelf.name;
            cost = mySelf.cost;
            faction = mySelf.faction;
            shipOrBase = mySelf.shipOrBase;
            isTaunt = mySelf.isTaunt;
            icon = mySelf.icon;
            image = mySelf.image;
            Actions = mySelf.Actions;
            baseLife = mySelf.baseLife;
            isUsed = mySelf.isUsed;
            needPlayer = mySelf.needPlayer;
        }
        public List<Condition> FindConditionsOfEffect(Effect e)
        {
            foreach ( KeyValuePair<List<Condition>, Dictionary<Effect, int>> k in Actions)
            {
                foreach (KeyValuePair<Effect, int> j in k.Value)
                {
                    if (j.Key == e)
                    {
                        return k.Key;
                    }
                }
            }
            List<Condition> resultat = new List<Condition>();
            resultat.Add(Condition.NotFound);
            return resultat;
        }
        public bool HaveIDamage(Card c)
        {
            foreach (KeyValuePair<List<Condition>, Dictionary<Effect, int>> k in c.Actions)
            {
                foreach (KeyValuePair<Effect, int> l in k.Value)
                {
                    if (l.Key == Effect.D)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool HaveIThisCondition(Condition condition)
        {
            foreach (List<Condition> lesConds in Actions.Keys)
            {
                foreach (Condition cond in lesConds)
                {
                    if (cond == condition)
                        return true;
                }
            }
            return false;
        }
        public List<Condition> GetListCondsFromCondition(Condition condition)
        {
            foreach (List<Condition> lesConds in Actions.Keys)
            {
                foreach (Condition cond in lesConds)
                {
                    if (cond == condition)
                        return lesConds;
                }
            }
            return new List<Condition>();
        }
        public ShipManager FindMyShip()
        {
            ShipManager monShip = null;
            List<ShipManager> lesShips = GameManager.CurrentPlayer.objectBoard.GetComponentsInChildren<ShipManager>().ToList();
            foreach (ShipManager sm in lesShips)
            {
                if (sm.hisCard.id == id)
                {
                    monShip = sm;
                }
            }
            Debug.Log(monShip.name);
            return monShip;
        }

        public bool ContainsWithId(List<Card> listCard)
        {
            foreach (Card card in listCard)
            {
                if (card.id == id)
                    return true;
            }
            return false;
        }
    }
}