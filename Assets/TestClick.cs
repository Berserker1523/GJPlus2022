using UnityEngine;

public class TestClick : MonoBehaviour
{
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(screenToWorld, Vector2.down);
        Debug.Log($"Hit: {(hit.collider != null ? hit.collider.gameObject : "null")}");
    }
}
