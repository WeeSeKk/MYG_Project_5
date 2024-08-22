using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Model", menuName = "ScriptableObjects/3DModels", order = 1)]

public class SpawnManagerScriptableObject : ScriptableObject
{
    public GameObject model3D;
    public Texture2D modelImage;
    public string productName;
    public string productDescription;
    public float productPrice;
    public string modelID;
}