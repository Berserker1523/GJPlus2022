using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RecipesBook : MonoBehaviour
{
    Image bookAnimator;
    private void Start()
    {
        bookAnimator = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void ShowBook()
    {
        gameObject.SetActive(gameObject.activeSelf? false :true);
    }
}
