﻿using System.Collections;
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
                actionsOut[3] = 2f;
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
                rb.AddForce(rightVelocity, ForceMode.VelocityChange);
            }
            if (vectorAction[1] > 0.5f)
            {
                Vector3 leftVelocity = new Vector3(-movementSpeed * vectorAction[1], 0f, 0f);
                rb.AddForce(leftVelocity, ForceMode.VelocityChange);
            }
            if (vectorAction[2] > 0.5f)
            {
                Vector3 leftVelocity = new Vector3(0f, 0f, movementSpeed * vectorAction[2]);
                rb.AddForce(leftVelocity, ForceMode.VelocityChange);
            }
            if (vectorAction[3] > 0.5f)
            {
                Vector3 leftVelocity = new Vector3(0f, 0f, -movementSpeed * vectorAction[3]);
                rb.AddForce(leftVelocity, ForceMode.VelocityChange);
            }

            if (vectorAction[4] != 0f)
            {
                //float rotation = rotationSpeed * (vectorAction[4] * 2 - 3) * Time.deltaTime;
                transform.Rotate(0f, (vectorAction[4] * rotationSpeed) * Time.deltaTime, 0f);
            }

        }



    }
}
