using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Range(1f, 10f)] public float detectionRadius = 5f;
    [Range(1f, 180f)] public float viewAngle = 90f;

    public Transform player;

    void Update()
    {
        if (CanSeePlayer())
        {
            Debug.Log("Player detected!");           
        }
        else{
            Debug.Log("Player outside of enemy's detection radius");
        }
    }

    public bool CanSeePlayer()
    {
        if (player == null) return false;

        // Step 1: Distance check
        Vector3 dirToPlayer = player.position - transform.position;
        if (dirToPlayer.magnitude > detectionRadius)  
            return false;
  

        // Step 2: Angle check
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer.normalized);
        if (angleToPlayer > viewAngle / 2f)
            return false;

        // Step 3: Line of sight check (raycast)
        if (Physics.Raycast(transform.position, dirToPlayer.normalized, out RaycastHit hit, detectionRadius))
        {
            if (hit.transform == player)
                return true;
        }

        return false;
    }
}
