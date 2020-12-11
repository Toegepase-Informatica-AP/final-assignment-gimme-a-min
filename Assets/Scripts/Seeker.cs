using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Assets.Scripts
{
    public class Seeker : MovingObject
    {
        private bool hasPlayerGrabbed;
        private int playersCaptured;
        private int playerCount;

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            base.CollectObservations(sensor);

            sensor.AddObservation(hasPlayerGrabbed);
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            base.OnActionReceived(vectorAction);

            if (vectorAction[0] == 0f && vectorAction[1] == 0f && vectorAction[2] == 0f && vectorAction[3] == 0f && vectorAction[4] == 0f)
            {
                // Stilstaan & niet rondkijken samen zorgt voor afstraffing.
                AddReward(-0.001f);
            }
        }

        public override void OnEpisodeBegin()
        {
            base.OnEpisodeBegin();

            classroom.ClearEnvironment();
            classroom.ResetSpawnSettings();
            classroom.SpawnPlayers();
            classroom.SpawnSeekers();

            playersCaptured = 0;
            hasPlayerGrabbed = false;
            playerCount = classroom.playerCount;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var collObject = collision.transform;

            if (collObject.CompareTag("Jail") && hasPlayerGrabbed)
            {
                hasPlayerGrabbed = false;
                AddReward(1f);
                playersCaptured++;
                // TODO: logica van student in gevangenis te zetten.
                if (playersCaptured == playerCount)
                {
                    // Eindig episode als alle players worden gevangen.
                    EndEpisode();
                }
            }
            else if (collObject.CompareTag("Player"))
            {
                if (!hasPlayerGrabbed)
                {
                    hasPlayerGrabbed = true;
                    AddReward(0.1f);
                    // TODO: logica van student vast te nemen.
                }
                else
                {
                    // Afstraffen voor tegen een Player te botsen als die er al eentje vast heeft?
                    AddReward(-0.05f);
                }
            }
            else if (collObject.CompareTag("Wall"))
            {
                // Afstraffen om tegen de buitenste muren te botsen.
                AddReward(-0.05f);
            }
            else if (collObject.CompareTag("HideWall"))
            {
                // Kleine beloning om tegen de muur te plakken voor stealthy actions.
                AddReward(0.001f);
            }
            else if (collObject.CompareTag("Grabbable"))
            {
                // Afstraffen als die zich laat vertragen door een grabbable?
                AddReward(-0.1f);
            }
            else
            {
                // Ignore
            }
        }
    }
}
