using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Script
{
    public class Card : MonoBehaviour
    {
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
        public List<Effect> needPlayer;

        public Card(int id, string name, int cost, Faction faction, bool shipOrBase, bool isTaunt, Mesh image, Sprite icon, Dictionary<Condition,List<Effect>> actions, int baseLife,List<Effect> needPlayer, bool isUsed)
        {
            this.id = id;
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
        }
    
        public void DoEffect()
        {
        
        }
    
        public void CheckFactionOnBoard(){}
        public void AddValue(String type, int value){}
        public void Scrap(int nbMax,ScrapZone zone, bool mustScrap){}
        public void TargetDiscard(){}
        public void DestroyTargetBase(Card card){}
        public void Requisition(){}
        public void CheckNbBase(){}
        public void Copy(Card card){}
        public void ToDiscard(int maxDiscard){}
    }
}
