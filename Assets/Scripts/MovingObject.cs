using System.Timers;
using Unity.MLAgents;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class MovingObject : Agent
    {
        [Header("Settings")]
        public float movementSpeed = 2f;
        public float rotationSpeed = 5f;

        protected Classroom classroom;
        protected Rigidbody rbody;


        public override void Initialize()
        {
            classroom = GetComponentInParent<Classroom>();
            rbody = GetComponent<Rigidbody>();
            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
            rbody.angularDrag = 50;
        }

        // Source: https://gamedev.stackexchange.com/questions/149283/unity-player-movement-rotation-breaks-the-movement
        protected virtual void FixedUpdate()
        {
            if (transform.position.y < 0)
            {
                EndEpisode();
            }

        }

        public override void Heuristic(float[] actionsOut)
        {
            ObjectRotation();

            actionsOut[0] = 0f;
            actionsOut[1] = 0f;
            actionsOut[2] = 0f;
            actionsOut[3] = 0f;
            actionsOut[4] = 0f;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                Debug.Log("Key UP");
                actionsOut[0] = 2f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                actionsOut[1] = 1f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                actionsOut[2] = 1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                actionsOut[3] = 1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                actionsOut[4] = 1f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                actionsOut[4] = -1f;
            }
        }

        public override void OnActionReceived(float[] vectorAction)
        {

            ObjectRotation();

            RequestDecision();
            if (vectorAction[0] > 0.5f)
            {
                Vector3 rightVelocity = new Vector3(movementSpeed * vectorAction[0], 0f, 0f);
                rbody.AddForce(rightVelocity, ForceMode.VelocityChange);
            }
            if (vectorAction[1] > 0.5f)
            {
                Vector3 leftVelocity = new Vector3(-movementSpeed * vectorAction[1], 0f, 0f);
                rbody.AddForce(leftVelocity, ForceMode.VelocityChange);
            }
            if (vectorAction[2] > 0.5f)
            {
                Vector3 leftVelocity = new Vector3(0f, 0f, movementSpeed * vectorAction[2]);
                rbody.AddForce(leftVelocity, ForceMode.VelocityChange);
            }
            if (vectorAction[3] > 0.5f)
            {
                Vector3 leftVelocity = new Vector3(0f, 0f, -movementSpeed * vectorAction[3]);
                rbody.AddForce(leftVelocity, ForceMode.VelocityChange);
            }

            if (vectorAction[4] != 0f)
            {
                transform.Rotate(0f, (vectorAction[4] * rotationSpeed) * Time.deltaTime, 0f);
            }
        }

        private void ObjectRotation()
        {
            float x = movementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            float z = movementSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.Translate(x, 0f, z, Space.World);

            float angle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;

            if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }
    }
}
