using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsAlive
    {
        get;
        private set;
    }

    [SerializeField] private GameObject blastParticle;

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
        }
    }

    public void Die()
    {
        IsAlive = false;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
