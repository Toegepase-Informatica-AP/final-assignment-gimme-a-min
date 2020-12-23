using UnityEngine;

namespace Assets.Scripts
{
    public class Jail : MonoBehaviour
    {
        private Player player = null;
        private Seeker seeker = null;

        private void OnCollisionEnter(Collision collision)
        {
            Transform collObject = collision.transform;

            if (collObject.CompareTag("Player"))
            {
                player = collObject.gameObject.GetComponent<Player>();

                if (player.CapturedBy.capturedPlayer)
                {
                    seeker = player.CapturedBy;
                }

                if (seeker != null)
                {
                    PutPlayerInJail();
                    seeker.AddReward(1f);
                    seeker.HasPlayerGrabbed = false;
                    seeker.capturedPlayer = null;
                    seeker.EndEpisodeLogic();
                    seeker = null;
                    player = null;
                }
            }
            if (collObject.CompareTag("Seeker"))
            {
                seeker = collObject.gameObject.GetComponent<Seeker>();

                if (seeker.HasPlayerGrabbed)
                {
                    player = seeker.capturedPlayer;
                }

                if (seeker != null)
                {
                    PutPlayerInJail();
                    seeker.AddReward(1f);
                    seeker.HasPlayerGrabbed = false;
                    seeker.capturedPlayer = null;
                    seeker.EndEpisodeLogic();
                    seeker = null;
                    player = null;
                }
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            Transform collObject = collision.transform;

            if (collObject.CompareTag("Player"))
            {
                player = collObject.gameObject.GetComponent<Player>();

                if (player.CapturedBy.capturedPlayer)
                {
                    seeker = player.CapturedBy;
                }

                if (seeker != null)
                {
                    PutPlayerInJail();
                    seeker.AddReward(1f);
                    seeker.HasPlayerGrabbed = false;
                    seeker.capturedPlayer = null;
                    seeker.EndEpisodeLogic();
                    seeker = null;
                    player = null;
                }
            }
            if (collObject.CompareTag("Seeker"))
            {
                seeker = collObject.gameObject.GetComponent<Seeker>();

                if (seeker.HasPlayerGrabbed)
                {
                    player = seeker.capturedPlayer;
                }

                if (seeker != null)
                {
                    PutPlayerInJail();
                    seeker.AddReward(1f);
                    seeker.HasPlayerGrabbed = false;
                    seeker.capturedPlayer = null;
                    seeker.EndEpisodeLogic();
                    seeker = null;
                    player = null;
                }
            }
        }
        
        private void PutPlayerInJail()
        {
            if (player != null && !player.IsJailed)
            {
                player.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
                player.IsJailed = true;
                player.IsGrabbed = false;
                seeker.playersCaptured++;
            }
        }
    }
}
