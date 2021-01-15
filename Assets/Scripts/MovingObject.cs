using Unity.MLAgents;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class MovingObject : Agent
    {
        [Header("Settings")]
        public float movementSpeed = 2f;
        public float rotationSpeed = 5f;

        public Classroom Classroom { get; set; }
        protected Rigidbody rbody;
        protected GameObject jailFloor;

        public override void Initialize()
        {
            Classroom = GetComponentInParent<Classroom>();
            rbody = GetComponent<Rigidbody>();
            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
            rbody.angularDrag = 50;
        }

        protected virtual void FixedUpdate()
        {
            RequestDecision();
        }

        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = 0f;
            actionsOut[1] = 0f;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                actionsOut[0] = 1f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                actionsOut[0] = 2f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                actionsOut[1] = 1f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                actionsOut[1] = 2f;
            }
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            if (vectorAction[0] == 0 & vectorAction[1] == 0)
            {
                AddReward(-0.001f);
                return;
            }

            if (vectorAction[0] == 1f)
            {
                rbody.velocity = transform.right * movementSpeed;
            }
            if (vectorAction[0] == 2f)
            {
                rbody.velocity = -transform.right * movementSpeed;
            }
            if (vectorAction[1] == 1f)
            {
                transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * rotationSpeed, Space.World);
            }
            if (vectorAction[1] == 2f)
            {
                transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotationSpeed, Space.World);
            }
        }
    }
}
