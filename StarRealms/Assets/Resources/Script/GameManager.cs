using System.Collections.Generic;
using UnityEngine;

namespace Resources.Script
{
    public class GameManager : MonoBehaviour
    {
        public GameObject popUp;

        public static Player player1;
        public static Player player2;
        public static Player currentPlayer;

        [SerializeField]
        private GameObject panelWin;
        private bool firstRound;

        private void Start()
        {
            popUp.SetActive(true);
            panelWin.SetActive(false);
            firstRound = true;
            player1.hp = 50;
            player2.hp = 50;
            currentPlayer = player1;
            currentPlayer.money = 0;
            currentPlayer.totalPower = 0;
            BeginTurn();
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
                if(player2.hp <= 0 )
                {
                    panelWin.SetActive(true);
                }
                player1.hp = currentPlayer.hp;
                currentPlayer = player2;
            }
            else
            {
                if (player1.hp <= 0)
                {
                    panelWin.SetActive(true);
                }
                player2.hp = currentPlayer.hp;
                currentPlayer = player1;
            }
            
        }

    }
}
