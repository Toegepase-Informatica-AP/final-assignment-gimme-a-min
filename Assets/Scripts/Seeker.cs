using Unity.MLAgents.Sensors;
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

            if (HasPlayerGrabbed)
            {
                RequestDecision();
            }

            if (CapturedPlayer != null && CapturedPlayer.IsGrabbed && !CapturedPlayer.IsJailed)
            {
                TransportPlayer();
            }

            if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Player") && !HasPlayerGrabbed)
                {
                    float reward = 0.001f;

                    // Blijf punten toevoegen zolang een speler in zijn zicht is.
                    AddReward(reward);

                    // TODO: eens nakijken of we dit wel willen.
                    //Player player = hit.transform.gameObject.GetComponent<Player>();

                    //if (player != null)
                    //{
                    //    // Blijf speler afstraffen zolang hij in het zicht van een seeker is.
                    //    player.AddReward(-reward);
                    //}
                }
            }
        }

        private void TransportPlayer()
        {
            if (CapturedPlayer != null)
            {
                CapturedPlayer.transform.position = new Vector3(transform.position.x -1 , transform.position.y, transform.position.z);
            }
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

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            Transform collObject = collision.transform;

            if (collObject.CompareTag("Player"))
            {
                if (!HasPlayerGrabbed)
                {
                    HasPlayerGrabbed = true;

                    CapturedPlayer = collObject.gameObject.GetComponent<Player>();
                    if (CapturedPlayer != null && !CapturedPlayer.IsJailed)
                    {
                        var reward = 0.5f;
                        CapturedPlayer.IsGrabbed = true;
                        CapturedPlayer.CapturedBy = this;
                        CapturedPlayer.AddReward(-reward);
                        AddReward(reward);
                    }
                }
                else
                {
                    // Afstraffen voor tegen een Player te botsen als die er al eentje vast heeft?
                    AddReward(-0.05f);
                }
            }
            else if (collObject.CompareTag("Grabbable"))
            {
                // Afstraffen als die zich laat vertragen door een grabbable?
                AddReward(-0.1f);
            }
            else if (collObject.CompareTag("JailFloor"))
            {
                EndEpisode();
            }
            else
            {
                // Ignore
                return;
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
