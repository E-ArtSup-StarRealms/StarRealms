using UnityEngine;

namespace Resources.Script
{
    public class GameManager : MonoBehaviour
    {
        public static GameObject PopUp;
        public static GameObject PopUpOr;
        public static GameObject PopUpAutoScrap;
        public static Player Player1 ;
        public static Player Player2;
        public static Player CurrentPlayer;

        [SerializeField]
        private GameObject panelWin;
        private static bool _firstRound = true;

        private void Awake()
        {
            PopUp = GameObject.Find("PopUp");
            PopUpOr = GameObject.Find("PopUpOr");
            PopUpAutoScrap = GameObject.Find("PopUpAutoScrap");
            
            PopUp.SetActive(false);
            PopUpAutoScrap.SetActive(false);
            PopUpOr.SetActive(false);

            Player1 = GameObject.Find("Player1").GetComponent<Player>();
            Player2 = GameObject.Find("Player2").GetComponent<Player>();
            CurrentPlayer = Player1;
        }

        private void Start()
        {
            // I SLASHED PANEL WIN CAUSE I COULDNT FIND IT
             panelWin.SetActive(false);
            //BeginTurn();
        }

        //Distribue les carte selon si c'est le premier tour ou non
        public static void BeginTurn()
        {

            //pioche de debut de tour
            if(_firstRound)
            {
                CurrentPlayer.Draw(3);
                _firstRound = false;
            }
            else
            {
                CurrentPlayer.Draw(5);
            }
        }

        public static void EndTurn()
        {
            //supression de la money et du power restan 
            CurrentPlayer.money = 0;
            CurrentPlayer.totalPower = 0;

            //echange de borad (visuelle)
            Vector3 boardpos1 = Player1.objectBoard.GetComponent<Transform>().position;
            Vector3 boardpos2 = Player2.objectBoard.GetComponent<Transform>().position;
            Player1.objectBoard.GetComponent < Transform >().position = boardpos2;
            Player2.objectBoard.GetComponent < Transform >().position = boardpos1;
            
            //switche du current player et des hp de ce dernier
            if (CurrentPlayer == Player1)
            {
                Player1.hp = CurrentPlayer.hp;
                CurrentPlayer = Player2;
            }
            else
            {
                Player2.hp = CurrentPlayer.hp;
                CurrentPlayer = Player1;
            }
            BeginTurn();
        }
    }
}