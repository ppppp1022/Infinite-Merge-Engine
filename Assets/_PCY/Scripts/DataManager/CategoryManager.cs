using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryManager : MonoBehaviour
{
    public List<string> categories = new List<string>();
    public int FindCategoryAboutItem(string category)
    {
        if(categories.Contains(category))
        {
            return categories.IndexOf(category);
        }
        else
        {
            categories.Add(category);
            return categories.IndexOf(category);
        }
    }
}
