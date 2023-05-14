using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HistoryBook
{
    public class MoveAlongCurve : MonoBehaviour, IDragHandler
    {
        public BezierCurve bezierCurve;
        public Image image;
        public float speed;
        public Scrollbar scrollbar;

        float minPoint;
        float maxPoint;

        private void Start()
        {
            minPoint = bezierCurve.GetPointAt(0f).y;
            maxPoint = bezierCurve.GetPointAt(1f).y;
        }

        private void Update()
        {
            float t = scrollbar.value; // Obtener el valor de la scrollbar
            if (t <= 0 || t >= 1)
                return;
            
            Vector3 position = bezierCurve.GetPointAt(t); // Obtener la posición en la curva de Bezier
            image.rectTransform.position = position; // Mover la imagen a la posición calculada

            // Si quieres que la imagen gire en la dirección de la curva, puedes usar el siguiente código:
            //Vector3 tangent = bezierCurve.get(t);
            //image.rectTransform.rotation = Quaternion.LookRotation(Vector3.forward, tangent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            float valueT= (eventData.position.y - minPoint ) / (maxPoint - minPoint);
            scrollbar.value = valueT;
        }
    }
}
