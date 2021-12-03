using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Script
{
    public class GameManager : MonoBehaviour
    {
        public static GameObject popUp;
        public static Player player1 ;
        public static Player player2;
        public static Player currentPlayer;

        [SerializeField]
        private GameObject panelWin;
        private bool firstRound;

        private void Awake()
        {
            popUp = GameObject.Find("PopUp");
            popUp.SetActive(false);
            player1 = GameObject.Find("Player1").GetComponent<Player>();
            player2 = GameObject.Find("Player2").GetComponent<Player>();
            currentPlayer = player1;
        }

        private void Start()
        {
            panelWin.SetActive(false);
            firstRound = true;
            //BeginTurn();
        }

        //Distribue les carte selon si c'est le premier tour ou non
        private void BeginTurn()
        {

           
           

            if(firstRound)
            {

                currentPlayer.Draw(3);
                firstRound = false;

            }
            else
            {

                currentPlayer.Draw(5);

            }

        }

        private void endTurn()
        {
            currentPlayer.money = 0;
            currentPlayer.totalPower = 0;
            if (currentPlayer = player1)
            {
             
                player1.hp = currentPlayer.hp;
                currentPlayer = player2;
            }
            else
            {

                player2.hp = currentPlayer.hp;
                currentPlayer = player1;
            }
            BeginTurn();
        }

    }
}
