using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Script
{
    public class PopUpAutoScrap : MonoBehaviour
    {
        public Card cardFrom;
        public Dictionary<Effect, int> effect = new Dictionary<Effect, int>();
        public bool isOr;
        
        public void Activate(Card c, Dictionary<Effect,int> lesEffets, bool hasOr)
        {
            gameObject.SetActive(true);
            cardFrom = c;
            effect.Clear();
            isOr = hasOr;
            if (!hasOr)
            {
                effect.Add(lesEffets.Keys.ToList()[0],lesEffets.Values.ToList()[0]);
            }
            else
            {
                foreach(KeyValuePair<Effect, int> k in lesEffets)
                {
                    effect.Add(k.Key,k.Value);
                }
            }
            SetUp();
        }
        void SetUp()
        {
            if (!isOr)
            {
                GameObject.Find("Text_AutoScrap").GetComponent<Text>().text = effect.Values.First() + " " + effect.Keys.First();
            }
            else
            {
                string explication = "";
                foreach (KeyValuePair<Effect, int> k in effect)
                {
                    explication += k.Value + " " + k.Key;
                    explication += " or ";
                }
                explication = explication.Substring(0, explication.Length - 4);
                GameObject.Find("Text_AutoScrap").GetComponent<Text>().text = explication;
            }
        }
        public void Validate()
        {
            if(isOr)
            {
                GameManager.popUpOr.GetComponent<PopUpOrManager>().
                    Activate(cardFrom,cardFrom.Actions[cardFrom.GetListCondsFromCondition(Condition.Or)]);
            }
            else
            {
                cardFrom.DoEffect(effect);
            }
            gameObject.SetActive(false);
        }
    }
}
