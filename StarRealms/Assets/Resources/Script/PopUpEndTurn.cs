using UnityEngine;
using UnityEngine.UI;

namespace Resources.Script
{
    public class PopUpEndTurn : MonoBehaviour
    {
        
        public void Activate(string s)
        {
            transform.GetChild(2).gameObject.GetComponent<Text>().text = s;
            gameObject.SetActive(true);
        }
        public void Validate()
        {
            GameManager.currentPlayer.EndTurn(true);
            gameObject.SetActive(false);
        }
        
        public void Cancel()
        {
            gameObject.SetActive(false);
        }
    }
}
