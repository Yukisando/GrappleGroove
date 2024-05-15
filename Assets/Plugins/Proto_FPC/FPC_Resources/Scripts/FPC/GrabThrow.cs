//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using UnityEngine;
using PrototypeFPC;

namespace PrototypeFPC
{
    public class GrabThrow : MonoBehaviour
    {
        //Dependencies
        [Header("Dependencies")]
        [SerializeField] Dependencies dependencies;

        //Input properties
        [Header("Input Properties")]
        [SerializeField] KeyCode grabThrowKey = KeyCode.G;

        //Grab and throw properties
        [Header("Grab/Throw Properties")]
        [SerializeField] float maxGrabDistance = 8f;
        [SerializeField] float grabSpeed = 15;
        [SerializeField] float throwForce = 800f;
        [SerializeField] GameObject grabIcon;

        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip grabSound;
        [SerializeField] AudioClip throwSound;

        //Helpers
        Camera cam;

        Rigidbody rb;
        Rigidbody grabbedObject;

        Transform grabPoint;
        Transform originalParent;

        AudioSource audioSource;

        Ray ray;
        RaycastHit hit;


        //-----------------------


        //Functions
        ///////////////

        void Start()
        {
            Setup(); //- Line 74
        }

        void Update()
        {
            GrabHoldThrow(); //- Line 88
        }

        void FixedUpdate()
        {
            Hold(); //- Line 137
        }


        //-----------------------


        void Setup()
        {
            //Setup dependencies
            cam = dependencies.cam;
            rb = dependencies.rb;
            grabPoint = dependencies.grabPoint;
            audioSource = dependencies.audioSourceTop;

            //Create and make grab point A kinematic rigidbody
            grabPoint.gameObject.AddComponent<Rigidbody>().useGravity = false;
            grabPoint.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }


        void GrabHoldThrow()
        {
            //Track the mouse position for raycasting
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(dependencies.isGrabbing && grabbedObject != null)
            {
                if(Input.GetKeyDown(grabThrowKey))
                {
                    //Apply force, throw the object
                    grabbedObject.AddForce(dependencies.cam.transform.forward *  throwForce, ForceMode.Impulse);
                    grabbedObject = null;

                    dependencies.isGrabbing = false;

                    //Disable grab icon
                    grabIcon.SetActive(false);

                    //Audio
                    audioSource.PlayOneShot(throwSound);
                }
            }

            else if(!dependencies.isGrabbing && !dependencies.isInspecting)
            {
                if(Input.GetKeyDown(grabThrowKey))
                {
                    if(Physics.Raycast(ray.origin, ray.direction, out hit, maxGrabDistance, ~(1 << LayerMask.NameToLayer("Ignore Raycast")), QueryTriggerInteraction.Ignore))
                    {
                        if(hit.collider.gameObject.GetComponent<Rigidbody>() != null && !hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic)
                        {
                            //Set grab object and point position
                            grabPoint.position = hit.point;
                            grabbedObject = hit.collider.gameObject.GetComponent<Rigidbody>();

                            dependencies.isGrabbing = true;

                            //Enable grab icon
                            grabIcon.SetActive(true);

                            //Audio
                            audioSource.PlayOneShot(grabSound);
                        }
                    }
                }
            }
        }


        void Hold()
        {
            if(dependencies.isGrabbing && grabbedObject != null)
            {
                //Move the grabbed object towards grab point
                grabbedObject.linearVelocity = grabSpeed * (grabPoint.position - grabbedObject.transform.position);
            }
        }
    }
}
