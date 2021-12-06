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
        
        public void Activate(Card c, Dictionary<Effect,int> lesEffets, bool HasOr)
        {
            gameObject.SetActive(true);
            cardFrom = c;
            effect.Clear();
            effect.Add(lesEffets.Keys.ToList()[0],lesEffets.Values.ToList()[0]);
            SetUp(HasOr);
        }

        void SetUp(bool HasOr)
        {
            if (!HasOr)
            {
                GameObject.Find("Text_AutoScrap").transform.GetChild(0).GetComponent<Text>().text = effect.Values.First() + " " + effect.Keys.First();
            }
            else
            {
                string explication = "";
                foreach (KeyValuePair<Effect, int> k in effect)
                {
                    explication += k.Value + " " + k.Key;
                    explication += " or ";
                }
                GameObject.Find("Text_AutoScrap").transform.GetChild(0).GetComponent<Text>().text = explication;
            }
            
        }
        
        public void Validate()
        {
            cardFrom.DoEffect(effect);
            gameObject.SetActive(false);
        }
    }
}
