using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ModelManager : MonoBehaviour
{
    LeanDragTranslate dragTranslate;
    Vector3 originalTransform;

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
    }
}