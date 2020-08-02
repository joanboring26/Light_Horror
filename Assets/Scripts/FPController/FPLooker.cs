using UnityEngine;

public class FPLooker : MonoBehaviour
{
    public Transform playerTran;
    public float sensitivity = 1;
    public float smoothing = 2;
    public Vector2 MouseLook;
    public Vector2 MouseDelta;
    public float rotVal;

    void Reset()
    {
        playerTran = GetComponentInParent<FPMover>().transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * sensitivity * smoothing);
        MouseDelta = Vector2.Lerp(MouseDelta, smoothMouseDelta, 1 / smoothing);
        MouseLook += MouseDelta;
        MouseLook.y = Mathf.Clamp(MouseLook.y + rotVal, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-MouseLook.y, Vector3.right);
        playerTran.localRotation = Quaternion.AngleAxis(MouseLook.x, Vector3.up);
        rotVal = 0;

        /*
        if(Input.GetKeyDown(KeyCode.E))
        {
            //Ray rc = new Ray();
            RaycastHit rc =  new RaycastHit();
            if(Physics.Raycast(transform.position, transform.forward,out rc, 2.2f, 1 << 0, QueryTriggerInteraction.Collide))
            {
                DoorScript dc = rc.collider.GetComponent<DoorScript>();
                if(dc != null)
                {
                    dc.InteractDoor();
                }
            }
        }
        */
    }
}
