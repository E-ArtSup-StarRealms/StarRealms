using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Resources.Script
{
    public class CardCreator : MonoBehaviour
    {
        public TextAsset textJson;
        
        [Serializable]
        public class Actions
        {
            public string[] conditions;
            public string[] effects;
        }
        [Serializable]
        public class Cards
        {
            public string name;
            public int cost;
            public string faction;
            public bool shipOrBase;
            public bool isTaunt;
            public string icon;
            public string mesh;
            public List<Actions> actions;
            public int baseLife;
            public bool isUsed;
            public bool needPlayer;
            public int nbExemplaire;
        }
        [Serializable]
        public class CardsList
        {
            public Cards[] card;
        }
        public CardsList myCardsList = new CardsList();
        void Start()
        {
            myCardsList = JsonUtility.FromJson<CardsList>(textJson.text);
            float posX = 4.75f;
            float posY = 2f;
            foreach (Cards c in myCardsList.card)
            {
                if (posX < -4.6f)
                {
                    posX = 4.75f;
                    posY -= 2f;
                }

                for (int i = 0; i < c.nbExemplaire; i++)
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

                    Vector3 where = new Vector3(posX, posY, 0);
                    finalObject.GetComponent<Transform>().position = where;
                    finalObject.name = c.name;
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
                        case "Neutre":
                            finalCard.faction = Faction.Neutre;
                            break;
                        case "All":
                            finalCard.faction = Faction.All;
                            break;
                    }

                    finalCard.shipOrBase = c.shipOrBase;
                    finalCard.isTaunt = c.isTaunt;
                    //finalCard.image = c.mesh;
                    //finalCard.icon = c.icon;
                    foreach (Actions a in c.actions)
                    {
                        List<Char> lesChars = new List<char>();
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
                                case "":
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
                                string sMontant = effet[0].ToString();
                                montant = Convert.ToInt32(sMontant);
                                effet = effet.Substring(1);
                            }

                            switch (effet)
                            {
                                case "D":
                                    lesEfs.Add(Effect.D, montant);
                                    break;
                                case "Discard":
                                    lesEfs.Add(Effect.Discard, montant);
                                    break;
                                case "Draw":
                                    lesEfs.Add(Effect.Draw, montant);
                                    break;
                                case "G":
                                    lesEfs.Add(Effect.G, montant);
                                    break;
                                case "H":
                                    lesEfs.Add(Effect.H, montant);
                                    break;
                                case "Sabotage":
                                    lesEfs.Add(Effect.Sabotage, montant);
                                    break;
                                case "Wormhole":
                                    lesEfs.Add(Effect.Wormhole, montant);
                                    break;
                                case "Hinder":
                                    lesEfs.Add(Effect.Hinder, montant);
                                    break;
                                case "Requisition":
                                    lesEfs.Add(Effect.Requisition, montant);
                                    break;
                                case "Scrap":
                                    lesEfs.Add(Effect.Scrap, montant);
                                    break;
                                case "Copy":
                                    lesEfs.Add(Effect.Copy, montant);
                                    break;
                                case "DiscardToDraw":
                                    lesEfs.Add(Effect.DiscardToDraw, montant);
                                    break;
                                case "AllShipOneMoreDamage":
                                    lesEfs.Add(Effect.AllShipOneMoreDamage, montant);
                                    break;
                                case "BaseDestruction":
                                    lesEfs.Add(Effect.BaseDestruction, montant);
                                    break;
                                case "MultiFaction":
                                    lesEfs.Add(Effect.MultiFaction, montant);
                                    break;
                            }
                        }

                        finalCard.Actions.Add(lesConds, lesEfs);
                    }

                    finalCard.baseLife = c.baseLife;
                    finalCard.needPlayer = c.needPlayer;
                    finalCard.isUsed = c.isUsed;
                    switch (finalCard.name)
                    {
                        case "Scout":
                            if(GameManager.Player1.deck.Count<8)
                                GameManager.Player1.deck.Add(finalCard);
                            else
                                GameManager.Player2.deck.Add(finalCard);
                            break;
                        case "Viper":
                            if(GameManager.Player1.deck.Count<10)
                                GameManager.Player1.deck.Add(finalCard);
                            else
                                GameManager.Player2.deck.Add(finalCard);
                            break;
                        case "Explorer":
                            GameManager.CurrentPlayer.shopObject.GetComponent<Shop>().explorer.Add(finalCard);
                            break;
                        default:
                            Shop.GameDeck.Add(finalCard);
                            break;
                    }
                }
                posX -= 0.85f;
            }
            Shop.ShuffleGameDeck();
            GameManager.CurrentPlayer.shopObject.GetComponent<Shop>().Startfill();
            GameManager.BeginTurn();
        }
    }
}
