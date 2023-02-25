using UnityEngine;

namespace Kitchen
{
    public class PotionResultPositioner : MonoBehaviour
    {
        [SerializeField] private GameObject potionSpriteObject;
        [SerializeField] private GameObject objectToPosition;
        [SerializeField] private Transform position1;
        [SerializeField] private Transform position2;

        private void Awake()
        {
            bool isInOddIndex = transform.parent.GetSiblingIndex() % 2 == 1;
            potionSpriteObject.transform.localScale = new Vector3(potionSpriteObject.transform.localScale.x * (isInOddIndex ? 1 : -1), 
                potionSpriteObject.transform.localScale.y, potionSpriteObject.transform.localScale.z);
            objectToPosition.transform.position = isInOddIndex ? position1.position : position2.position;
        }
    }
}
