using UnityEngine;

namespace Resources.Script
{
    public class GameManager : MonoBehaviour
    {
        public static GameObject PopUpPlayerChoice;
        public static GameObject PopUpOr;
        public static GameObject PopUpAutoScrap;
        public static GameObject PopUpEndTurn;
        
        public static Player Player1 ;
        public static Player Player2;
        public static Player CurrentPlayer;

        [SerializeField]
        private GameObject panelWin;
        private static bool _firstRound = true;

        public static GameObject camP1;
        public static GameObject camP2;
        public static Vector3 pos1 = new Vector3(-0.28f, 0.5881766f, -10.71f);
        public static Vector3 pos2 = new Vector3(-0.11f, 0.72f, 14f);
        /*rotation : 11.687, -4.918, -7.369
          position : -1.299936, 0.4832786, 1.042565
         */
        public static GameObject shopObject;

        private void Awake()
        {
            shopObject = GameObject.Find("Shop");
            camP1 = GameObject.Find("CM vcam1");
            camP2 = GameObject.Find("CM vcam1 (1)");
            camP1.SetActive(false);
            PopUpPlayerChoice = GameObject.Find("PopUpPlayerChoice");
            PopUpOr = GameObject.Find("PopUpOr");
            PopUpAutoScrap = GameObject.Find("PopUpAutoScrap");
            PopUpEndTurn = GameObject.Find("PopUpEndTurn");
            
            PopUpPlayerChoice.SetActive(false);
            PopUpAutoScrap.SetActive(false);
            PopUpOr.SetActive(false);
            PopUpEndTurn.SetActive(false);

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
                camP2.SetActive(true);
                shopObject.transform.position = pos1;
                shopObject.transform.eulerAngles = new Vector3(shopObject.transform.eulerAngles.x, shopObject.transform.eulerAngles.y + 180 , shopObject.transform.eulerAngles.z);
                camP1.SetActive(false);
            }
            else
            {
                camP1.SetActive(true);
                shopObject.transform.position = pos2;
                shopObject.transform.eulerAngles = new Vector3(shopObject.transform.eulerAngles.x, shopObject.transform.eulerAngles.y + 180, shopObject.transform.eulerAngles.z);
                camP2.SetActive(false);
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