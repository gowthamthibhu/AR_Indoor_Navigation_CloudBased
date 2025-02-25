using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;
using System.Collections.Generic;

public class AnchorPlacement : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public ARAnchorManager anchorManager;
    public GameObject anchorPrefab; // Optional, for visualization
    public List<ARCloudAnchor> cloudAnchors = new List<ARCloudAnchor>(); // Store placed anchors

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose pose = hits[0].pose;
                    PlaceAnchor(pose);
                }
            }
        }
    }

    void PlaceAnchor(Pose pose)
    {
        ARAnchor anchor = anchorManager.AddAnchor(pose);
        if (anchor != null)
        {
            ARCloudAnchor cloudAnchor = anchor.gameObject.AddComponent<ARCloudAnchor>(); // Attach Cloud Anchor
            cloudAnchors.Add(cloudAnchor);

            if (anchorPrefab != null)
            {
                Instantiate(anchorPrefab, pose.position, pose.rotation); // Place a visual marker
            }

            Debug.Log("✅ Anchor Placed!");
        }
        else
        {
            Debug.LogError("❌ Failed to place anchor!");
        }
    }
}
