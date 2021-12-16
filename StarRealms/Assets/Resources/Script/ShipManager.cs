using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Resources.Script
{
    public class ShipManager : MonoBehaviour
    {
        public GameObject objectToMove;
        private float _currentTime;
        public float timer;
        public Card hisCard;
        
        void Update()
        {
            if (objectToMove.transform.position != transform.position || objectToMove.transform.rotation != transform.rotation)
            {
                _currentTime += Time.deltaTime;
                float percent = _currentTime / timer;
                transform.position = Vector3.Lerp(transform.position, objectToMove.transform.position, percent);
                transform.rotation = Quaternion.Lerp(transform.rotation, objectToMove.transform.rotation, percent);
            }
            else
            {
                _currentTime = 0;
            }
        }

        private void OnMouseOver()
        {
            
        }

        private void OnMouseDown()
        {
            if (!hisCard.isUsed && GameManager.currentPlayer.board.Contains(hisCard))
            {
                if(hisCard.HaveIThisCondition(Condition.AutoScrap))
                {
                    GameManager.popUpAutoScrap.GetComponent<PopUpAutoScrap>().
                        Activate(hisCard,hisCard.Actions[hisCard.GetListCondsFromCondition(Condition.AutoScrap)],
                            hisCard.GetListCondsFromCondition(Condition.AutoScrap).Contains(Condition.Or));
                } else if (hisCard.HaveIThisCondition(Condition.Or))
                {
                    GameManager.popUpOr.GetComponent<PopUpOrManager>().
                        Activate(hisCard,hisCard.Actions[hisCard.GetListCondsFromCondition(Condition.Or)],false);
                }
            }
        }

        public void Destruction()
        {
            Destroy(gameObject);
        }
    }
}
