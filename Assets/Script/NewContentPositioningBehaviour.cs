using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vuforia;

public class NewContentPositioningBehaviour : VuforiaMonoBehaviour
{
    [SerializeField] AnchorBehaviour anchorStage;
    [SerializeField] PlaneFinderBehaviour planeFinderBehaviour;
    [SerializeField] LeanFingerTap leanFingerTap;
    [SerializeField] GameObject _3DModel;
    bool groundPlanePlaced;

    public void SetPosition(HitTestResult hit)
    {
        _3DModelsManager modelManager = _3DModel.GetComponent<_3DModelsManager>();

        if (!groundPlanePlaced)
        {
            anchorStage = VuforiaBehaviour.Instance.ObserverFactory.CreateAnchorBehaviour("Ground Plane", hit);
            
            if (_3DModel.transform.childCount == 0)
            {
                modelManager.InstantiateModel(2, hit.Position, hit.Rotation);
            }
            groundPlanePlaced = true;
            leanFingerTap.enabled = true;
        }
        else
        {
            modelManager.InstantiateModel(2, hit.Position, hit.Rotation);
        }
    }

    public void HitTest()
    {
        planeFinderBehaviour.PerformHitTest(new Vector2());
    }

    public void DestroyPlanes()
    {
        foreach (Transform child in _3DModel.transform)
        {
            Destroy(child.gameObject);
        }
        anchorStage.UnconfigureAnchor();
        groundPlanePlaced = false;
        leanFingerTap.enabled = false;
    }
}