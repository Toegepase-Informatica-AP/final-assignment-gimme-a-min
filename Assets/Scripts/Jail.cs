using UnityEngine;

namespace Assets.Scripts
{
    public class Jail : MonoBehaviour
    {
        private Player player = null;
        private Seeker seeker = null;

        public void Initialize()
        {
            seeker = transform.GetComponentInParent<Seeker>();
        }

        protected void FixedUpdate()
        {
            PutPlayerInJail();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Transform collObject = collision.transform;

            if (collObject.CompareTag("Player"))
            {
                player = collObject.gameObject.GetComponent<Player>();
            }
        }

        private void PutPlayerInJail()
        {
            if (player != null && !player.IsJailed)
            {
                player.IsJailed = true;
                player.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
                seeker.playersCaptured++;
            }
        }
    }
}
