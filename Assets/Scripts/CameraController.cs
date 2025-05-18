using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float minX = -10f, maxX = 20f;
    public float minY = 10f, maxY = 30f;

    private void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) movement += transform.up;
        if (Input.GetKey(KeyCode.S)) movement -= transform.up;
        if (Input.GetKey(KeyCode.A)) movement -= transform.right;
        if (Input.GetKey(KeyCode.D)) movement += transform.right;

        movement = movement.normalized * moveSpeed * Time.deltaTime;
        Vector3 newPos = transform.position + movement;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        newPos.z = transform.position.z;

        transform.position = newPos;
    }
}
