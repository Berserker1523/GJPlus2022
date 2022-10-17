using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class TreeSpriteChanger : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private List<Sprite> treeSprites = new();

        private void Start() =>
            image.sprite = treeSprites[Random.Range(0, treeSprites.Count)];
    }
}
