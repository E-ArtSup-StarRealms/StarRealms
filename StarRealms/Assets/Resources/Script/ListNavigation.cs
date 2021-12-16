using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Script
{
    public class ListNavigation : MonoBehaviour
    {
        public int firstCardHp;
        public int lastCardHp;
        public List<GameObject> lesElements = new List<GameObject>();
        public GameObject btnNext;
        public GameObject btnPrevious;

        private void Initialize()
        {
            lesElements.Clear();
            foreach (Card c in GameManager.currentPlayer.hand)
            {
                if (c.objectToMove.activeSelf && c.handPos <= firstCardHp)
                {
                    firstCardHp = c.handPos;
                }
                if (c.objectToMove.activeSelf && c.handPos >= firstCardHp)
                {
                    lastCardHp = c.handPos;
                }
            }
            foreach (Card c in GameManager.currentPlayer.hand)
            {
                lesElements.Add(c.objectToMove);
            }
        }
        public void AddElement(Card c)
        {
            lesElements.Add(c.objectToMove);
            if (lesElements.Count-firstCardHp > 5)
            {
                c.objectToMove.SetActive(false);
                btnNext.GetComponent<Button>().interactable = true;
            }
            Initialize();
        }
        public void Next()
        {
            lesElements[firstCardHp].SetActive(false);
            firstCardHp++;
            lastCardHp++;
            lesElements[lastCardHp].SetActive(true);
            if (lastCardHp == GameManager.currentPlayer.hand.Count - 1)
            {
                btnNext.GetComponent<Button>().interactable = false;
            }
            if (firstCardHp != 0)
            {
                btnPrevious.GetComponent<Button>().interactable = true;
            }
        }
        public void Previous()
        {
            lesElements[lastCardHp].SetActive(false);
            lastCardHp--;
            firstCardHp--;
            lesElements[firstCardHp].SetActive(true);
            if (firstCardHp == 0)
            {
                btnPrevious.GetComponent<Button>().interactable = false;
            }
            if (lastCardHp != GameManager.currentPlayer.hand.Count - 1)
            {
                btnNext.GetComponent<Button>().interactable = true;
            }
        }
        public void FillGap(Card card)
        {
            lesElements.Remove(card.objectToMove);
            Destroy(card.objectToMove);
            int nbDisplay = 0;
            foreach (Card c in GameManager.currentPlayer.hand)
            {
                if (c.handPos >= card.handPos)
                {
                    if (c.handPos == lastCardHp)
                    {
                        lastCardHp--;
                    }
                    c.handPos--;
                }
            }
            while (firstCardHp != 0 && lastCardHp-firstCardHp < 4)
            {
                firstCardHp--;
                lesElements[firstCardHp].SetActive(true);
            }
            if(lastCardHp-firstCardHp < 4)
                foreach (GameObject go in lesElements)
                {
                    if (go.activeSelf)
                    {
                        nbDisplay++;
                    }
                    else if(nbDisplay < 5)
                    {
                        go.SetActive(true);
                        lastCardHp++;
                        nbDisplay++;
                    }
                }
            if (lastCardHp == GameManager.currentPlayer.hand.Count - 1)
            {
                btnNext.GetComponent<Button>().interactable = false;
            }
            if (firstCardHp == 0)
            {
                btnPrevious.GetComponent<Button>().interactable = false;
            }
        }
        public void Reset()
        {
            foreach (GameObject go in lesElements)
            {
                Destroy(go);
            }
            lesElements.Clear();
            firstCardHp = 0 ;
            lastCardHp = 0 ;
        }
    }
}
