using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    public class TreeSpriteChanger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private List<Sprite> treeSprites = new();

        private void Start() =>
            spriteRenderer.sprite = treeSprites[Random.Range(0, treeSprites.Count)];
    }
}
