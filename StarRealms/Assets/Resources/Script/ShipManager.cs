using UnityEngine;

namespace Resources.Script
{
    public class ShipManager : MonoBehaviour
    {
        public GameObject objectToMove;
        private float _currentTime;
        public float timer;
        public Card hisCard;
        
        void Update()
        {
            if (objectToMove.transform.position != transform.position || objectToMove.transform.rotation != transform.rotation)
            {
                _currentTime += Time.deltaTime;
                float percent = _currentTime / timer;
                transform.position = Vector3.Lerp(transform.position, objectToMove.transform.position, percent);
                transform.rotation = Quaternion.Lerp(transform.rotation, objectToMove.transform.rotation, percent);
            }
            else
            {
                _currentTime = 0;
            }
        }
    }
}
