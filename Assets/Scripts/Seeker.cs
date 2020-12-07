using System.Collections;
using System.Collections.Generic;
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
        private Rigidbody rb;
        private Player player;

        public override void Initialize()
        {
            base.Initialize();
            classroom = GetComponentInParent<Classroom>();
            player = GetComponentInParent<Player>();
            rb = GetComponent<Rigidbody>();
        }


        private void FixedUpdate()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Debug.Log(hit.transform.tag);
                Debug.Log(hit.transform.CompareTag("Wall"));
            }
        }



        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = 0f;
            actionsOut[1] = 0f;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                Debug.Log("Key UP");
                actionsOut[0] = 2f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                actionsOut[0] = 1f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                actionsOut[1] = 1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
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

            if (vectorAction[0] != 0)
            {
                Vector3 translation = transform.forward * movementSpeed * (vectorAction[0] * 2 - 3) * Time.deltaTime;
                transform.Translate(translation, Space.World);
            }

            if (vectorAction[1] != 0)
            {
                float rotation = rotationSpeed * (vectorAction[1] * 2 - 3) * Time.deltaTime;
                transform.Rotate(0, rotation, 0);
            }
        }



    }
}
