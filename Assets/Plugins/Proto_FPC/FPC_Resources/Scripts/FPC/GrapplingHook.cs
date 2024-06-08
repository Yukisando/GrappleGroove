#region

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#endregion

namespace PrototypeFPC
{
    public class GrapplingHook : MonoBehaviour
    {
        // Dependencies
        [Header("Dependencies")]
        [SerializeField] public Dependencies dependencies;

        // Hook properties
        [Header("Hook Properties")]
        [SerializeField] public LayerMask grappleLayerMask;
        [SerializeField] public LayerMask ropeLayerMask;
        [SerializeField] GameObject hookModel;
        [SerializeField] GameObject platformPrefab;
        public float hookDistance = 50f;
        [SerializeField] KeyCode cutRopeKey;
        [SerializeField] KeyCode resetHookKey;
        [SerializeField] float minimumRopeLength = 1f;
        [SerializeField] float releaseImpulseFactor = 50f;
        [SerializeField] float holdDelayToSwing = 0.2f;
        [SerializeField] float playerRetractStrength = 1000f;
        [SerializeField] float retractStrength = 500f;
        [SerializeField] float latchOnImpulse = 200f;

        // Rope properties
        [Header("Rope Properties")]
        [SerializeField] Material ropeMaterial;
        [SerializeField] Color leftClickRopeColor = Color.red;
        [SerializeField] Color rightClickRopeColor = Color.blue;
        [SerializeField] float startThickness = 0.02f;
        [SerializeField] float endThickness = 0.06f;

        // Rope visual spring properties
        [Header("Rope Visual Spring Properties")]
        [SerializeField] int segments = 50;
        [SerializeField] float damper = 12;
        [SerializeField] float springStrength = 800;
        [SerializeField] float speed = 12;
        [SerializeField] float waveCount = 5;
        [SerializeField] float waveHeight = 4;
        [SerializeField] AnimationCurve affectCurve;

        // Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip grapplingSound;
        [SerializeField] AudioClip releaseSound;
        [SerializeField] AudioClip retractSound;

        // Lists to store data
        [HideInInspector] [SerializeField] List<GameObject> hooks, hookModels, hookLatches, ropeColliders;
        [HideInInspector] [SerializeField] List<LineRenderer> ropes;

        AudioSource audioSource;

        bool executeHookSwing;
        RaycastHit hit;
        bool hooked;
        bool hookRelease;
        bool isOptimizing;

        float mouseDownTimer;
        Rigidbody player;
        Ray ray;
        Spring spring;

        //-----------------------

        void Start() {
            Setup();
            CreateSpring();
        }

        void Update() {
            InputCheck();
            CreateHooks(0);
            CreateHooks(1);
            RetractHooks();
            CutRopes();
        }

        void LateUpdate() {
            DrawRopes();
        }

        void Setup() {
            // Setup dependencies
            player = dependencies.rb;
            audioSource = dependencies.audioSourceTop;
        }

        // Create spring
        void CreateSpring() {
            // Create and set rope visual spring value
            spring = new Spring();
            spring.SetTarget(0);
        }

        // Input
        void InputCheck() {
            // Reset checker
            if ((Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) && !Input.GetKey(KeyCode.LeftControl) && !dependencies.isInspecting) {
                mouseDownTimer = 0;
                hookRelease = false;
                executeHookSwing = false;
            }

            // Check input for hook to swing
            if ((Input.GetMouseButton(1) || Input.GetMouseButton(0)) && !Input.GetKey(KeyCode.LeftControl) && !dependencies.isInspecting) {
                mouseDownTimer += Time.deltaTime;

                if (hooked && mouseDownTimer >= holdDelayToSwing && !executeHookSwing) executeHookSwing = true;
            }

            // Check input for hook to latch
            if ((Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0)) && !Input.GetKey(KeyCode.LeftControl) && mouseDownTimer >= holdDelayToSwing && executeHookSwing && !dependencies.isInspecting) {
                executeHookSwing = false;
                hookRelease = true;

                // Get the player's current velocity
                var playerVelocity = player.linearVelocity;
                float speedFactor = playerVelocity.magnitude;

                // Apply an impulse based on the speed at release
                // You might want to adjust the multiplier to get the feel right
                var releaseImpulse = playerVelocity.normalized * (speedFactor * releaseImpulseFactor);
                player.AddForce(releaseImpulse, ForceMode.Impulse);
            }
        }

        // Create Hooks
        void CreateHooks(int mouseButton) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(mouseButton) && !Input.GetKey(KeyCode.LeftControl) && !dependencies.isInspecting) {
                // Check and set target rigidbody if none
                if (Physics.Raycast(ray.origin, ray.direction, out hit, hookDistance, grappleLayerMask, QueryTriggerInteraction.Ignore))
                    if (hit.transform.gameObject.GetComponent<Rigidbody>() == null)

                        // Add and set to kinematic/Static if not manually created/specified
                        hit.transform.gameObject.AddComponent<Rigidbody>().isKinematic = true;

                // Create first hook
                if (!hooked) {
                    if (!Physics.Raycast(ray.origin, ray.direction, out hit, hookDistance, grappleLayerMask, QueryTriggerInteraction.Ignore)) return;
                    if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == player) return;
                    hooked = true;

                    // Create new hook object
                    hooks.Add(new GameObject("Hook"));
                    hooks[^1].transform.position = hit.point;

                    // Add Rigidbody to hook
                    var hookRb = hooks[^1].AddComponent<Rigidbody>();
                    hookRb.isKinematic = true;

                    // Hook end point model
                    hookModels.Add(Instantiate(hookModel, hooks[^1].transform.position, Quaternion.identity));
                    hookModels[^1].transform.parent = hooks[^1].transform;

                    // Hook start point model
                    var spawnPoint = mouseButton == 0 ? dependencies.spawnPointLeft.position : dependencies.spawnPointRight.position;
                    hookModels.Add(Instantiate(hookModel, spawnPoint, Quaternion.identity));
                    hookModels[^1].transform.parent = mouseButton == 0 ? dependencies.spawnPointLeft.transform : dependencies.spawnPointRight.transform;

                    // Set hook rope values
                    ropes.Add(hooks[^1].AddComponent<LineRenderer>());
                    ropes[^1].material = new Material(ropeMaterial); // Create a new instance of the material to modify the color
                    ropes[^1].startWidth = startThickness;
                    ropes[^1].endWidth = endThickness;
                    ropes[^1].numCornerVertices = 2;
                    ropes[^1].numCapVertices = 10;
                    ropes[^1].textureMode = LineTextureMode.Tile;
                    ropes[^1].shadowCastingMode = ShadowCastingMode.On;
                    ropes[^1].receiveShadows = false;
                    ropes[^1].material.color = mouseButton switch {
                        // Set rope color based on mouse button
                        0 => leftClickRopeColor,
                        1 => rightClickRopeColor,
                        _ => ropes[^1].material.color,
                    };

                    // Add and set joint parameters
                    spring.Reset();

                    // spring.SetVelocity(speed);
                    ropes[hooks.Count - 1].positionCount = segments + 1;

                    player.gameObject.AddComponent<SpringJoint>().connectedBody = hooks[^1].GetComponent<Rigidbody>();
                    var sj = player.GetComponent<SpringJoint>();
                    sj.autoConfigureConnectedAnchor = false;
                    sj.connectedAnchor = Vector3.zero;

                    // Calculate the distance between the player and the hook point
                    float distanceFromHook = Vector3.Distance(player.gameObject.transform.position, hooks[^1].transform.position);

                    // Set the maxDistance and minDistance to the initial distance from the hook point
                    sj.maxDistance = mouseButton switch {
                        // Set maxDistance based on mouse button
                        0 => distanceFromHook,
                        1 => distanceFromHook * 3f,
                        _ => sj.maxDistance,
                    };
                    sj.minDistance = mouseButton switch {
                        // Set minDistance based on mouse button
                        0 => distanceFromHook * .025f,
                        1 => distanceFromHook * 0.95f,
                        _ => sj.minDistance,
                    };

                    sj.spring = 20000f; // Increase spring strength to make it tighter
                    sj.damper = 10000f; // Adjust damper to control oscillation

                    // Add collider for rope cutting
                    ropeColliders.Add(new GameObject("RopeCollider"));
                    ropeColliders[^1].transform.parent = hooks[^1].transform;
                    ropeColliders[^1].AddComponent<BoxCollider>().size = new Vector3(0.1f, 0, 0.1f);
                    ropeColliders[^1].GetComponent<BoxCollider>().isTrigger = true;
                    ropeColliders[^1].GetComponent<BoxCollider>().enabled = false;

                    // Knock back when hooked
                    hooks[^1].GetComponent<Rigidbody>().AddForce(ray.direction * (latchOnImpulse * 0.2f), ForceMode.Impulse);

                    // Set previous rope quality to 2 if not already
                    if (ropes.Count > 1)
                        if (ropes[^2].positionCount > 2)
                            ropes[^2].positionCount = 2;

                    // Audio
                    audioSource.PlayOneShot(grapplingSound);
                }

                // Create hook latch
                else if (hooked) {
                    if (!Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, grappleLayerMask, QueryTriggerInteraction.Ignore)) return;
                    if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == player) return;

                    // Create new hook latch object
                    hookLatches.Add(new GameObject("HookLatch"));
                    hookLatches[^1].transform.position = hit.point;

                    // Add Rigidbody to hook latch
                    var latchRb = hookLatches[^1].AddComponent<Rigidbody>();
                    latchRb.isKinematic = true;

                    // Remove hook start point model
                    Destroy(hookModels[^1].gameObject);
                    hookModels.RemoveAt(hookModels.Count - 1);

                    // Add hook latch point model
                    hookModels.Add(Instantiate(hookModel, hookLatches[hooks.Count - 1].transform.position, Quaternion.identity));
                    hookModels[^1].transform.parent = hookLatches[hooks.Count - 1].transform;

                    // Add and set joint parameters
                    spring.Reset();
                    spring.SetVelocity(speed);

                    hookLatches[^1].AddComponent<FixedJoint>().connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();

                    Destroy(player.GetComponent<SpringJoint>());
                    hooks[^1].AddComponent<SpringJoint>().connectedBody = hookLatches[^1].GetComponent<Rigidbody>();
                    var hsj = hooks[^1].GetComponent<SpringJoint>();
                    hsj.autoConfigureConnectedAnchor = false;
                    hsj.anchor = Vector3.zero;
                    hsj.connectedAnchor = Vector3.zero;
                    hsj.spring = 0;
                    hsj.damper = 0f;
                    hsj.maxDistance = 0;
                    hsj.minDistance = 0;

                    // Knock back when hooked
                    hookLatches[^1].GetComponent<Rigidbody>().AddForce(ray.direction * (latchOnImpulse * 0.2f), ForceMode.Impulse);

                    // Set rope width
                    ropes[^1].startWidth = endThickness;
                    ropes[^1].endWidth = endThickness;

                    // Enable rope collider
                    ropeColliders[^1].GetComponent<BoxCollider>().enabled = true;

                    isOptimizing = true;

                    hooked = false;

                    // Calculate the distance between the hooks
                    float ropeLength = Vector3.Distance(hooks[^1].transform.position, hit.point);

                    // Check if the rope is too short
                    if (ropeLength < minimumRopeLength) {
                        // Destroy the last hook and related components if the rope is too short
                        Destroy(hooks[^1]);
                        hooks.RemoveAt(hooks.Count - 1);

                        Destroy(hookLatches[^1]);
                        hookLatches.RemoveAt(hookLatches.Count - 1);

                        Destroy(ropeColliders[^1]);
                        ropeColliders.RemoveAt(ropeColliders.Count - 1);

                        Destroy(ropes[^1]);
                        ropes.RemoveAt(ropes.Count - 1);

                        Destroy(hookModels[^1]);
                        hookModels.RemoveAt(hookModels.Count - 1);

                        audioSource.PlayOneShot(releaseSound);

                        return; // Exit the method as the rope is too short
                    }

                    // Audio
                    audioSource.PlayOneShot(grapplingSound);

                    // Instantiate the plank as a child of the hook latch
                    var plank = Instantiate(platformPrefab, hooks[^1].transform.position, Quaternion.identity);
                    plank.transform.parent = ropes[^1].transform;

                    // Adjust the scale and position of the plank
                    var startPoint = hooks[^1].transform.position;
                    var endPoint = hit.point;
                    var midPoint = (startPoint + endPoint) / 2;
                    plank.transform.position = midPoint;

                    float distance = Vector3.Distance(startPoint, endPoint);
                    plank.transform.localScale = new Vector3(distance, plank.transform.localScale.y, plank.transform.localScale.z);

                    // Adjust the rotation of the plank
                    plank.transform.LookAt(endPoint);
                    plank.transform.Rotate(0, 90, 0); // Rotate 90 degrees to make the plank align with the rope
                }
            }
        }

        // Retract hooked objects
        void RetractHooks() {
            // Set player hook swing strength
            if (executeHookSwing && player.GetComponent<SpringJoint>() && Mathf.Approximately(player.GetComponent<SpringJoint>().spring, playerRetractStrength)) player.GetComponent<SpringJoint>().spring = playerRetractStrength;

            // Set player hook retract strength
            if (!Input.GetMouseButtonDown(2) || dependencies.isInspecting) return;

            if (player.GetComponent<SpringJoint>() != null) player.GetComponent<SpringJoint>().spring = playerRetractStrength;

            // Set all other hook and latched retract strengths
            foreach (var hookJoints in hooks) {
                if (hookJoints.GetComponent<SpringJoint>() && hookJoints.GetComponent<SpringJoint>().connectedBody != player) hookJoints.GetComponent<SpringJoint>().spring = retractStrength;
            }

            if (hooks.Count > 0)

                // Audio
                audioSource.PlayOneShot(retractSound);
        }

        // Cut Ropes
        void CutRopes() {
            // Destroy player hooks upon hold release
            if (hookRelease && hooked) {
                hookRelease = false;

                if (hooks.Count > 0) {
                    Destroy(player.GetComponent<SpringJoint>());
                    Destroy(hooks[^1].gameObject);
                    hooks.RemoveAt(hooks.Count - 1);
                }

                if (hookLatches.Count > 0) {
                    Destroy(hookLatches[^1].gameObject);
                    hookLatches.RemoveAt(hookLatches.Count - 1);
                }

                if (ropeColliders.Count > 0) {
                    Destroy(ropeColliders[^1].gameObject);
                    ropeColliders.RemoveAt(ropeColliders.Count - 1);
                }

                if (hookModels.Count > 0) {
                    Destroy(hookModels[^1].gameObject);
                    hookModels.RemoveAt(hookModels.Count - 1);
                }

                if (ropes.Count > 0) ropes.RemoveAt(ropes.Count - 1);

                hooked = false;
                hookRelease = false;

                // Audio
                audioSource.PlayOneShot(releaseSound);
            }

            // Remove specific hooks
            if (Input.GetKey(cutRopeKey) && !dependencies.isInspecting) {
                // If attached to player
                if (hooked) {
                    if (hooks.Count > 0) {
                        Destroy(player.GetComponent<SpringJoint>());
                        Destroy(hooks[^1].gameObject);
                        hooks.RemoveAt(hooks.Count - 1);
                    }

                    if (hookLatches.Count > 0) {
                        // Remove latch
                        Destroy(hookLatches[^1].gameObject);
                        hookLatches.RemoveAt(hookLatches.Count - 1);
                    }

                    if (ropeColliders.Count > 0) {
                        Destroy(ropeColliders[^1].gameObject);
                        ropeColliders.RemoveAt(ropeColliders.Count - 1);
                    }

                    if (hookModels.Count > 0) {
                        Destroy(hookModels[^1].gameObject);
                        hookModels.RemoveAt(hookModels.Count - 1);
                    }

                    if (ropes.Count > 0) ropes.RemoveAt(ropes.Count - 1);

                    hooked = false;

                    // Audio
                    audioSource.PlayOneShot(releaseSound);
                }

                // At index and are not attached to player
                else if (!hooked && Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, ropeLayerMask)) {
                    if (hit.collider.isTrigger) {
                        int index = GameObjectToIndex(hit.collider.gameObject);

                        Destroy(hooks[index].gameObject);
                        Destroy(hookLatches[index].gameObject);
                        Destroy(ropeColliders[index].gameObject);

                        hooks.RemoveAt(index);
                        hookLatches.RemoveAt(index);
                        ropes.RemoveAt(index);
                        ropeColliders.RemoveAt(index);

                        if (hooks.Count == 0) hooked = false;

                        // Audio
                        audioSource.PlayOneShot(releaseSound);
                    }

                    // Clean up the hook model list if missing after the models get destroyed
                    for (int i = hookModels.Count - 1; i >= 0; i--) {
                        if (hookModels[i] == null) hookModels.RemoveAt(i);
                    }
                }
            }

            // Destroy everything created and clear all lists
            if (Input.GetKeyDown(resetHookKey) && !dependencies.isInspecting) ResetHook();
        }

        public void ResetHook() {
            hooked = false;

            // Destroy joints and objects if any
            if (hooks.Count <= 0) return;

            if (player.GetComponent<SpringJoint>()) Destroy(player.GetComponent<SpringJoint>());

            foreach (var hookObjects in hooks) {
                Destroy(hookObjects);
            }

            foreach (var hookLatchObjects in hookLatches) {
                Destroy(hookLatchObjects);
            }

            foreach (var ropeColliderObjects in ropeColliders) {
                Destroy(ropeColliderObjects);
            }

            foreach (var hookModelObjects in hookModels) {
                Destroy(hookModelObjects);
            }

            // Clear all lists
            hooks.Clear();
            hookModels.Clear();
            hookLatches.Clear();
            ropes.Clear();
            ropeColliders.Clear();

            // Audio
            audioSource.PlayOneShot(releaseSound);
        }

        // Draw ropes
        void DrawRopes() {
            if (ropes.Count != 0 && ropes.Count == hooks.Count)
                for (var i = 0; i < ropes.Count; i++) {
                    if (player.GetComponent<SpringJoint>() != null && player.GetComponent<SpringJoint>().connectedBody == hooks[i].GetComponent<Rigidbody>()) {
                        // Determine the correct spawn point based on the rope color
                        var spawnPoint = ropes[i].material.color == leftClickRopeColor ? dependencies.spawnPointLeft : dependencies.spawnPointRight;

                        // Set spring properties
                        spring.SetDamper(damper);
                        spring.SetStrength(springStrength);
                        spring.Update(Time.deltaTime);

                        var up = Quaternion.LookRotation((hooks[i].transform.position - spawnPoint.position).normalized) * Vector3.up;

                        var currentGrapplePosition = Vector3.zero;
                        Vector3.Lerp(currentGrapplePosition, hooks[i].transform.position, 12f * Time.deltaTime);

                        for (var t = 0; t < segments + 1; t++) {
                            float delta = t / (float)segments;

                            var right = Quaternion.LookRotation((hooks[i].transform.position - spawnPoint.position).normalized) * Vector3.right;

                            var offset = up * (waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta)) + right * (waveHeight * Mathf.Cos(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta));

                            if (ropes[i].positionCount > 2) ropes[i].SetPosition(t, Vector3.Lerp(spawnPoint.position, hooks[i].transform.position, delta) + offset);
                        }
                    }

                    // From hook to latch
                    else if (hooks[i].GetComponent<SpringJoint>() != null && hooks[i].GetComponent<SpringJoint>().connectedBody != player && ropes[i].positionCount > 2) {
                        // Set spring properties
                        spring.SetDamper(damper);
                        spring.SetStrength(springStrength);
                        spring.Update(Time.deltaTime);

                        var up = Quaternion.LookRotation((hooks[i].transform.position - hookLatches[i].transform.position).normalized) * Vector3.up;

                        var currentGrapplePosition = Vector3.zero;
                        Vector3.Lerp(currentGrapplePosition, hooks[i].transform.position, 12f * Time.deltaTime);

                        for (var t = 0; t < segments + 1; t++) {
                            float delta = t / (float)segments;
                            var right = Quaternion.LookRotation((hooks[i].transform.position - hookLatches[i].transform.position).normalized) * Vector3.right;

                            var offset = up * (waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta)) + right * (waveHeight * Mathf.Cos(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta));

                            if (ropes[i].positionCount > 2 && i == ropes.Count || i == ropes.Count - 1) ropes[i].SetPosition(t, Vector3.Lerp(hookLatches[i].transform.position, hooks[i].transform.position, delta) + offset);
                        }

                        // Set rope segments to 2 (start and end) after spring visuals
                        if (isOptimizing) {
                            StartCoroutine(delay());

                            IEnumerator delay() {
                                yield return new WaitForSeconds(1);
                                if (ropes.Count > 1)
                                    if (i != ropes.Count)
                                        ropes[^2].positionCount = 2;

                                isOptimizing = false;
                            }
                        }
                    }

                    // Set rope start and end positions after spring
                    else if (hooks[i].GetComponent<SpringJoint>() != null && hooks[i].GetComponent<SpringJoint>().connectedBody != player && ropes[i].positionCount == 2) {
                        ropes[i].SetPosition(0, hooks[i].transform.position);
                        ropes[i].SetPosition(1, hookLatches[i].transform.position);
                    }

                    // Set rope collider size and position
                    if (ropeColliders.Count > 0 && hooks[i].GetComponent<SpringJoint>() != null) {
                        ropeColliders[i].transform.position = hooks[i].transform.position;
                        ropeColliders[i].transform.LookAt(hooks[i].GetComponent<SpringJoint>().connectedBody.transform.position);
                        ropeColliders[i].GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, Vector3.Distance(hooks[i].transform.position, hooks[i].GetComponent<SpringJoint>().connectedBody.transform.position));
                        float worldZCenter = Vector3.Distance(hooks[i].GetComponent<SpringJoint>().connectedBody.transform.position, hooks[i].transform.position) / 2;
                        ropeColliders[i].GetComponent<BoxCollider>().center = new Vector3(0f, 0f, worldZCenter);
                    }
                }
        }

        // Rope collider Index checker for cutting
        int GameObjectToIndex(GameObject ropeColliderList) {
            for (var i = 0; i < ropeColliders.Count; i++) {
                // Check if a rope collider is in the list
                if (ropeColliders[i] == ropeColliderList)

                    // Return the current index
                    return i;
            }

            // Return if nothing
            return -1;
        }
    }
}