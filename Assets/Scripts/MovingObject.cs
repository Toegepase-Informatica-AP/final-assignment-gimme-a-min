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
        protected GameObject jailFloor;

        public override void Initialize()
        {
            classroom = GetComponentInParent<Classroom>();
            rbody = GetComponent<Rigidbody>();
            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
            rbody.angularDrag = 50;
        }

        protected virtual void FixedUpdate()
        {
            if (transform.position.y < 0)
            {
                EndEpisode();
            }

            ObjectRotation();
            RequestDecision();
        }

        public override void Heuristic(float[] actionsOut)
        {
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
                //actionsOut[2] = 1f;
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                //actionsOut[3] = 1f;
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //actionsOut[4] = 1f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                //actionsOut[4] = -1f;
            }
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            if (vectorAction[0] > 0.5f)
            {
                //Vector3 rightVelocity = new Vector3(movementSpeed * vectorAction[0], 0f, 0f);
                Vector3 rightVelocity = movementSpeed * transform.right;
                rbody.AddForce(rightVelocity, ForceMode.VelocityChange);
            }
            if (vectorAction[1] > 0.5f)
            {
                //Vector3 leftVelocity = new Vector3(-movementSpeed * vectorAction[1], 0f, 0f);
                Vector3 leftVelocity = -movementSpeed * transform.right;
                rbody.AddForce(leftVelocity, ForceMode.VelocityChange);
            }
            /*if (vectorAction[2] > 0.5f)
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
            }*/
        }

        private Quaternion newTargetDirection = Quaternion.identity;

        public void SetBlendedEulerAngles(Vector3 angles)
        {
            newTargetDirection = Quaternion.Euler(angles);
        }
        // Source: https://gamedev.stackexchange.com/questions/149283/unity-player-movement-rotation-breaks-the-movement
        private void ObjectRotation()
        {
            /*float x = movementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            float z = movementSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.Translate(x, 0f, z, Space.World);

            float angle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;

            if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            //transform.RotateAround(gameObject.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            */
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
