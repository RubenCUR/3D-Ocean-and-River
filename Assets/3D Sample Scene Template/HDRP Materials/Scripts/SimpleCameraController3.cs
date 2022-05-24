using UnityEngine;

namespace UnityTemplateProjects
{
    public class SimpleCameraController3 : MonoBehaviour
    {
        public bool UsePitchYaw = true;
        public bool UseMinMax = false;

        public float minAnglePitch = -12.0f;
        public float maxAnglePitch = -14.0f;

        public float minAngleYaw = 60.0f;
        public float maxAngleYaw = 120.0f;

        public float rotationY = 0;

        public float rotationX = 0;

        public Spawner spwn;

        public GameObject goMaterials;

        class CameraState
        {
            public float yaw;
            public float pitch;
            public float roll;
            public float x;
            public float y;
            public float z;

            public void SetFromTransform(Transform t)
            {
                pitch = t.eulerAngles.x;
                yaw = t.eulerAngles.y;
                roll = t.eulerAngles.z;
                x = t.position.x;
                y = t.position.y;
                z = t.position.z;
            }

            public void Translate(Vector3 translation)
            {
                Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

                x += rotatedTranslation.x;
                y += rotatedTranslation.y;
                z += rotatedTranslation.z;
            }

            public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            {
                yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
                pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
                roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);
                
                x = Mathf.Lerp(x, target.x, positionLerpPct);
                y = Mathf.Lerp(y, target.y, positionLerpPct);
                z = Mathf.Lerp(z, target.z, positionLerpPct);
            }

            public void UpdateTransform(Transform t)
            {
                t.eulerAngles = new Vector3(pitch, yaw, roll);
                t.position = new Vector3(x, y, z);
            }
        }
        
        CameraState m_TargetCameraState = new CameraState();
        CameraState m_InterpolatingCameraState = new CameraState();

        [Header("Movement Settings")]
        [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
        public float boost = 3.5f;

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
        public float positionLerpTime = 0.2f;

        [Header("Rotation Settings")]
        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        public bool invertY = false;

        void OnEnable()
        {
            m_TargetCameraState.SetFromTransform(transform);
            m_InterpolatingCameraState.SetFromTransform(transform);
        }

        bool IsMiddleMouseButtonPressed = false;

        bool IsMiddleMouseButtonToggled = false;

        Vector3 GetInputTranslationDirection()
        {
            Vector3 direction = new Vector3();

            if (Input.GetMouseButtonDown(2))
            {
                if(IsMiddleMouseButtonToggled)
                    IsMiddleMouseButtonToggled = false;
                else
                    IsMiddleMouseButtonToggled = true;
            }

            if (Input.GetMouseButtonUp(2))
            {
                IsMiddleMouseButtonPressed = false;

                if(spwn != null)
                    if (!spwn.World_ID.Equals(2))
                    {
                        //Texture Picker
                        //if (Cursor.lockState == CursorLockMode.None)
                        //    Cursor.lockState = CursorLockMode.Confined;
                        if (Cursor.lockState == CursorLockMode.Confined || Cursor.lockState == CursorLockMode.None)
                            Cursor.lockState = CursorLockMode.Locked;
                        else if (Cursor.lockState == CursorLockMode.Locked)
                            Cursor.lockState = CursorLockMode.Confined;
                    }
                    else if (spwn.World_ID.Equals(2))
                    {
                        if (Cursor.lockState != CursorLockMode.None)
                            Cursor.lockState = CursorLockMode.None;
                    }
            }
            
            if (Input.GetKey(KeyCode.W)) // && IsMiddleMouseButtonToggled == true
            {
                direction += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S)) // && IsMiddleMouseButtonToggled == true
            {
                direction += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A)) // && IsMiddleMouseButtonToggled == true
            {
                direction += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D)) // && IsMiddleMouseButtonToggled == true
            {
                direction += Vector3.right;
            }
            if (Input.GetKey(KeyCode.Q)) // && IsMiddleMouseButtonToggled == true
            {
                direction += Vector3.down;
            }
            if (Input.GetKey(KeyCode.E)) // && IsMiddleMouseButtonToggled == true
            {
                direction += Vector3.up;
            }
            
            return direction;
        }

        private void Start()
        {
            /////
            Cursor.visible = true;

            //Cursor.lockState = CursorLockMode.Locked;

            if (goMaterials != null)
                goMaterials.SetActive(false);

            if (GameObject.Find("OutlinedPlaceholders") != null)
                GameObject.Find("OutlinedPlaceholders").SetActive(true);

            //if (spwn != null)
            //    if (spwn.World_ID.Equals(2))
            //        Cursor.lockState = CursorLockMode.None;
            //    else
            //        Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            // Exit Sample  
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
				#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false; 
				#endif
            }

            // Confine, Hide and lock cursor
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                /////
                Cursor.visible = true;

                Cursor.lockState = CursorLockMode.Confined;

                if (goMaterials != null)
                    goMaterials.SetActive(true);

                if(spwn != null)
                    foreach (GameObject item in spwn.outlinedObjects)
                    {
                        if(item != null)
                            item.SetActive(false);
                    }
            }
            else if (Input.GetKeyDown(KeyCode.Period))
            {
                /////
                Cursor.visible = true;

                Cursor.lockState = CursorLockMode.None;

                if (goMaterials != null)
                    goMaterials.SetActive(true);

                if (GameObject.Find("OutlinedPlaceholders") != null)
                    GameObject.Find("OutlinedPlaceholders").SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Slash))
            {
                /////
                Cursor.visible = true;

                Cursor.lockState = CursorLockMode.Locked;
                
                if(goMaterials != null)
                    goMaterials.SetActive(false);

                if (GameObject.Find("OutlinedPlaceholders") != null)
                    GameObject.Find("OutlinedPlaceholders").SetActive(true);
            }

            // Rotation
            //if (Input.GetMouseButton(1))
            {
                var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));
                
                var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                if(UsePitchYaw)
                { 
                    m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
                    m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;
                }
            }
            
            // Translation
            var translation = GetInputTranslationDirection() * Time.deltaTime;

            // Speed up movement when shift key held
            if (Input.GetKey(KeyCode.LeftShift))
            {
                translation *= 10.0f;
            }
            
            // Modify movement by a boost factor (defined in Inspector and modified in play mode through the mouse scroll wheel)
            //boost += Input.mouseScrollDelta.y * 0.2f; //+=
            translation *= Mathf.Pow(2.0f, boost);

            m_TargetCameraState.Translate(translation);

            // Framerate-independent interpolation
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);

            //cameraV = transform.rotation.eulerAngles;

            if(UseMinMax)
            { 
                rotationY = m_TargetCameraState.pitch;

                rotationX =  m_TargetCameraState.yaw;

                if (rotationX <= minAngleYaw)
                    rotationX = minAngleYaw;

                if (rotationX >= maxAngleYaw)
                    rotationX = maxAngleYaw;

                m_TargetCameraState.yaw = rotationX;

                if (rotationY <= minAnglePitch)
                    rotationY = minAnglePitch;

                if (rotationY >= maxAnglePitch)
                    rotationY = maxAnglePitch;

                m_TargetCameraState.pitch = rotationY;
            }

            m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

            m_InterpolatingCameraState.UpdateTransform(transform);
            
        }
    }

}