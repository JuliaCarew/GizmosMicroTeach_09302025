using UnityEngine;

public class Detection : MonoBehaviour
{
    [Range(0f, 1f)] public float opacity = 0.25f; // Transparency for detection range visuals
    public Color sphereColor = Color.yellow;
    public Color coneColor = Color.red;
    public Color coneDetectedColor = Color.green;

    // Icon file names (in Assets/Gizmos folder)
    public string alertIcon = "alert_icon.png";   // shown when player detected
    public string idleIcon = "idle_icon.png";     // shown when player not detected

    void OnDrawGizmosSelected()
    {
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai == null) return; // only draw if enemy has AI component

        // STEP 1: Draw detection sphere
        Color transparentSphere = new Color(sphereColor.r, sphereColor.g, sphereColor.b, opacity);
        Gizmos.color = transparentSphere;
        Gizmos.DrawSphere(transform.position, ai.detectionRadius);

        // STEP 2: Build cone mesh
        Mesh coneMesh = CreateViewConeMesh(ai.detectionRadius, ai.viewAngle, 30);

        // STEP 3: Change cone color and display icon based on detection
        if (ai.CanSeePlayer())
        {
            // Player detected -> green cone + alert icon
            Gizmos.color = coneDetectedColor;
            Gizmos.DrawIcon(transform.position + Vector3.up * 2f, alertIcon, true); 
        }
        else
        {
            // Player not detected -> red cone + idle icon
            Gizmos.color = coneColor;
            Gizmos.DrawIcon(transform.position + Vector3.up * 2f, idleIcon, true); 
        }

        // STEP 3: Draw cone mesh
        Gizmos.DrawMesh(coneMesh, transform.position, transform.rotation);
    }

    Mesh CreateViewConeMesh(float radius, float angle, int segments)
    {
        Mesh mesh = new Mesh();

        // Define vertices (cone center + outer edge points)
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        // First vertex = origin (center point)
        vertices[0] = Vector3.zero; 

        // Build outer arc vertices based on the view angle
        float halfAngle = angle * 0.5f;
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -halfAngle + (angle * i / segments);
            float rad = currentAngle * Mathf.Deg2Rad;

            // Position each vertex along the arc
            vertices[i + 1] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        // Connect vertices into triangle fan (center -> edges)
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0; // center
            triangles[i * 3 + 1] = i + 1; // current arc vertex
            triangles[i * 3 + 2] = i + 2; // next arc vertex
        }

        // Assign vertices/triangles to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
