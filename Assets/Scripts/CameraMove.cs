
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Camera cam;
    public GameObject Ghost;
    public GameObject Girl;
    public Players playerUiControl;
    // Use this for initialization
    void Start()
    {


        cam = GetComponent<Camera>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        FixedCameraFollowSmooth(cam, Ghost.transform, Girl.transform);
    }

    public void FixedCameraFollowSmooth(Camera cam, Transform t1, Transform t2)
    {
        // How many units should we keep from the players
        float zoomFactor = 0.3f;
        float followTimeDelta = 1f;

        // Midpoint we're after
        Vector3 midpoint = (t1.position + t2.position) / 2f;

        // Distance between objects
        float distance = (t1.position - t2.position).magnitude;
        if (distance < 5f)
            distance = 5f;
        else if (distance >= 7f && distance < 28)
        {
            distance = 7f;
        }
        else if (distance >= 28f)
        {
            distance *= 0.25f;
            playerUiControl.DealWithHealth(1);
        }
        //else if (distance > 7f)
        //distance = 7f;
        // Move camera a certain distance

        Vector3 cameraDestination = midpoint - cam.transform.forward * distance * zoomFactor;

        // Adjust ortho size if we're using one of those
        if (cam.orthographic)
        {
            // The carmera's forward vector is irrelevant, only this size will matter
            cam.orthographicSize = distance;

        }
        // You specified to use MoveTowards instead of Slerp
        cam.transform.position = Vector3.Slerp(cam.transform.position * Time.deltaTime, cameraDestination, followTimeDelta);

        // Snap when close enough to prevent annoying slerp behavior
        if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
            cam.transform.position = cameraDestination;
    }
}
