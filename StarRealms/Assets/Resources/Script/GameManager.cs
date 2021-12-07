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

        public static GameObject camP1;
        public static GameObject camP2;

        private void Awake()
        {
            camP1 = GameObject.Find("CM vcam1");
            camP2 = GameObject.Find("CM vcam1 (1)");
            camP1.SetActive(false);
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
         if(camP2.activeSelf == false)
            {
                GameManager.camP2.SetActive(true);
                GameManager.camP1.SetActive(false);
            }
            else
            {
                GameManager.camP1.SetActive(true);
                GameManager.camP2.SetActive(false);
            }

            //switche du current player et des hp de ce dernier
                if (CurrentPlayer == Player1)
            {
             
               CurrentPlayer = Player2;
            }
            else
            {
                CurrentPlayer = Player1;
            }
            BeginTurn();
        }
    }
}