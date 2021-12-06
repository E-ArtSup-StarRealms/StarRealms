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
            if (!HasOr)
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
                Debug.Log(effect.Count);
                foreach (KeyValuePair<Effect, int> k in effect)
                {
                    explication += k.Value + " " + k.Key;
                    explication += " or ";
                }
                GameObject.Find("Text_AutoScrap").GetComponent<Text>().text = explication;
            }
            
        }
        
        public void Validate()
        {
            cardFrom.DoEffect(effect);
            gameObject.SetActive(false);
        }
    }
}
