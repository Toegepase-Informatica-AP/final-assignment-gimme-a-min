﻿using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Assets.Scripts
{
    public class Seeker : MovingObject
    {
        public Player CapturedPlayer { get; set; }
        public bool HasPlayerGrabbed { get; set; }
        public int PlayerCount { get; set; }
        public int PlayersCaptured { get; set; }

        public override void CollectObservations(VectorSensor sensor)
        {
            base.CollectObservations(sensor);

            sensor.AddObservation(HasPlayerGrabbed);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (CapturedPlayer != null && CapturedPlayer.IsGrabbed && !CapturedPlayer.IsJailed)
            {
                TransportPlayer();
            }

        }

        private void TransportPlayer()
        {
            if (CapturedPlayer != null)
            {
                CapturedPlayer.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            }
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            base.OnActionReceived(vectorAction);

            if (vectorAction[0] == 0f && vectorAction[1] == 0f && vectorAction[2] == 0f && vectorAction[3] == 0f && vectorAction[4] == 0f && vectorAction[5] == 0f)
            {
                // Stilstaan & niet rondkijken samen zorgt voor afstraffing.
                AddReward(-0.001f);
            }
        }

        public override void OnEpisodeBegin()
        {
            Classroom = GetComponentInParent<Classroom>();

            if (Classroom != null)
            {
                Classroom.ClearEnvironment();
                Classroom.ResetSpawnSettings();
                Classroom.SpawnPlayers();
                Classroom.SpawnSeekers();
                PlayerCount = Classroom.playerCount;
            }

            PlayersCaptured = 0;
            HasPlayerGrabbed = false;
            CapturedPlayer = null;
        }

        protected void OnCollisionEnter(Collision collision)
        {
            Transform collObject = collision.transform;

            if (collObject.CompareTag("Player"))
            {
                if (!HasPlayerGrabbed)
                {
                    HasPlayerGrabbed = true;

                    CapturedPlayer = collObject.gameObject.GetComponent<Player>();
                    if (CapturedPlayer != null && !CapturedPlayer.IsJailed)
                    {
                        CapturedPlayer.IsGrabbed = true;
                        CapturedPlayer.CapturedBy = this;
                        CapturedPlayer.AddReward(-1f);
                        AddReward(0.5f);
                    }
                }
                else
                {
                    // Afstraffen voor tegen een Player te botsen als die er al eentje vastheeft.
                    AddReward(-0.1f);
                }
            }
            else if (collObject.CompareTag("JailFloor"))
            {
                EndEpisode();
            }
        }

        public void EndEpisodeLogic()
        {
            if (PlayersCaptured == PlayerCount)
            {
                // Eindig episode als alle players worden gevangen.
                EndEpisode();
            }
        }

        public void ClearCapturedPlayer()
        {
            PlayersCaptured++;
            AddReward(1f);
            HasPlayerGrabbed = false;
            CapturedPlayer = null;
            EndEpisodeLogic();
        }
    }
}
