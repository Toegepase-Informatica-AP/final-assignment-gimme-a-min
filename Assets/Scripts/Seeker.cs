using Unity.MLAgents;
using UnityEngine;

namespace Assets.Scripts
{
    public class Seeker : Agent
    {
        [Header("Settings")]
        public float movementSpeed = 2f;
        public float rotationSpeed = 5f;

        private Classroom classroom;
        private Rigidbody rbody;
        private Player player;

        public override void Initialize()
        {
            base.Initialize();
            classroom = GetComponentInParent<Classroom>();
            player = GetComponentInParent<Player>();
            rbody = GetComponent<Rigidbody>();
            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
            rbody.angularDrag = 50;
        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            {
                Debug.Log(hit.transform.tag);
                Debug.Log(hit.transform.CompareTag("Wall"));
            }
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
                actionsOut[2] = 1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                actionsOut[3] = 1f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                actionsOut[4] = 1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                actionsOut[4] = -1f;
            }
        }

        public override void OnActionReceived(float[] vectorAction)
        {
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
                //float rotation = rotationSpeed * (vectorAction[4] * 2 - 3) * Time.deltaTime;
                transform.Rotate(0f, (vectorAction[4] * rotationSpeed) * Time.deltaTime, 0f);
            }
        }

        // D'Haese code
        // URL: https://ddhaese.github.io/ML-Agents/gedragingen-van-de-agent-en-de-andere-spelobjecten.html#obelix.cs
        public override void OnEpisodeBegin()
        {
            transform.localPosition = new Vector3(-22f, 1.5f, 22f);
            transform.localRotation = Quaternion.Euler(0f, 225f, 0f);

            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
        }
    }
}
