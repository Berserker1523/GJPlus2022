using UnityEngine;

namespace LevelSelector
{
    public class LobbyCamera : MonoBehaviour
    {

        public PolygonCollider2D limitCollider;
        float pointer_x;
        float pointer_y;


        void Update()
        {

        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || PLATFORM_STANDALONE_WIN
            if (Input.GetMouseButton(0) || Input.touchCount == 1)
            {
                if(Input.GetMouseButton(0)) 
                { 
                    pointer_x = Input.GetAxis("Mouse X");
                    pointer_y = Input.GetAxis("Mouse Y");
                }
        #endif

        #if UNITY_ANDROID
                if (Input.touchCount == 1)
                {

                    Touch touchZero = Input.GetTouch(0);
                    if (touchZero.phase == TouchPhase.Moved)
                    {

                        pointer_x = Input.GetTouch(0).deltaPosition.x;
                        pointer_y = Input.GetTouch(0).deltaPosition.y;
                    }
                }
        #endif



                Vector3 vec3 = new Vector3(
                                            Mathf.Clamp(gameObject.transform.position.x -pointer_x, limitCollider.bounds.min.x, limitCollider.bounds.max.x),
                                            Mathf.Clamp(gameObject.transform.position.y -pointer_y, limitCollider.bounds.min.y, limitCollider.bounds.max.y),
                                            0
                                           );

                    gameObject.transform.position = Vector3.MoveTowards(transform.position, vec3, 0.5f);
            }
        }
    }
}
