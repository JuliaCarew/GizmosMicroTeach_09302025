using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform endPoint;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, endPoint.position);
    }


}
