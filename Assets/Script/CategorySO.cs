using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Category", menuName = "ScriptableObjects/Category Item", order = 2)]

public class CategoryScriptableObject : ScriptableObject
{
    public string categoryHeader;
    public List<string> categoryItems = new List<string>();
}
