using UnityEngine;
using UnityEngine.Rendering;

public class AimCursor : MonoBehaviour
{
    Vector3 pos;
    public float speed = 1f;
    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        pos = Input.mousePosition;
        pos.z = speed;
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}
