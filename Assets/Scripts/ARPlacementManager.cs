using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;

    [Header("Placement")]
    public GameObject placementIndicator;

    [Header("AR Quiz")]
    public GameObject arQuizPrefab;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool quizPlaced = false;

    void Start()
    {
        placementIndicator.SetActive(false);
    }

    void Update()
    {
        if (quizPlaced)
            return;

        if (Touchscreen.current == null)
            return;

        if (!Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            return;

        Vector2 touchPosition =
            Touchscreen.current.primaryTouch.position.ReadValue();

        if (raycastManager.Raycast(
            touchPosition,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            placementIndicator.SetActive(true);

            placementIndicator.transform.position = hitPose.position;
            placementIndicator.transform.rotation = hitPose.rotation;

            Instantiate(
                arQuizPrefab,
                hitPose.position,
                hitPose.rotation
            );

            quizPlaced = true;

            placementIndicator.SetActive(false);

            Debug.Log("AR Quiz placed at: " + hitPose.position);
        }
    }
}