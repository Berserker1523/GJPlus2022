using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HistoryBook
{
    public class MoveAlongCurve : MonoBehaviour
    {
        public BezierCurve bezierCurve;
        public Image image;
        public float speed;
        public Scrollbar scrollbar;

        private void Update()
        {
            float t = scrollbar.value; // Obtener el valor de la scrollbar
            if (t <= 0 || t >= 1)
                return;
            
            Vector3 position = bezierCurve.GetPointAt(t); // Obtener la posici�n en la curva de Bezier
            image.rectTransform.position = position; // Mover la imagen a la posici�n calculada
            

            // Si quieres que la imagen gire en la direcci�n de la curva, puedes usar el siguiente c�digo:
            //Vector3 tangent = bezierCurve.get(t);
            //image.rectTransform.rotation = Quaternion.LookRotation(Vector3.forward, tangent);
        }
    }
}
