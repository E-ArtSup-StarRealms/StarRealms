using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Script
{
    public class ListNavigation : MonoBehaviour
    {
        public List<GameObject> lesPositions = new List<GameObject>();
        public Dictionary<GameObject, Card> lesElements = new Dictionary<GameObject, Card>();
        public int firstPos;
        public int finalPos;
        
        public void Initialisation()
        {
            firstPos = lesElements[lesPositions[0]].GetComponent<Card>().handPos;
            foreach (GameObject pos in lesPositions)
            {
                if (pos.activeSelf)
                {
                    finalPos = lesElements[pos].GetComponent<Card>().handPos;
                }
            }
            if (!lesPositions[lesPositions.Count - 1].activeSelf)
            {
                GameObject.Find("Btn_Next").GetComponent<Button>().interactable = false;
            } else if (finalPos == GameManager.CurrentPlayer.hand[GameManager.CurrentPlayer.hand.Count-1].handPos)
            {
                GameObject.Find("Btn_Next").GetComponent<Button>().interactable = false;
            }
            else
            {
                GameObject.Find("Btn_Next").GetComponent<Button>().interactable = true;
            }
            if (firstPos == 0)
            {
                GameObject.Find("Btn_Previous").GetComponent<Button>().interactable = false;
            }
            else
            {
                GameObject.Find("Btn_Previous").GetComponent<Button>().interactable = true;
            }
        }
        public void Suivant()
        {
            if (GameManager.CurrentPlayer.hand.Count-lesElements[lesPositions[0]].handPos > 4)
            {
                Card oldCard;
                GameObject oldPos = null;
                GameObject myOldPos;
                for (int i = 0; i < lesPositions.Count; i++)
                {
                    oldCard = lesElements[lesPositions[i]];
                    lesElements.Remove(lesPositions[i]);
                    lesElements.Add(lesPositions[i], findNextCard(oldCard.handPos));
                    myOldPos = oldCard.objectToMove;
                    if (oldPos != null)
                    {
                        oldCard.objectToMove = oldPos;
                    }
                    else
                    {
                        oldCard.objectToMove = GameObject.Find("Btn_Previous");
                        oldCard.gameObject.SetActive(false);
                    }
                    oldPos = myOldPos;
                }
                lesElements[lesPositions[3]].gameObject.SetActive(true);
                lesElements[lesPositions[3]].objectToMove = lesPositions[3];
                Initialisation();
            }
        }
        public void Precedent()
        {
            if (lesElements[lesPositions[0]].handPos > 0)
            {
                Card oldCard;
                GameObject oldPos = null;
                GameObject myOldPos;
                for (int i = lesPositions.Count; i > 0; i--)
                {
                    oldCard = lesElements[lesPositions[i-1]];
                    lesElements.Remove(lesPositions[i-1]);
                    lesElements.Add(lesPositions[i-1], findPreviousCard(oldCard.handPos));
                    myOldPos = oldCard.objectToMove;
                    if (oldPos != null)
                    {
                        oldCard.objectToMove = oldPos;
                    }
                    else
                    {
                        oldCard.objectToMove = GameObject.Find("Btn_Next");
                        oldCard.gameObject.SetActive(false);
                    }
                    oldPos = myOldPos;
                }
                lesElements[lesPositions[0]].gameObject.SetActive(true);
                lesElements[lesPositions[0]].objectToMove = lesPositions[0];
                Initialisation();
            }
        }
        public Card findNextCard(int handPos)
        {
            foreach (Card c in GameManager.CurrentPlayer.hand)
            {
                if (c.handPos == handPos + 1)
                {
                    return c;
                }
            }
            foreach (Card c in GameManager.CurrentPlayer.hand)
            {
                if (c.handPos == handPos)
                {
                    return c;
                }
            }
            return null;
        }
        public Card findPreviousCard(int handpos)
        {
            foreach (Card c in GameManager.CurrentPlayer.hand)
            {
                if (c.handPos == handpos - 1)
                {
                    return c;
                }
            }
            foreach (Card c in GameManager.CurrentPlayer.hand)
            {
                if (c.handPos == handpos)
                {
                    return c;
                }
            }
            return null;
        }
        public void Actualisation(GameObject pos, Card card)
        {
            if (firstPos == 0)
            {
                if (finalPos < 4)
                {
                    for (int i = card.handPos; i < GameManager.CurrentPlayer.hand.Count; i++)
                    {
                        findNextCard(i).objectToMove = lesPositions[i];
                    }
                    
                    foreach (Card c in GameManager.CurrentPlayer.hand)
                    {
                        if (card.handPos < c.handPos)
                        {
                            c.handPos--;
                        } 
                    }
                    for (int i = finalPos; i < lesPositions.Count; i++)
                    {
                        lesPositions[i].SetActive(false);
                    }
                    //Initialisation();
                }
            }
            else
            {
                
            }
        }
    }
}
