using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Resources.Script
{
    public class PopUpManager : MonoBehaviour
    {
        public Card cardFrom;
        public Zone zoneApplied;
        public Effect effectApplied;
        public int nbToSelect;
        public readonly List<Card> LesCartes = new List<Card>();
        public void Activate(Card c,Zone zone,Effect e, int nbSelect)
        {
            gameObject.SetActive(true);
            cardFrom = c;
            zoneApplied = zone;
            effectApplied = e;
            nbToSelect = nbSelect;
            SetUp();
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
                    Debug.Log(LesCartes.Count);
                    if (LesCartes.Count > 0)
                    {
                        cardFrom.Copy(LesCartes[0]);
                    }
                    break;
                case Effect.Discard:
                    cardFrom.ScrapOrDiscard(true,LesCartes);
                    break;
                case Effect.Scrap:
                    cardFrom.ScrapOrDiscard(false,LesCartes);
                    break;
                case Effect.Hinder:
                    cardFrom.ScrapOrDiscard(false,LesCartes);
                    break;
            }
            gameObject.SetActive(false);
        }
        public void Cancel()
        {
            cardFrom.CancelPlay();
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
                                            if (hit.transform.gameObject.GetComponent<Card>()
                                                .GetComponentInParent<Transform>().name == "Hand")
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
                                        case Zone.Shop:
                                            if (hit.transform.gameObject.GetComponent<Card>()
                                                .GetComponentInParent<Transform>().name == "Shop")
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
                                            if (hit.transform.gameObject.GetComponent<Card>()
                                                .GetComponentInParent<Transform>().name == "DiscardPile")
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
                                            if (hit.transform.gameObject.GetComponent<Card>()
                                                    .GetComponentInParent<Transform>().name == "Hand" ||
                                                hit.transform.gameObject.GetComponent<Card>()
                                                    .GetComponentInParent<Transform>().name == "DiscardPile")
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
                if (LesCartes.Count > 1)
                {
                    GameObject.Find("Text_Validate").GetComponent<Text>().text = LesCartes[0].name;
                }
                else
                {
                    GameObject.Find("Text_Validate").GetComponent<Text>().text = LesCartes.Count + " cards";
                }
            }
        }
    }
}
