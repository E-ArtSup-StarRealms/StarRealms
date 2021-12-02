using System.Collections.Generic;
using UnityEngine;

namespace Resources.Script
{
    public class CardCreator : MonoBehaviour
    {
        public TextAsset textJSON;
        
        [System.Serializable]
        public class Actions
        {
            public string[] conditions;
            public string[] effects;
        }
        [System.Serializable]
        public class Card
        {
            public string name;
            public int cost;
            public string faction;
            public bool shipOrBase;
            public bool isTaunt;
            public string icon;
            public string mesh;
            public List<Actions> Actions;
            public int baseLife;
            public bool isUsed;
            public bool needPlayer;
        }
        [System.Serializable]
        public class CardList
        {
            public Card[] card;
        }
        
        public CardList myCardList = new CardList();
        void Start()
        {
            myCardList = JsonUtility.FromJson<CardList>(textJSON.text);
        }
    }
}
