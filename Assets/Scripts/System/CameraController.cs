using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isMoving;
    public bool isZoomedIn;

    public IEnumerator MoveCam(Vector3 endPos, float speed, float waitTime = 0f)
    {
        var startTime = Time.time;
        while (Time.time - startTime < waitTime) yield return null;

        var startPos = transform.position;
        isMoving = true;

        startTime = Time.time;
        var fraction = 0f;
        var distance = Vector3.Distance(startPos, endPos);

        while (fraction < 1f)
        {
            fraction           = (Time.time - startTime) * speed / distance;
            transform.position = Vector3.Lerp(startPos, endPos, fraction);
            yield return null;
        }

        isMoving = false;
    }

    public void ZoomIn(Vector3 islandPos)
    {
        isZoomedIn = true;
        StartCoroutine(MoveCam(new Vector3(islandPos.x, Constants.CamYValueZoomedIn, islandPos.z - Constants.CamYValueZoomedIn),
                               Constants.CamZoomSpeed));
    }

    public void ZoomOut()
    {
        isZoomedIn = false;
        StartCoroutine(MoveCam(Constants.CamDefaultPosition, Constants.CamZoomSpeed));
    }
}