using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.CameraComponents
{
    public class CameraControls : MonoBehaviour
    {
        [SerializeField] private float zoomSpeed = 100.0f;
        [SerializeField] private float rotationSpeed = 10.0f;

        private CinemachineBrain brain;

        // Start is called before the first frame update
        void Start()
        {
            this.brain = GetComponent<CinemachineBrain>();
        }

        // Update is called once per frame
        void Update()
        {
            
            CinemachineVirtualCamera vCamera = this.brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
            CinemachineFramingTransposer vTransposer = vCamera.GetCinemachineComponent<CinemachineFramingTransposer>();


            float vAxis = Input.GetAxis("Vertical") * -1.0f;
            if (vAxis != 0.0f)
            {
                vTransposer.m_CameraDistance = vTransposer.m_CameraDistance + (zoomSpeed * Time.deltaTime * vAxis);
            }

            float hAxis = Input.GetAxis("Horizontal");
            if (hAxis != 0.0f)
            {
                Vector3 euler = vCamera.transform.eulerAngles;
                vCamera.transform.eulerAngles = new Vector3(euler.x, euler.y + (rotationSpeed * Time.deltaTime * hAxis), euler.z);
                Debug.Log(new Vector3(euler.x, euler.y + (rotationSpeed * Time.deltaTime * hAxis), euler.z));
            }
        }
    }
}
