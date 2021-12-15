using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Script
{
    public class NavigationBoard : MonoBehaviour
    {
        public int firstCardBp;
        public int lastCardBp;
        public List<GameObject> lesElements = new List<GameObject>();
        public GameObject btnNext;
        public GameObject btnPrevious;

        private void Initialize()
        {
            lesElements.Clear();
            firstCardBp = GameManager.currentPlayer.board[0].handPos;
            lastCardBp = GameManager.currentPlayer.board[Mathf.Clamp(GameManager.currentPlayer.board.Count - 1,0, 4)].handPos;
            foreach (Card c in GameManager.currentPlayer.board)
            {
                lesElements.Add(c.objectToMove);
            }
        }
        public void AddElement(ShipManager s)
        {
            lesElements.Add(s.objectToMove);
            if (lesElements.Count-firstCardBp > 5)
            {
                s.objectToMove.SetActive(false);
                btnNext.GetComponent<Button>().interactable = true;
            }
            Initialize();
        }
        public void Next()
        {
            lesElements[firstCardBp].SetActive(false);
            firstCardBp++;
            lastCardBp++;
            lesElements[lastCardBp].SetActive(true);
            if (lastCardBp == GameManager.currentPlayer.board.Count - 1)
            {
                btnNext.GetComponent<Button>().interactable = false;
            }
            if (firstCardBp != 0)
            {
                btnPrevious.GetComponent<Button>().interactable = true;
            }
        }
        public void Previous()
        {
            lesElements[lastCardBp].SetActive(false);
            lastCardBp--;
            firstCardBp--;
            lesElements[firstCardBp].SetActive(true);
            if (firstCardBp == 0)
            {
                btnPrevious.GetComponent<Button>().interactable = false;
            }
            if (lastCardBp != GameManager.currentPlayer.board.Count - 1)
            {
                btnNext.GetComponent<Button>().interactable = true;
            }
        }
        public void FillGap(Card card)
        {
            lesElements.Remove(card.objectToMove);
            Destroy(card.objectToMove);
            int nbDisplay = 0;
            foreach (Card c in GameManager.currentPlayer.board)
            {
                if (c.handPos > card.handPos)
                {
                    if (c.handPos == lastCardBp)
                    {
                        lastCardBp--;
                    }
                    c.handPos--;
                }
            }
            while (firstCardBp != 0 && lastCardBp-firstCardBp < 4)
            {
                firstCardBp--;
                lesElements[firstCardBp].SetActive(true);
            }
            if(lastCardBp-firstCardBp < 4)
                foreach (GameObject go in lesElements)
                {
                    if (go.activeSelf)
                    {
                        nbDisplay++;
                    }
                    else if(nbDisplay < 5)
                    {
                        go.SetActive(true);
                        lastCardBp++;
                        nbDisplay++;
                    }
                }
            if (lastCardBp == GameManager.currentPlayer.board.Count - 1)
            {
                btnNext.GetComponent<Button>().interactable = false;
            }
            if (firstCardBp == 0)
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
            firstCardBp = 0 ;
            lastCardBp = 0 ;
        }
    }
}
