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
            actionsOut[2] = 0f;
            actionsOut[3] = 0f;
            actionsOut[4] = 0f;
            actionsOut[5] = 0f;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                actionsOut[0] = 2f;
            }
             if (Input.GetKey(KeyCode.DownArrow))
            {
                actionsOut[1] = 1f;
            }
             if (Input.GetKey(KeyCode.LeftArrow))
            {
                actionsOut[2] = 1f;
            }
             if (Input.GetKey(KeyCode.RightArrow))
            {
                actionsOut[3] = 1f;
            }
             if (Input.GetKey(KeyCode.D))
            {
                actionsOut[4] = 1f;
            }
             if (Input.GetKey(KeyCode.A))
            {
                actionsOut[5] = 1f;
            }
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            if (vectorAction[0] > 0.5f)
            {
                Vector3 rightVelocity = new Vector3(movementSpeed * vectorAction[0], 0f, 0f);
                rbody.velocity = rightVelocity;
            }
            if (vectorAction[1] > 0.5f)
            {
                Vector3 leftVelocity = new Vector3(-movementSpeed * vectorAction[1], 0f, 0f);
                rbody.velocity = leftVelocity;
            }
            if (vectorAction[2] > 0.5f)
            {
                Vector3 rightVelocity = new Vector3(0f, 0f, movementSpeed * vectorAction[2]);
                rbody.velocity = rightVelocity;
            }
            if (vectorAction[3] > 0.5f)
            {
                Vector3 leftVelocity = new Vector3(0f, 0f, -movementSpeed * vectorAction[3]);
                rbody.velocity = leftVelocity;
            }

            if (vectorAction[4] > 0.5f)
            {
                transform.Rotate(0f, (2 * rotationSpeed) * Time.deltaTime, 0f);
            }

            if (vectorAction[5] > 0.5f)
            {
                transform.Rotate(0f, (2 * rotationSpeed) * Time.deltaTime * -1, 0f);
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            Transform collObject = collision.transform;

            if (collObject.CompareTag("Wall"))
            {
                // Afstraffen om tegen de buitenste muren te botsen.
                AddReward(-0.05f);
            }
            else if (collObject.CompareTag("HideWall"))
            {
                // Kleine beloning om tegen de muur te plakken voor stealthy actions.
                // AddReward(0.001f);
            }
            else
            {
                // Ignore
            }
        }
    }
}
