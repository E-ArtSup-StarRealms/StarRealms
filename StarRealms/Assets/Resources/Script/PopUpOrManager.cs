using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Script
{
    public class PopUpOrManager : MonoBehaviour
    {
        public Card cardFrom;
        public Dictionary<Effect, int> choix1 = new Dictionary<Effect, int>();
        public Dictionary<Effect, int> choix2 = new Dictionary<Effect, int>();
        
        public void Activate(Card c, Dictionary<Effect,int> lesEffets)
        {
            gameObject.SetActive(true);
            cardFrom = c;
            choix1.Add(lesEffets.Keys.ToList()[0],lesEffets.Values.ToList()[0]);
            choix2.Add(lesEffets.Keys.ToList()[1],lesEffets.Values.ToList()[1]);
            SetUp();
        }

        void SetUp()
        {
            GameObject.Find("Btn_choix_1").transform.GetChild(0).GetComponent<Text>().text = choix1.Values.First() + " " + choix1.Keys.First();
            GameObject.Find("Btn_choix_2").transform.GetChild(0).GetComponent<Text>().text = choix2.Values.First() + " " + choix2.Keys.First();
        }
        
        public void Choix1()
        {
            cardFrom.DoEffect(choix1);
            gameObject.SetActive(false);
        }
        
        public void Choix2()
        {
            cardFrom.DoEffect(choix2);
            gameObject.SetActive(false);
        }
    }
}
