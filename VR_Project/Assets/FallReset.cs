using UnityEngine;
using UnityEngine.InputSystem; // For Input System

public class FallReset : MonoBehaviour
{
    public float minY = 0.67f; // Y threshold
    public Vector3 resetPosition = new Vector3(1.19f, 1.034f, -8.56f); // Safe respawn position
    public Vector3 testPosition = new Vector3(0.0f, 0.0f, 0.0f); // Position for test keypress

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.position = testPosition;
        }

        if (transform.position.y < minY)
        {
            transform.position = resetPosition;

            // Reset velocity if Rigidbody is present
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}


