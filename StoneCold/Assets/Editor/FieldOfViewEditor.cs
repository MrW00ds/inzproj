using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIScript))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        AIScript fov = (AIScript)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.up, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.z, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.z, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerZ, float angleInDegrees)
    {
        angleInDegrees += eulerZ;

        return new Vector3(Mathf.Cos((angleInDegrees + 90) * Mathf.Deg2Rad), Mathf.Sin((angleInDegrees + 90) * Mathf.Deg2Rad),0);
    }
}
