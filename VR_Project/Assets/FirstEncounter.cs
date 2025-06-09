using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem; // For Input System
using System.Collections;

public class FirstEncounter : MonoBehaviour
{
    public Animator animator;
    public string animationStateName = "BallRoll";
    public AudioSource audioSource;
    public AudioClip clipDuringAnimation;
    public AudioClip slenderClip1;
    public AudioClip slenderClip2;
    public AudioClip afterPickUp;

    public GameObject[] lightObjects; // store all light GameObjects
    private Light[] lights; //  Light components
    private bool[] lightsOn; //  track whether each light is on or off

    private bool animationStarted = false;
    private bool animationEnded = false;

    private bool isDrainingHealth = false;
    public GameObject enemy; //
    public GameObject flashlight;
    public AudioSource slenderSource;
    public SlenderManLogic slenderManLogic; // ref to slender script
    public PlayerHealth playerHealth;
    public float healthDecreaseRate = 1f;
    public GameObject player;
    public Camera mainCam;
    public KeySound keySound; // clipboard sound script
    public Light playerLight;
    public Light flashlightLight;
    public float offsetDistance = 2.0f;    // Distance in front of enemy
    public float heightOffset = 1.5f; 
    void Start()
    {
       if(flashlight!=null){
            flashlight.SetActive(false);
       }
       
        lights = new Light[lightObjects.Length];
        lightsOn = new bool[lightObjects.Length];

        // turn off lights
        for (int i = 0; i < lightObjects.Length; i++)
        {
            lights[i] = lightObjects[i].GetComponent<Light>();
            lightsOn[i] = false; 
        }
        if (enemy != null)
        {
            enemy.SetActive(false); //deactivate slender
        }
        playerLight.enabled=true;
    }

    void Update()
    {
            if (isDrainingHealth)
    {
        // Continuously drain health
        playerHealth.TakeDamage(healthDecreaseRate * Time.deltaTime);
    }
       
  if (Input.GetKeyDown(KeyCode.A))
{
    // playerLight.enabled = true;

    // if (flashlightLight != null)
    // {
    //     flashlightLight.enabled = true;
    // }
    FlashlightHit();
}

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // play laughing clip
        if (!animationStarted && stateInfo.IsName(animationStateName))
        {
            animationStarted = true;
            audioSource.clip = clipDuringAnimation;
            audioSource.loop = false;
            audioSource.Play();
        }

        
        if (animationStarted && !animationEnded && stateInfo.normalizedTime >= 1f)
        {
            Invoke("ActivateSlender", 3f); // activate slender after ball anim
            animationEnded = true;
        }
    }

    // 
    private void ActivateSlender()
    {
        audioSource.Stop();

        if (enemy != null)
        {
                                Vector3 enemyPosition = enemy.transform.position;
        
       //update x+z
        enemyPosition.x =mainCam.transform.position.x + mainCam.transform.forward.x * 2f; // Move in front on the X-axis
        enemyPosition.z = mainCam.transform.position.z + mainCam.transform.forward.z * 2f; // Move in front on the Z-axis
        
        enemy.transform.position = enemyPosition;
            enemy.SetActive(true); // activate slender
             Invoke("PlayFirstAudio", 0.5f); //play all work audio
             isDrainingHealth = true; //drain health
          
        }
    }
public void PlayFirstAudio(){ //play all work audio
           if (slenderClip1 != null)
        {

            slenderSource.clip = slenderClip1;
            slenderSource.loop = false;
            slenderSource.Play();

         
        }
        playerLight.enabled=false;
        Invoke("PlaySlenderAudioSequence", 3f);
}
    public void PlaySlenderAudioSequence()
    {
        // play static audio
        if (slenderClip2 != null && enemy.activeSelf==true && flashlight!=null)
        {
            audioSource.clip = afterPickUp; //play get rid of it audio
            audioSource.loop = true;
            audioSource.Play();
            flashlight.SetActive(true);
            slenderSource.clip = slenderClip2;
            slenderSource.loop = true;
            slenderSource.Play();
        }
    }

    public void ActivateEnemy() //for when script ends after get rid of slender
    {
         enemy.transform.position = new Vector3(9999f, -100f, 9999f);  //teleport far
        enemy.SetActive(true); //activate slender
        slenderSource.Play();
        Invoke("ActivateScript", 1f);
    }
    public void ActivateScript(){
                if (slenderManLogic != null) //enable slender script
        {
            slenderManLogic.enabled = true;
        }
        // gameObject.SetActive(false); //deactivate ball
         
    }

    // when flashlight detects slender
    public void FlashlightHit()
    {
        audioSource.Stop();
        // slenderSource.Stop();
        gameObject.transform.position = new Vector3(9999f, -100f, 9999f);  //teleport ball far
        if (isDrainingHealth)
        {
            // stop health drain reset health
            isDrainingHealth = false;
            playerHealth.ResetHealth();
            //  playerHealth.TakeDamage(-1*healthDecreaseRate * Time.deltaTime);

            if (enemy != null)
            {
                slenderSource.Stop(); //stop audio
                enemy.SetActive(false); // deactivate the enemy
            }
                    // turn on lights
        for (int i = 0; i < lightObjects.Length; i++)
        {
            if (!lightsOn[i])
            {
                lights[i].enabled = true; 
                lightsOn[i] = true; 
            }
        }

            Invoke("ActivateEnemy", 10f); //activate after 10sec

            if (keySound != null)
            {
                keySound.enabled = true; // turn on clipboard script
            }
        }
    }
}
// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.InputSystem; // For Input System
// using System.Collections;

// public class FirstEncounter : MonoBehaviour
// {
//     public Animator animator;
//     public string animationStateName = "BallRoll";
//     public AudioSource audioSource;
//     public AudioClip clipDuringAnimation;
//     public AudioClip slenderClip1;
//     public AudioClip slenderClip2;
//     public AudioClip afterPickUp;

//     private bool animationStarted = false;
//     private bool animationEnded = false;

//     private bool isDrainingHealth = false;
//     public GameObject enemy; // The enemy to activate
//     public AudioSource slenderSource;
//     public SlenderManAI slenderAI; // Reference to slender script
//     public PlayerHealth playerHealth;
//     public GameObject player;
//     public KeySound keySound; // Key sound script

//     void Start()
//     {
//         if (enemy != null)
//         {
//             enemy.SetActive(false); // Enemy starts inactive
//         }
//     }

//     void Update()
//     {
//         AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

//         // Start playing Clip A when animation starts
//         if (!animationStarted && stateInfo.IsName(animationStateName))
//         {
//             animationStarted = true;
//             audioSource.clip = clipDuringAnimation;
//             audioSource.loop = false;
//             audioSource.Play();
//         }

//         // After animation ends, play Clip B on loop
//         if (animationStarted && !animationEnded && stateInfo.normalizedTime >= 1f)
//         {
//             Invoke("ActivateSlender", 3f); // After 3 seconds, trigger SlenderMan's appearance
//             animationEnded = true;
//         }
//     }

//     // This function will be called after the ball animation ends
//     private void ActivateSlender()
//     {
//         audioSource.Stop();
//         if (slenderClip1 != null)
//         {
//             slenderSource.clip = slenderClip1;
//             slenderSource.loop = false;
//             slenderSource.Play();
//         }

//         if (enemy != null)
//         {
//             enemy.SetActive(true); // Activate the enemy
//             StartCoroutine(PlaySlenderAudioSequence()); // Start playing audio sequence
//         }

//         isDrainingHealth = true; // Start draining health
//     }

//     private IEnumerator PlaySlenderAudioSequence()
//     {
//         // Play slenderClip1 first
//         if (slenderClip1 != null)
//         {
//             slenderSource.clip = slenderClip1;
//             slenderSource.loop = false;
//             slenderSource.Play();

//             yield return new WaitForSeconds(slenderClip1.length);
//         }

//         // Play slenderClip2 next
//         if (slenderClip2 != null)
//         {
//             audioSource.clip = afterPickUp;
//             audioSource.loop = true;
//             audioSource.Play();

//             slenderSource.clip = slenderClip2;
//             slenderSource.loop = true;
//             slenderSource.Play();
//         }
//     }
//     public void ActivateScript(){
//         if(slenderAI!=null)
//         {
//             slenderAI.enabled=true;
//         }
//     }

//     // This method will be triggered when the flashlight detects the SlenderMan (when the flashlight hits him)
//     private void FlashlightHit()
//     {
//         gameObject.SetActive(false);
//         if (isDrainingHealth)
//         {
//             // Stop health drain and reset everything
//             isDrainingHealth = false;
//             playerHealth.ResetHealth();

//             if (enemy != null)
//             {
//                 enemy.SetActive(false); // Deactivate the enemy
//             }
//             Invoke("ActivateScript", 5f);
//             // if (slenderAI != null)
//             // {
//             //     slenderAI.enabled = true; // Enable SlenderMan AI after being hit by the flashlight
//             // }

//             if (keySound != null)
//             {
//                 keySound.enabled = true; // Enable key sound
//             }

//             slenderSource.Stop(); // Stop the SlenderMan sound
//         }
//     }
// }