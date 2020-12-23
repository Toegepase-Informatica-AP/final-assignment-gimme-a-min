using System.Timers;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Assets.Scripts
{
    public class Seeker : MovingObject
    {
        public int playersCaptured;
        public int playerCount;
        public Player capturedPlayer = null;

        public bool HasPlayerGrabbed { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            playerCount = 1;
        }

        protected override void FixedUpdate()
        {

            base.FixedUpdate();

            if (capturedPlayer != null && capturedPlayer.IsGrabbed && !capturedPlayer.IsJailed)
            {
                TransportPlayer();
            }

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    // Blijf punten toevoegen zolang een speler in zijn zicht is.
                    AddReward(0.001f);

                    Player player = hit.transform.gameObject.GetComponent<Player>();

                    if (player != null)
                    {
                        // Blijf speler afstraffen zolang hij in het zicht van een seeker is.
                        player.AddReward(-0.001f);
                    }
                }
            }
        }

        private void TransportPlayer()
        {
            if (capturedPlayer != null)
            {
                capturedPlayer.transform.localPosition = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            base.CollectObservations(sensor);

            sensor.AddObservation(HasPlayerGrabbed);
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
            classroom = GetComponentInParent<Classroom>();

            if (classroom != null)
            {
                classroom.ClearEnvironment();
                classroom.ResetSpawnSettings();
                classroom.SpawnPlayers();
                classroom.SpawnSeekers();
                playerCount = classroom.playerCount;
            }

            playersCaptured = 0;
            HasPlayerGrabbed = false;
            capturedPlayer = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Transform collObject = collision.transform;

            if (collObject.CompareTag("Player"))
            {
                if (!HasPlayerGrabbed)
                {
                    HasPlayerGrabbed = true;

                    capturedPlayer = collObject.gameObject.GetComponent<Player>();
                    if (capturedPlayer != null)
                    {
                        capturedPlayer.IsGrabbed = true;
                        capturedPlayer.CapturedBy = this;
                    }

                    AddReward(0.1f);

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
                // AddReward(0.001f);
            }
            else if (collObject.CompareTag("Grabbable"))
            {
                // Afstraffen als die zich laat vertragen door een grabbable?
                AddReward(-0.1f);
            }
            else
            {
                // Ignore
                return;
            }
        }

        public void EndEpisodeLogic()
        {
            if (playersCaptured >= playerCount)
            {
                // Eindig episode als alle players worden gevangen.
                EndEpisode();
            }
        }
    }
}
