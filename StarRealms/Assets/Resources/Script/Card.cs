using System;
using System.Collections.Generic;
using System.Transactions;
using System.Xml;
using UnityEditor.Search;
using UnityEngine;

namespace Resources.Script
{
    public class Card : MonoBehaviour
    {
        private static int nbCard = 0;
        public int id;
        public new string name;
        public int cost;
        public Faction faction;
        public bool shipOrBase;
        public bool isTaunt;
        public Sprite icon;
        public Mesh image;
        public Dictionary<Condition, List<Effect>> Actions;
        public int baseLife;
        public bool isUsed;
        public bool needPlayer;
        public int montantD = 0;
        public int montantG = 0;
        public int montantH = 0;
        public int montantDraw = 0;
        public int montantScrap = 0;
        public Card mySelf = null;

        public Card()
        {
            
        }
        public Card(int id, string name, int cost, Faction faction, bool shipOrBase, bool isTaunt, Mesh image, Sprite icon, Dictionary<Condition,List<Effect>> actions, int baseLife,bool needPlayer, bool isUsed)
        {
            nbCard++;
            this.id = nbCard;
            this.name = name;
            this.cost = cost;
            this.faction = faction;
            this.shipOrBase = shipOrBase;
            this.isTaunt = isTaunt;
            this.icon = icon;
            this.image = image;
            this.Actions = actions;
            this.baseLife = baseLife;
            this.isUsed = isUsed;
            this.needPlayer = needPlayer;

            foreach (List<Effect> lesEffets in Actions.Values)
            {
                if (lesEffets.Contains(Effect.Copy))
                {
                    mySelf = GameObject.Instantiate(this);
                }
            }
        }
        public void PlaySelf()
        {
            GameManager.currentPlayer.PlayCard(this);
        }
        public void CheckCondition()
        {
            foreach (Condition cond in Actions.Keys)
            {
                switch (cond)
                {
                    case Condition.Nothing:
                        DoEffect(Actions[cond]);
                        break;
                    case Condition.Or:
                        /*choix du joueur*/
                        int choix = 0;
                        List<Effect> aFaire = new List<Effect>();
                        aFaire.Add(Actions[cond][choix]);
                        DoEffect(aFaire);
                        break;
                    case Condition.Synergie:
                        if (CheckFactionOnBoard())
                        {
                            DoEffect(Actions[cond]);
                        }
                        break;
                    case Condition.AutoScrap:
                        /*Confirmation player*/
                        DoEffect(Actions[cond]);
                        Destroy(this);
                        break;
                    case Condition.ForEachSameFaction:
                        for (int i = 0; i < CheckNbSameFactionOnBoard(); i++)
                        {
                            DoEffect(Actions[cond]);
                        }
                        break;
                    case Condition.TwoBaseOrMore:
                        if (CheckNbBaseOnBoard() >= 2)
                        {
                            DoEffect(Actions[cond]);
                        }
                        break;
                }
            }
        }
        public void DoEffect(List<Effect> lesEffets)
        {
            foreach (Effect e in lesEffets)
            {
                switch (e)
                {
                    case Effect.Copy:
                        Copy();
                        break;
                    case Effect.D:
                        AddValue(Effect.D,montantD);
                        break;
                    case Effect.Discard:
                        break;
                    case Effect.Draw:
                        GameManager.currentPlayer.Draw(montantDraw);
                        break;
                    case Effect.G:
                        AddValue(Effect.G,montantG);
                        break;
                    case Effect.H:
                        AddValue(Effect.H,montantH);
                        break;
                    case Effect.Hinder:
                        /*choix du joueur*/
                        break;
                    case Effect.Requisition:
                        Requisition();
                        break;
                    case Effect.Sabotage:
                        TargetDiscard();
                        break;
                    case Effect.Scrap:
                        ScrapOrDiscard(false,montantScrap,Zone.HandAndDiscardPile,false);
                        break;
                    case Effect.Wormhole:
                        GameManager.currentPlayer.cardOnTop = true;
                        break;
                    case Effect.BaseDestruction:
                        DestroyTargetBase();
                        break;
                    case Effect.DiscardToDraw:
                        /*choix du joueur*/
                        int nbDiscard = 0;
                        GameManager.currentPlayer.Draw(nbDiscard);
                        break;
                    case Effect.AllShipOneMoreDamage:
                        foreach (Card card in GameManager.currentPlayer.board)
                        {
                            if (!card.shipOrBase && card.montantD > 0)
                            {
                                card.AddValue(Effect.D,1);
                            } 
                        }
                        break;
                }
            }
        }
        public bool CheckFactionOnBoard()
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
        public void ScrapOrDiscard(bool scrapOrDiscard, int nbMax, Zone zone, bool mustDoIt)
        {
            switch(zone)
            {
                case Zone.Hand:
                    if (!scrapOrDiscard)
                    {
                        /*choix du joueur*/
                        Card c = new Card();
                        Destroy(c);
                    }
                    else
                    {
                        //GameManager.currentPlayer.Discard(nbMax);
                    }
                    break;
                case Zone.DiscardPile:
                    if (!scrapOrDiscard)
                    {
                        /*choix du joueur*/
                        Card c = new Card();
                        Destroy(c);
                    }
                    break;
                case Zone.HandAndDiscardPile:
                    if (!scrapOrDiscard)
                    {
                        /*choix du joueur*/
                        Card c = new Card();
                        Destroy(c);
                    }
                    break;
                case Zone.Shop:
                    if (!scrapOrDiscard)
                    {
                        /*choix du joueur*/
                        Card c = new Card();
                        Destroy(c);
                    }
                    break;
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
        public void DestroyTargetBase()
        {
            /*Choix du joueur*/
            Card card = new Card();
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
        public void Copy()
        {
            /*choix du joueur*/
            Card card = new Card();
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

            Condition cond = FindConditionOfEffect(Effect.Copy);
            Actions.Remove(cond);
            foreach (KeyValuePair<Condition, List<Effect>> k in card.Actions)
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
        private Condition FindConditionOfEffect(Effect e)
        {
            foreach ( KeyValuePair<Condition,List<Effect>> k in Actions)
            {
                foreach (Effect effect in k.Value)
                {
                    if (effect == e)
                    {
                        return k.Key;
                    }
                }
            }
            return Condition.NotFound;
        }
    }
}
