using System.Collections;
using UnityEngine;

public class BlastEffect : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionForce = 700f;
    public float explosionRadius = 5f;
    public float upwardsModifier = 0.4f;

    [Header("Cleanup Settings")]
    public float sleepCheckDelay = 2f;          // How long to wait before checking for sleep
    public float sinkSpeed = 1f;                // Speed at which the part sinks
    public float sinkDestroyDelay = 3f;         // How long after sinking starts to destroy it


    [SerializeField] private GameObject blastParticle;

    [Header("Other Settings")]
    public bool addColliderIfMissing = true;

    private bool hasBlasted = false;

    public void Blast()
    {
        if (hasBlasted) return;
        hasBlasted = true;

        Vector3 blastPosition = transform.position;

        var blastObject = Instantiate(blastParticle, blastPosition, Quaternion.identity);

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in meshRenderers)
        {
            GameObject part = renderer.gameObject;

            // Add Rigidbody if missing
            Rigidbody rb = part.GetComponent<Rigidbody>();
            if (rb == null)
                rb = part.AddComponent<Rigidbody>();

            // Add MeshCollider if missing
            if (addColliderIfMissing && part.GetComponent<Collider>() == null)
            {
                MeshCollider collider = part.AddComponent<MeshCollider>();
                collider.convex = true;
            }

            // Detach from parent
            part.transform.SetParent(null);

            // Apply explosion
            rb.AddExplosionForce(explosionForce, blastPosition, explosionRadius, upwardsModifier, ForceMode.Impulse);
            
            rb.gameObject.AddComponent<SinkingPart>().StartSinkProcess();
        }

        // Optionally destroy original parent shell
        Destroy(gameObject);
    }
}