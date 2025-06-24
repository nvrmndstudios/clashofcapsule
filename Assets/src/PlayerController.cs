using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BlastEffect blastEffect))
        {
            blastEffect.Blast();
        }
    }
}
