using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMoveDirect : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject clickParticlePrefab;
    [SerializeField] private Vector3 particleRotationEuler = Vector3.zero;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                // Instantiate particle at clicked position
                if (clickParticlePrefab != null)
                {
                    Quaternion rotation = Quaternion.Euler(particleRotationEuler);
                    Instantiate(clickParticlePrefab, hit.point, rotation);
                }

                // Lock movement to XZ plane
                targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                isMoving = true;

                // Rotate toward the target (Y axis only)
                Vector3 direction = (targetPosition - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
                }
            }
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isMoving = false;
            }
        }
    }
}