using UnityEngine;

namespace MadSmith.Scripts.UI
{
    public class LookToCamera : MonoBehaviour
    {
        private Transform _cameraLocation;
        [SerializeField] private GameObject rotationObject;
        // Start is called before the first frame update
        void Start()
        {
            _cameraLocation = Camera.main.transform;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            rotationObject.transform.right = _cameraLocation.right;
            rotationObject.transform.up = _cameraLocation.up;
            // rotationObject.transform.LookAt(_cameraLocation);
        }
    }
}