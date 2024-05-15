//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using System.Collections.Generic;
using UnityEngine;
using PrototypeFPC;

namespace PrototypeFPC
{
    public class Inspect : MonoBehaviour
    {
        //Dependencies
        [Header("Dependencies")]
        [SerializeField] Dependencies dependencies;

        //Input properties
        [Header("Input Properties")]
        [SerializeField] KeyCode addToListKey = KeyCode.I;
        [SerializeField] KeyCode inspectKey = KeyCode.E;

        //Inspection properties
        [Header("Inspection Properties")]
        [SerializeField] GameObject inspectIcon;
        [SerializeField] GameObject aimDot;
        [SerializeField] float maxPickupDistance = 6;
        [SerializeField] float pickupSpeed = 5f;
        [SerializeField] float rotateSpeed = 2f;
        [SerializeField] float zoomSpeed = 0.2f;

        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip pickUpSound;
        [SerializeField] AudioClip putDownSound;
        [SerializeField] AudioClip zoomSound;

        //Inspect & Disable Lists
        [Header("Inspect & Disable Lists")]
        [SerializeField] List<Collider> objectsToInspect;
        [SerializeField] List<Collider> objectsToIgnore;

        //Helpers
        float rotX, rotY;
        
        Camera cam;
        Transform inspectPoint;
        GameObject inspectedObject;

        Vector3 objectOrigin;
        Vector3 originalDistance;

        Quaternion objectRotation;
        AudioSource audioSource;

        Ray ray;
        RaycastHit hit;


        //-----------------------


        //Functions
        ///////////////

        void Start()
        {
            Setup(); //- Line 81
        }

        void Update()
        {
            Inspection(); //- Line 89
        }


        //-----------------------


        void Setup()
        {
            //Setup dependencies
            cam = dependencies.cam;
            inspectPoint = dependencies.inspectPoint;
            audioSource = dependencies.audioSourceTop;
        }

        void Inspection()
        {
            //Track the mouse position for raycasting
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Revert if already inspecting
            if(dependencies.isInspecting)
            {
                if(Input.GetKeyDown(inspectKey))
                {
                    dependencies.isInspecting = false;

                    //Enable object collider
                    inspectedObject.GetComponent<Collider>().enabled = true;

                    //Audio
                    audioSource.PlayOneShot(putDownSound);
                }
            }

            //Inspect if not already
            else if(!dependencies.isInspecting && !dependencies.isGrabbing)
            {
                if(Physics.Raycast(ray.origin, ray.direction, out hit, maxPickupDistance, ~(1 << LayerMask.NameToLayer("Ignore Raycast")), QueryTriggerInteraction.Ignore))
                {
                    if(objectsToInspect.Contains(hit.collider))
                    {
                        aimDot.SetActive(false);
                        inspectIcon.SetActive(true);
                    }

                    else
                    {
                        inspectIcon.SetActive(false);
                        aimDot.SetActive(true);
                    }

                    if(!objectsToIgnore.Contains(hit.collider))
                    {
                        //Objects that can and connot be inspected
                        if(Input.GetKeyDown(addToListKey))
                        {
                            //Remove object from inspection list
                            if(objectsToInspect.Contains(hit.collider))
                            {
                                objectsToInspect.Remove(hit.collider);
                            }

                            //Add object to inspection list
                            else
                            {
                                objectsToInspect.Add(hit.collider);
                            }
                        }

                        //Inspect object if found in inspection list
                        else if(Input.GetKeyDown(inspectKey))
                        {
                            if(objectsToInspect.Contains(hit.collider))
                            {
                                dependencies.isInspecting = true;
                                inspectIcon.SetActive(false);
                                aimDot.SetActive(false);

                                //Record the original position and rotation of the inspected object
                                inspectedObject = hit.collider.gameObject;
                                objectOrigin = inspectedObject.transform.position;
                                objectRotation = inspectedObject.transform.rotation;
                                originalDistance = inspectPoint.localPosition;

                                //Set kinematic if rigidbody found
                                if(inspectedObject != null && inspectedObject.GetComponent<Rigidbody>())
                                {
                                    inspectedObject.GetComponent<Rigidbody>().isKinematic = true;
                                }

                                //Disable object collider
                                inspectedObject.GetComponent<Collider>().enabled = false;

                                //Audio
                                audioSource.PlayOneShot(pickUpSound);
                            }
                        }
                    }
                }

                else
                {
                    if(inspectIcon.activeSelf || !aimDot.activeSelf)
                    {
                        inspectIcon.SetActive(false);
                        aimDot.SetActive(true);
                    }
                }
            }

            if(dependencies.isGrabbing)
            {
                inspectIcon.SetActive(false);
            }
            
            //Inspection
            if(dependencies.isInspecting && inspectedObject != null)
            {
                //Move object position to inspection point
                inspectedObject.transform.position = Vector3.Lerp(inspectedObject.transform.position, inspectPoint.position, pickupSpeed * Time.deltaTime);

                //Get and set mouse input axis
                rotX += Input.GetAxisRaw("Mouse X");
                rotY += Input.GetAxisRaw("Mouse Y");

                //Set object rotation based on input
                inspectedObject.transform.localRotation = Quaternion.Euler(inspectedObject.transform.localRotation.x + rotY * rotateSpeed, inspectedObject.transform.localRotation.y - rotX * rotateSpeed, 0);

                //Set inspection distance
                if(Input.mouseScrollDelta.y != 0)
                {
                    inspectPoint.localPosition = new Vector3(inspectPoint.localPosition.x, inspectPoint.localPosition.y, inspectPoint.localPosition.z + (Input.mouseScrollDelta.y * zoomSpeed));

                    //Audio
                    audioSource.PlayOneShot(zoomSound);
                }
            }

            //Exit inspection
            else if(!dependencies.isInspecting && inspectedObject != null)
            {
                //Reset object position and rotation to original
                inspectedObject.transform.position = objectOrigin;
                inspectedObject.transform.rotation = objectRotation;
                inspectPoint.localPosition = originalDistance;

                rotX = 0f;
                rotY = 0f;

                //Revert kinematic if rigidbody found
                if(inspectedObject.GetComponent<Rigidbody>())
                {
                    inspectedObject.GetComponent<Rigidbody>().isKinematic = false;
                }

                inspectedObject = null;
            }
        }
    }
}
