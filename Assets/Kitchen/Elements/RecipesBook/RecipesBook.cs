using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipesBook : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowBook()
    {
        gameObject.SetActive(gameObject.activeSelf? false :true);
    }
}
