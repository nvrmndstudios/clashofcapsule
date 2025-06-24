using System.Collections;
using UnityEngine;

public class SinkingPart : MonoBehaviour
{
    private float sinkDelay = 2f;
    private float destroyDelay = 1f;

    public void StartSinkProcess()
    {
        StartCoroutine(StartSink());
    }

    private IEnumerator StartSink()
    {
        yield return new WaitForSeconds(sinkDelay);
        if (gameObject.TryGetComponent(out Collider collider))
        {
            collider.enabled = false;
        }
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

}