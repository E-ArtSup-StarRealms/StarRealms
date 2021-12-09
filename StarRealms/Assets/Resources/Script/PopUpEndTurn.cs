using UnityEngine;

namespace Resources.Script
{
    public class PopUpEndTurn : MonoBehaviour
    {
        
        public void Activate()
        {
            gameObject.SetActive(true);
        }
        public void Validate()
        {
            GameManager.CurrentPlayer.EndTurn(true);
            gameObject.SetActive(false);
        }
        
        public void Cancel()
        {
            gameObject.SetActive(false);
        }
    }
}
