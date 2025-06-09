using UnityEngine;
using UnityEngine.InputSystem; // For Input System
public class DoorOpen : MonoBehaviour
{
    public Transform door;                    
    public Transform key;                    
    public GameObject staticPadlock;          
    public GameObject fallingPadlock;        
    public float openAngle = 90f;             
    public float speed = 2f;                  
    public float detectionDistance = 2.0f;    
    public AudioSource audioSource;          
    public AudioClip openSound;              
    public GameObject clipBoard;
    public ClipboardSound clipboardSound;
    private bool isOpen = false;
    private bool hasUnlocked = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    public KeySound keySound;
    void Start()
    {
        closedRotation = door.rotation;
        openRotation = Quaternion.Euler(door.eulerAngles + new Vector3(0, openAngle, 0));
        if (fallingPadlock != null)
            fallingPadlock.SetActive(false);
    }

    void Update()
    {
        if (!hasUnlocked && Vector3.Distance(key.position, transform.position) < detectionDistance)
        {
            UnlockDoor();
        }

        if (isOpen)
        {
            door.rotation = Quaternion.Lerp(door.rotation, openRotation, Time.deltaTime * speed);
        }
     if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Unlocking Door");
                //  UnlockDoor();
                        Vector3 offset = new Vector3(0.1f, 0f, 0.1f); 
        key.transform.position = door.transform.position + offset;
        key.transform.rotation = door.transform.rotation; 
        }
    }
private void ActivateSound()
{
     isOpen = true;
          if(clipboardSound!=null){
                clipboardSound.enabled=true;
            }
}
private void DeactivatePadlock()
{
   fallingPadlock.SetActive(false);
}
    private void UnlockDoor()
    {
        Debug.Log("Happening rn");
        hasUnlocked = true;
        // isOpen = true;
        if(clipBoard!=null){
            clipBoard.SetActive(true);
          Invoke("ActivateSound", 2f);
          }
          keySound.enabled=false;
        if (audioSource && openSound)
            audioSource.PlayOneShot(openSound);

        if (staticPadlock)
            staticPadlock.SetActive(false);

        if (fallingPadlock)
        {
            fallingPadlock.SetActive(true);
            Rigidbody rb = fallingPadlock.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;  // make padlock fall
                  Invoke("DeactivatePadlock", 1f);
        }
    }
}
