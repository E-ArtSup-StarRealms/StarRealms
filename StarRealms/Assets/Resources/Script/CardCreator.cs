using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources.Script
{
    public class CardCreator : MonoBehaviour
    {
        public TextAsset textJSON;
        
        [System.Serializable]
        public class Actions
        {
            public string[] conditions;
            public string[] effects;
        }
        [System.Serializable]
        public class Cards
        {
            public string name;
            public int cost;
            public string faction;
            public bool shipOrBase;
            public bool isTaunt;
            public string icon;
            public string mesh;
            public List<Actions> Actions;
            public int baseLife;
            public bool isUsed;
            public bool needPlayer;
        }
        [System.Serializable]
        public class CardsList
        {
            public Cards[] card;
        }
        public CardsList myCardsList = new CardsList();
        void Start()
        {
            myCardsList = JsonUtility.FromJson<CardsList>(textJSON.text);
            foreach (Cards c in myCardsList.card)
            {
                Object finalObject;
                if (!c.shipOrBase)
                {
                    finalObject = Instantiate(UnityEngine.Resources.Load("Prefab/carteShip"),
                        GameObject.Find("GameDeck").transform);
                }
                else
                {
                    finalObject = Instantiate(UnityEngine.Resources.Load("Prefab/carteBase"),
                        GameObject.Find("GameDeck").transform);
                }
                Card finalCard = finalObject.GetComponent<Card>();
                finalCard.SetId();
                finalCard.name = c.name;
                finalCard.cost = c.cost;
                switch (c.faction)
                {
                    case "Rouge":
                        finalCard.faction = Faction.Rouge;
                        break;
                    case "Vert":
                        finalCard.faction = Faction.Vert;
                        break;
                    case "Jaune":
                        finalCard.faction = Faction.Jaune;
                        break;
                    case "Bleu":
                        finalCard.faction = Faction.Bleu;
                        break;
                    case "All":
                        finalCard.faction = Faction.All;
                        break;
                }
                finalCard.shipOrBase = c.shipOrBase;
                finalCard.isTaunt = c.isTaunt;
                //finalCard.image = c.mesh;
                //finalCard.icon = c.icon;
                foreach (Actions a in c.Actions)
                {
                    List<char> lesChars = new List<char>();
                    lesChars.Add('2');
                    lesChars.Add('3');
                    lesChars.Add('4');
                    lesChars.Add('5');
                    lesChars.Add('6');
                    lesChars.Add('7');
                    lesChars.Add('8');
                    lesChars.Add('9');
                    List<Condition> lesConds = new List<Condition>();
                    Dictionary<Effect, int> lesEfs = new Dictionary<Effect, int>();
                    foreach (string sC in a.conditions)
                    {
                        switch (sC)
                        {
                            case "Nothing":
                                lesConds.Add(Condition.Nothing);
                                break;
                            case "Or":
                                lesConds.Add(Condition.Or);
                                break;
                            case "Synergie":
                                lesConds.Add(Condition.Synergie);
                                break;
                            case "AutoScrap":
                                lesConds.Add(Condition.AutoScrap);
                                break;
                            case "TwoBaseOrMore":
                                lesConds.Add(Condition.TwoBaseOrMore);
                                break;
                            case "ForEachSameFaction":
                                lesConds.Add(Condition.ForEachSameFaction);
                                break;
                        }
                    }
                    foreach (string sE in a.effects)
                    {
                        string effet = sE;
                        int montant = 1;
                        if (lesChars.Contains(effet[0]))
                        {
                            char sMontant = effet[0];
                            montant = sMontant;
                            effet = effet.Remove(0);
                        }
                        switch (effet)
                        {
                                case "D":
                                    lesEfs.Add(Effect.D,montant);
                                    break;
                                case "Discard":
                                    lesEfs.Add(Effect.Discard,montant);
                                    break;
                                case "Draw":
                                    lesEfs.Add(Effect.Draw,montant);
                                    break;
                                case "G":
                                    lesEfs.Add(Effect.G,montant);
                                    break;
                                case "H":
                                    lesEfs.Add(Effect.H,montant);
                                    break;
                                case "Sabotage":
                                    lesEfs.Add(Effect.Sabotage,montant);
                                    break;
                                case "Wormhole":
                                    lesEfs.Add(Effect.Wormhole,montant);
                                    break;
                                case "Hinder":
                                    lesEfs.Add(Effect.Hinder,montant);
                                    break;
                                case "Requisition":
                                    lesEfs.Add(Effect.Requisition,montant);
                                    break;
                                case "Scrap":
                                    lesEfs.Add(Effect.Scrap,montant);
                                    break;
                                case "Copy":
                                    lesEfs.Add(Effect.Copy,montant);
                                    break;
                                case "DiscardToDraw":
                                    lesEfs.Add(Effect.DiscardToDraw,montant);
                                    break;
                                case "AllShipOneMoreDamage":
                                    lesEfs.Add(Effect.AllShipOneMoreDamage,montant);
                                    break;
                                case "BaseDestruction":
                                    lesEfs.Add(Effect.BaseDestruction,montant);
                                    break;
                                case "MultiFaction":
                                    lesEfs.Add(Effect.MultiFaction,montant);
                                    break;
                        }
                    }
                    finalCard.Actions.Add(lesConds, lesEfs);
                }
                finalCard.baseLife = c.baseLife;
                finalCard.needPlayer = c.needPlayer;
                finalCard.isUsed = c.isUsed;
            }
        }
    }
}
