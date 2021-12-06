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
        private static bool firstRound;

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
            BeginTurn();
        }

        //Distribue les carte selon si c'est le premier tour ou non
        public static void BeginTurn()
        {

            //pioche de debut de tour
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

        public static void endTurn()
        {
           //supression de la money et du power restan 
            currentPlayer.money = 0;
            currentPlayer.totalPower = 0;

        //echange de borad (visuelle)
            Vector3 boardpos1 = player1.objectBoard.GetComponent<Transform>().position;
            Vector3 boardpos2 = player2.objectBoard.GetComponent<Transform>().position;
            player1.objectBoard.GetComponent < Transform >().position = boardpos2;
            player2.objectBoard.GetComponent < Transform >().position = boardpos1;
            
            //switche du current player et des hp de ce dernier
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
