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
        
        public void Activate(Card c, Dictionary<Effect,int> lesEffets)
        {
            gameObject.SetActive(true);
            cardFrom = c;
            effect.Clear();
            effect.Add(lesEffets.Keys.ToList()[0],lesEffets.Values.ToList()[0]);
            SetUp();
        }

        void SetUp()
        {
            GameObject.Find("Text_AutoScrap").transform.GetChild(0).GetComponent<Text>().text = effect.Values.First() + " " + effect.Keys.First();
        }
        
        public void Validate()
        {
            cardFrom.DoEffect(effect);
            gameObject.SetActive(false);
        }
    }
}
