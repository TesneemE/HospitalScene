using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class ClipboardSound : MonoBehaviour
{
    public GameObject player;
    public float nearDistance = 5.0f;
    public float minDistance = 1.0f;
    public AudioSource audioSource;
    public AudioClip pickupAudio;

    private bool isPickedUp = false;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Coroutine audioCoroutine;

    public ClipboardDialogue clipBoardDialogue;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnKeyPickup);
    }

    void Update()
    {
        // Optional debug pickup key
        if (Input.GetKeyDown(KeyCode.B))
        {
            OnKeyPickup(default); // Manually trigger pickup
        }

        if (!isPickedUp)
        {
            bool checkNear = IsNearTarget(player);

            if (checkNear)
            {
                if (audioCoroutine == null)
                {
                    audioCoroutine = StartCoroutine(PlayAudioRandomly());
                }
            }
            else
            {
                if (audioCoroutine != null)
                {
                    StopCoroutine(audioCoroutine);
                    audioCoroutine = null;
                }
            }
        }
    }

    bool IsNearTarget(GameObject target)
    {
        if (target == null) return false;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance <= nearDistance;
    }

    IEnumerator PlayAudioRandomly()
    {
        while (!isPickedUp && IsNearTarget(player))
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            float volume = Mathf.Lerp(0f, 1f, (nearDistance - distance) / nearDistance);
            audioSource.volume = volume;
            audioSource.clip = pickupAudio;
            audioSource.Play();

            float randomDelay = Random.Range(1f, 5f);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    private void OnKeyPickup(SelectEnterEventArgs args)
    {
        if (isPickedUp) return; // Prevent duplicate triggers

        isPickedUp = true;

        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }

        audioSource.Stop();

        if (clipBoardDialogue != null)
        {
            clipBoardDialogue.enabled = true;
        }
    }
}