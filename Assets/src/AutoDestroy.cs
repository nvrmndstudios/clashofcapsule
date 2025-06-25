using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyAfterSeconds = 3f;

    private void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }
}