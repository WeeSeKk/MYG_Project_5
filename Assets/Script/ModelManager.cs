using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ModelManager : MonoBehaviour
{
    LeanDragTranslate dragTranslate;
    Vector3 originalTransform;
    public float rotateSpeed = -0.3f;
    public bool rotate;

    // Start is called before the first frame update
    void Start()
    {
        dragTranslate = GetComponent<LeanDragTranslate>();
        originalTransform = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.y = originalTransform.y;
        transform.position = position;

        TouchRotate();
    }

    void TouchRotate()
    {
        if (LeanTouch.Fingers.Count == 1 && rotate)
        {
            LeanFinger finger = LeanTouch.Fingers[0];

            if (finger.StartedOverGui == false)
            {
                transform.Rotate(Vector3.up, finger.ScaledDelta.x * rotateSpeed);
            }
        }
    }
}