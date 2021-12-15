using UnityEngine;

namespace Resources.Script
{
    public class GameManager : MonoBehaviour
    {
        public static GameObject popUpPlayerChoice;
        public static GameObject popUpOr;
        public static GameObject popUpAutoScrap;
        public static GameObject popUpEndTurn;
        
        public static Player player1 ;
        public static Player player2;
        public static Player currentPlayer;

        [SerializeField]
        private GameObject panelWin;
        private static bool _firstRound = true;

        public static GameObject camP1;
        public static GameObject camP2;
        public static Vector3 pos1 = new Vector3(-1.18f, 0.82f, -9.17f);
        public static Vector3 pos2 = new Vector3(1.14f, 0.82f, 9.16f);
        public static GameObject shopObject;

        private void Awake()
        {
            shopObject = GameObject.Find("Shop");
            camP1 = GameObject.Find("CM vcam1");
            camP2 = GameObject.Find("CM vcam2");
            camP2.SetActive(false);
            popUpPlayerChoice = GameObject.Find("PopUpPlayerChoice");
            popUpOr = GameObject.Find("PopUpOr");
            popUpAutoScrap = GameObject.Find("PopUpAutoScrap");
            popUpEndTurn = GameObject.Find("PopUpEndTurn");

            popUpPlayerChoice.SetActive(false);
            popUpAutoScrap.SetActive(false);
            popUpOr.SetActive(false);
            popUpEndTurn.SetActive(false);

            player1 = GameObject.Find("Player1").GetComponent<Player>();
            player2 = GameObject.Find("Player2").GetComponent<Player>();
            currentPlayer = player1;
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
                currentPlayer.Draw(3);
                _firstRound = false;
            }
            else
            {
                currentPlayer.Draw(5);
            }
        }

        public static void EndTurn()
        {
            //supression de la money et du power restan 
            currentPlayer.money = 0;
            currentPlayer.totalPower = 0;

           //echange de borad (visuelle)
            if(camP2.activeSelf == false)
            {
                camP2.SetActive(true);
                shopObject.transform.position = pos2;
                shopObject.transform.eulerAngles = new Vector3(shopObject.transform.eulerAngles.x, shopObject.transform.eulerAngles.y + 180 , shopObject.transform.eulerAngles.z);
                camP1.SetActive(false);
            }
            else
            {
                camP1.SetActive(true);
                shopObject.transform.position = pos1;
                shopObject.transform.eulerAngles = new Vector3(shopObject.transform.eulerAngles.x, shopObject.transform.eulerAngles.y + 180, shopObject.transform.eulerAngles.z);
                camP2.SetActive(false);
            }

            //switche du current player et des hp de ce dernier
                if (currentPlayer == player1)
            {
             
               currentPlayer = player2;
            }
            else
            {
                currentPlayer = player1;
            }
            BeginTurn();
        }

        public static bool IsPopUpActivated()
        {
            return popUpPlayerChoice.activeSelf ||
                   popUpAutoScrap.activeSelf ||
                   popUpOr.activeSelf ||
                   popUpEndTurn.activeSelf;
        }
    }
}