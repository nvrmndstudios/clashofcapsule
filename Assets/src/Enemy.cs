using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public PlayerController player;             // Reference to the player
    public float moveSpeed = 3f;         // Enemy movement speed
    public float maxLife = 100f;         // Max life value
    public float lifeDrainRate = 5f;     // Life lost per second

    private float currentLife;
    
    public event Action<GameObject, bool> OnEnemyDied;

    [SerializeField] private Image lifeBarFill;  // UI Image (fill) reference

    private void Start()
    {
        currentLife = maxLife;
    }

    private void Update()
    {
        if (player == null || !player.IsAlive) return;

        // Follow player
        Vector3 targetPos = new Vector3(player.GetPosition().x, transform.position.y, player.GetPosition().z);
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * (moveSpeed * Time.deltaTime);

        // Face player
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);
        }

        // Drain life
        currentLife -= lifeDrainRate * Time.deltaTime;
        if (currentLife <= 0f)
        {
            if (OnEnemyDied != null)
            {
                OnEnemyDied(gameObject, false);
            }
            Destroy(gameObject);
            return;
        }

        // Update life bar
        if (lifeBarFill != null)
        {
            lifeBarFill.fillAmount = currentLife / maxLife;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BlastEffect blastEffect))
        {
            blastEffect.Blast();
            if (OnEnemyDied != null)
            {
                OnEnemyDied(gameObject, true);
            }
        }
    }
}
