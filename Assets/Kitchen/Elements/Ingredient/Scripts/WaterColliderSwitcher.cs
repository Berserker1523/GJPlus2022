using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    public class WaterColliderSwitcher : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        BoxCollider2D boxCollider2D;

        //Small Collider
        Vector2 smallSize = new Vector2(0.8491659f, 1.201012f);
        Vector2 smallOffset = new Vector2(-0.006249905f, 0.0359959f);

        //Large collider
        Vector2 largeSize = new Vector2(1.851554f, 4.112803f);
        Vector2 largeOffset= new Vector2(0.01173651f, 1.050415f) ;

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
