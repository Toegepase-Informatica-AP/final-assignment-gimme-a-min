using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts
{
    public class MovementProvider : LocomotionProvider
    {
        public float movementSpeed = 1.0f;
        public float gravityMultiplier = 1.0f;

        public List<XRController> characterControllers = null;
        private CharacterController characterController = null;
        private GameObject headObject = null;

        protected override void Awake()
        {
            characterController = GetComponent<CharacterController>();
            headObject = GetComponent<XRRig>().cameraGameObject;
        }

        void Start()
        {
            PositionController();
        }

        void Update()
        {
            PositionController();
            CheckForControllerInput();
            ApplyGravity();
        }

        private void CheckForControllerInput()
        {
            foreach (XRController controller in characterControllers)
            {
                if (controller.enableInputActions)
                {
                    CheckForMovement(controller.inputDevice);
                }
            }
        }

        private void CheckForMovement(InputDevice device)
        {
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
            {
                StartMove(position);
            }
        }

        private void StartMove(Vector2 position)
        {
            Vector3 direction = new Vector3(position.x, 0, position.y);
            Vector3 headRotation = new Vector3(0, headObject.transform.eulerAngles.y, 0);

            direction = Quaternion.Euler(headRotation) * direction;

            Vector3 movement = direction * movementSpeed;
            characterController.Move(movement * Time.deltaTime);
        }

        private void PositionController()
        {
            float headHeight = Mathf.Clamp(headObject.transform.localPosition.y, 2, 3);
            characterController.height = headHeight;

            Vector3 newCenter = Vector3.zero;
            newCenter.y = characterController.height / 4;
            newCenter.y += characterController.skinWidth;

            newCenter.x = headObject.transform.localPosition.x;
            newCenter.z = headObject.transform.localPosition.z;

            characterController.center = newCenter;
        }

        private void ApplyGravity()
        {
            Vector3 gravity = new Vector3(0f, Physics.gravity.y * gravityMultiplier, 0f);
            gravity.y = gravity.y * Time.deltaTime;

            characterController.Move(gravity * Time.deltaTime);
        }
    }
}
