using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    public class WaterColliderSwitcher : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        BoxCollider2D boxCollider2D;

        //Small Collider
        Vector2 smallSize = new Vector2(1.01f, 1.37f);
        Vector2 smallOffset = new Vector2(0.007f, -0.01f);

        //Large collider
        Vector2 largeSize = new Vector2(1.8f, 4.1f);
        Vector2 largeOffset= new Vector2(0.009f, -0.25f) ;

        private void Start()=>       
            boxCollider2D= GetComponent<BoxCollider2D>(); 
        

        public void OnBeginDrag(PointerEventData eventData)
        {
            boxCollider2D.size = smallSize;
            boxCollider2D.offset = smallOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            boxCollider2D.size = largeSize;
            boxCollider2D.offset = largeOffset;
        }
    }

}
