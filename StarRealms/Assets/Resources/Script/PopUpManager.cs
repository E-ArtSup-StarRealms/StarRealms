using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Script
{
    public class PopUpManager : MonoBehaviour
    {
        public Card cardFrom;
        public Zone zoneApplied;
        public Effect effectApplied;
        public int nbToSelect;
        public List<Card> LesCartes = new List<Card>();
        public bool isAutoScrap;
        public void Activate(Card c,Zone zone,Effect e, int nbSelect,bool isAs)
        {
            LesCartes.Clear();
            gameObject.SetActive(true);
            isAutoScrap = isAs;
            GameObject.Find("Text_Validate").GetComponent<Text>().text = "No Card";
            cardFrom = c;
            zoneApplied = zone;
            effectApplied = e;
            nbToSelect = nbSelect;
            SetUp();
        }

        public void onEnable()
        {
            GameObject.Find("Text_Validate").GetComponent<Text>().text = "No Card";
        }
        public void SetUp()
        {
            GameObject.Find("Explanation").GetComponent<Text>().text = effectApplied + " up to " + nbToSelect + " card(s)";
        }
        public void Validate()
        {
            
            switch (effectApplied)
            {
                case Effect.Copy:
                    if (LesCartes.Count > 0)
                    {
                        cardFrom.Copy(LesCartes[0]);
                        cardFrom.rankCond = 0;
                    }
                    break;
                case Effect.Discard:
                    if (LesCartes.Count < nbToSelect)
                    {
                        cardFrom.rankCond++;
                        cardFrom.ScrapOrDiscard(true, LesCartes,isAutoScrap);
                    }
                    break;
                case Effect.Scrap:
                    cardFrom.rankCond++;
                    cardFrom.ScrapOrDiscard(false,LesCartes,isAutoScrap);
                    break;
                case Effect.Hinder:
                    cardFrom.rankCond++;
                    cardFrom.ScrapOrDiscard(false,LesCartes,isAutoScrap);
                    break;
            }
            gameObject.SetActive(false);
        }
        public void Cancel()
        {
            //cardFrom.CancelPlay();
            gameObject.SetActive(false);
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && cardFrom != null)
            {
                if (Camera.main)
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var hit, 100.0f))
                    {
                        if (hit.transform != null)
                        {
                            if (hit.transform.gameObject.GetComponent<Card>() != null)
                            {
                                if (hit.transform.gameObject.GetComponent<Card>() != cardFrom)
                                {
                                    switch (zoneApplied)
                                    {
                                        case Zone.Board:
                                            if (GameManager.currentPlayer.board.Contains(hit.transform.gameObject.GetComponent<Card>()))
                                            {
                                                if (LesCartes.Count < nbToSelect)
                                                    LesCartes.Add(hit.transform.gameObject.GetComponent<Card>());
                                                else
                                                {
                                                    LesCartes.Remove(LesCartes[0]);
                                                    LesCartes.Add(hit.transform.gameObject.GetComponent<Card>());
                                                }
                                            }
                                            break;
                                        case Zone.Hand:
                                            if (GameManager.currentPlayer.hand.Contains(hit.transform.gameObject.GetComponent<Card>()))
                                            {
                                                if (LesCartes.Count < nbToSelect)
                                                    LesCartes.Add(hit.transform.gameObject.GetComponent<Card>());
                                                else
                                                {
                                                    LesCartes.Remove(LesCartes[0]);
                                                    LesCartes.Add(hit.transform.gameObject.GetComponent<Card>());
                                                }
                                            }
                                            break;
                                        case Zone.Display:
                                            if (GameManager.currentPlayer.shopObject.GetComponent<Shop>().display.
                                                Contains(hit.transform.gameObject.GetComponent<Card>()))
                                            {
                                                if (LesCartes.Count < nbToSelect)
                                                    LesCartes.Add(hit.transform.gameObject.GetComponent<Card>());
                                                else
                                                {
                                                    LesCartes.Remove(LesCartes[0]);
                                                    LesCartes.Add(hit.transform.gameObject.GetComponent<Card>());
                                                }
                                            }
                                            break;
                                        case Zone.DiscardPile:
                                            if (GameManager.currentPlayer.discardPile.Contains(hit.transform.gameObject.GetComponent<Card>()))
                                            {
                                                if (LesCartes.Count < nbToSelect)
                                                    LesCartes.Add(hit.transform.gameObject.GetComponent<Card>());
                                                else
                                                {
                                                    LesCartes.Remove(LesCartes[0]);
                                                    LesCartes.Add(hit.transform.gameObject.GetComponent<Card>());
                                                }
                                            }
                                            break;
                                        case Zone.HandAndDiscardPile:
                                            if (GameManager.currentPlayer.hand.Contains(hit.transform.gameObject.GetComponent<Card>()) ||
                                                GameManager.currentPlayer.discardPile.Contains(hit.transform.gameObject.GetComponent<Card>()))
                                            {
                                                if (LesCartes.Count < nbToSelect)
                                                    LesCartes.Add(hit.transform.gameObject.GetComponent<Card>());
                                                else
                                                {
                                                    LesCartes.Remove(LesCartes[0]);
                                                    LesCartes.Add(hit.transform.gameObject.GetComponent<Card>());
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (LesCartes.Count == 0)
                {
                    GameObject.Find("Text_Validate").GetComponent<Text>().text = "No Card";
                }
                else if (LesCartes.Count == 1)
                {
                    GameObject.Find("Text_Validate").GetComponent<Text>().text = LesCartes[0].name;
                }
                else
                {
                    GameObject.Find("Text_Validate").GetComponent<Text>().text = LesCartes.Count + " Cards";
                }
            }
        }
    }
}
