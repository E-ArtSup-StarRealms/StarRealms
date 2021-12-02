using System.Collections.Generic;
using UnityEngine;

namespace Resources.Script
{
    public class Card : MonoBehaviour
    {
        private static int _nbCard = 0;
        public int id;
        public new string name = "";
        public int cost = 0;
        public Faction faction = Faction.Bleu;
        public bool shipOrBase = false;
        public bool isTaunt = false;
        public Sprite icon;
        public Mesh image;
        public Dictionary<List<Condition>, Dictionary<Effect, int>> Actions = new Dictionary<List<Condition>,
            Dictionary<Effect, int>>();
        public int baseLife = 0;
        public bool isUsed = false;
        public bool needPlayer = false;
        public Card mySelf = null;

        void Start()
        {
            GameManager.currentPlayer.board.Add(this);
        }
        void Update()
        {
            transform.LookAt(GameObject.FindWithTag("MainCamera").transform.position - new Vector3(0,0,0));
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckCondition();
            }
        }
        public void SetId()
        {
            _nbCard++;
            id = _nbCard;
        }
        public void PlaySelf()
        {
            GameManager.currentPlayer.PlayCard(this);
        }
        public void CheckCondition()
        {
            foreach (List<Condition> conds in Actions.Keys)
            {
                foreach (Condition cond in conds)
                {
                    switch (cond)
                    {
                        case Condition.Nothing:
                            DoEffect(Actions[conds]);
                            break;
                        case Condition.Or:
                            /*choix du joueur*/
                            Effect choix = Effect.D;
                            Dictionary<Effect, int> aFaire = new Dictionary<Effect, int>();
                            aFaire.Add(choix,Actions[conds][choix]);
                            DoEffect(aFaire);
                            break;
                        case Condition.Synergie:
                            if (CheckSynergie())
                            {
                                DoEffect(Actions[conds]);
                            }

                            break;
                        case Condition.AutoScrap:
                            /*Confirmation player*/
                            DoEffect(Actions[conds]);
                            Destroy(this);
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
                                DoEffect(Actions[conds]);
                            }
                            break;
                    }
                }
            }
        }
        public void DoEffect(Dictionary<Effect, int> lesEffets)
        {
            foreach (KeyValuePair<Effect, int> e in lesEffets)
            {
                switch (e.Key)
                {
                    case Effect.Copy:
                        GameManager.popUp.GetComponent<PopUpManager>().Activate(this,Zone.Board,e.Key,e.Value);
                        break;
                    case Effect.D:
                        AddValue(Effect.D,e.Value);
                        break;
                    case Effect.Discard:
                        GameManager.popUp.GetComponent<PopUpManager>().Activate(this,Zone.HandAndDiscardPile,e.Key,e.Value);
                        break;
                    case Effect.Draw:
                        GameManager.currentPlayer.Draw(e.Value);
                        break;
                    case Effect.G:
                        AddValue(Effect.G,e.Value);
                        break;
                    case Effect.H:
                        AddValue(Effect.H,e.Value);
                        break;
                    case Effect.Hinder:
                        GameManager.popUp.GetComponent<PopUpManager>().Activate(this,Zone.Board,e.Key,e.Value);
                        break;
                    case Effect.Requisition:
                        Requisition();
                        break;
                    case Effect.Sabotage:
                        TargetDiscard();
                        break;
                    case Effect.Scrap:
                        GameManager.popUp.GetComponent<PopUpManager>().Activate(this,Zone.HandAndDiscardPile,e.Key,e.Value);
                        break;
                    case Effect.Wormhole:
                        GameManager.currentPlayer.cardOnTop = true;
                        break;
                    case Effect.BaseDestruction:
                        GameManager.popUp.GetComponent<PopUpManager>().Activate(this,Zone.EnnemyBoardBase,e.Key,e.Value);
                        break;
                    case Effect.DiscardToDraw:
                        GameManager.popUp.GetComponent<PopUpManager>().Activate(this,Zone.Hand,e.Key,e.Value);
                        break;
                    case Effect.AllShipOneMoreDamage:
                        foreach (Card card in GameManager.currentPlayer.board)
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
        }
        public void DiscardToDraw(int nbDiscard)
        {
            GameManager.currentPlayer.Draw(nbDiscard);
        }
        public bool CheckSynergie()
        {
            foreach (Card c in GameManager.currentPlayer.board)
            {
                if (c.faction == faction || c.faction == Faction.All)
                {
                    return true;
                }
            }
            return false;
        }
        public int CheckNbSameFactionOnBoard()
        {
            int nbSameFaction = 0;
            foreach (Card c in GameManager.currentPlayer.board)
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
                    GameManager.currentPlayer.hp += value;
                    break;
                case Effect.D:
                    GameManager.currentPlayer.totalPower += value;
                    break;
                case Effect.G:
                    GameManager.currentPlayer.money += value;
                    break;
            }
        }
        public void ScrapOrDiscard(bool scrapOrDiscard, List<Card> lesCartes)
        {
            if (!scrapOrDiscard)
            {
                foreach (Card c in lesCartes)
                {
                    Destroy(c);
                }
            }
            else
            {
                //GameManager.currentPlayer.Discard(nbMax);
            }
        }
        public void TargetDiscard()
        {
            if (GameManager.currentPlayer == GameManager.player1)
            {
                GameManager.player2.toDiscard += 1;
            } else if (GameManager.currentPlayer == GameManager.player2)
            {
                GameManager.player1.toDiscard += 1;
            }
        }
        public void DestroyTargetBase(Card card)
        {
            Destroy(card);
        }
        public void Requisition()
        {
            GameManager.currentPlayer.freeShip = true;
        }
        public int CheckNbBaseOnBoard()
        {
            int nbBase = 0;
            foreach (Card c in GameManager.currentPlayer.board)
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
            CheckCondition();
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
        private List<Condition> FindConditionsOfEffect(Effect e)
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
        private bool HaveIDamage(Card c)
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
    }
}
