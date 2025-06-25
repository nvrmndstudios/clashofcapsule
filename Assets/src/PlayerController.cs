using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject blastParticle;
    public bool IsAlive { get; private set; }

    private void Awake()
    {
        IsAlive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BlastEffect blastEffect))
        {
            blastEffect.Blast();
            Die();

            var blast = gameObject.AddComponent<BlastEffect>();
            blast.SetValuesManually(blastParticle);
            blast.Blast();
        }
    }

    public void Die()
    {
        IsAlive = false;
        GameManager.Instance.OnPlayerDie();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
